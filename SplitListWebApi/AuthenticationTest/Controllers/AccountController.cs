using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public string Index()
        {
            return "Index Reached";
        }

        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult GoogleLogin(int id)
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest("Login-user not found");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo =
                {info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value, info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value};
            if (result.Succeeded)
                return Ok(userInfo);
            else
            {
                IdentityUser user = new IdentityUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    Id = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value
                };

                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (!identResult.Succeeded) return Forbid();
                identResult = await _userManager.AddLoginAsync(user, info);

                if (!identResult.Succeeded) return Forbid();
                await _signInManager.SignInAsync(user, false);

                return Ok(userInfo);

            }
        }

        [AllowAnonymous]
        [Route("[action]")]
        public string Login()
        {
            return "Login Reached";
        }
    }
}