using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiFormat;
using ApiFormat.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public AccountController(UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            var user = new UserModel
            {
                Name = info.Principal.FindFirst(ClaimTypes.Name).Value,
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                Id = Double.Parse(info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value),
                EmailConfirmed = true
            };

            var loginInfo = new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.Principal.FindFirst(ClaimTypes.Name).Value);

            var result = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);

            if (result.Succeeded)
                return Ok(user);
            else
            {
                var identityResult = await _userManager.CreateAsync(user);

                if (!identityResult.Succeeded) return Forbid();
                identityResult = await _userManager.AddLoginAsync(user, info);

                if (!identityResult.Succeeded) return Forbid();
                await _signInManager.SignInAsync(user, false);

                return Ok(user);
            }
        }
    }
}