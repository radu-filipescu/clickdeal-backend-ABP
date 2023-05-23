using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace clickdeal.UserProfile
{
    public class UserProfileAppService : AccountAppService
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly IdentityUserManager _userManager;
        private readonly ICurrentUser _currentUser;

        public UserProfileAppService(IdentityUserManager userManager, IIdentityRoleRepository roleRepository, IAccountEmailer accountEmailer, IdentitySecurityLogManager identitySecurityLogManager, IOptions<IdentityOptions> identityOptions, ICurrentUser currentUser) : base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            _roleRepository = roleRepository;
            _identityOptions = identityOptions;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public class EditUserProfileDTO
        {
            public string Username { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
        }

        public class EditUserProfileResponseDTO
        {
            public bool Result { get; set; }
        }

        [IgnoreAntiforgeryToken]
        public async Task<EditUserProfileResponseDTO> EditUserProfile(EditUserProfileDTO input)
        {
            // TODO: validations !!!
            
            EditUserProfileResponseDTO badResponse = new EditUserProfileResponseDTO();
            badResponse.Result = false;

            if (input == null)
                return badResponse;

            Guid? userId = _currentUser.Id;

            if (userId == null)
                return badResponse;

            IdentityUser currentUser = await _userManager.GetByIdAsync((Guid)userId);

            if (input.Username != null && input.Username.Length > 0)
                await _userManager.SetUserNameAsync(currentUser, input.Username);

            if (input.Name != null && input.Name.Length > 0)
                currentUser.Name = input.Name;

            if (input.Surname != null && input.Surname.Length > 0)
                currentUser.Surname = input.Surname;

            if (input.PhoneNumber != null && input.PhoneNumber.Length > 0)
                await _userManager.SetPhoneNumberAsync(currentUser, input.PhoneNumber);

            var result = await _userManager.UpdateAsync(currentUser);

            EditUserProfileResponseDTO goodResponse = new EditUserProfileResponseDTO();
            goodResponse.Result = true;

            return goodResponse;
        }
    }


}
