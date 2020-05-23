﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityService _identityService;
        private readonly ILogger<UserController> _logger;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IIdentityService identityService, ILogger<UserController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _identityService = identityService;
            _logger = logger;
        }

        [Route("login")]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid) return View(model);

            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User with Email " + model.Email + " has Logged In");
                if (returnUrl == "/")
                    return RedirectToAction("Index", "Dashboard", new {area = "Panel"});
                return LocalRedirect(returnUrl);
            }

            if (result.IsNotAllowed)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (!user.EmailConfirmed)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callBackUrl = Url.Action("ConfirmEmail", "User", new {userId = user.Id, code},
                        Request.Scheme);

                    await _identityService.SendConfirmationEmailAsync(model.Email, callBackUrl);

                    return RedirectToAction("NotAllowed", new {email = user.Email});
                }
            }

            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            //}
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account with email " + model.Email + "locked out.");
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, "تلاش برای ورود به سیستم نامعتبر است.");
            return View(model);
        }

        [Route("ExternalLogin")]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", new {ReturnUrl = returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl = (returnUrl != null && Url.IsLocalUrl(returnUrl)) ? returnUrl : Url.Content("~/panel/dashboard");
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", remoteError);
                return View("Login", model);
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                ModelState.AddModelError("", "خطایی رخ داد!");
                return View("Login", model);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey, false, true);
            if (signInResult.Succeeded)
                return LocalRedirect(returnUrl);

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var firstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname);

                var (result, userId) = await _identityService.CreateExternalLoginUserAsync(email, firstName, lastName);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    await _userManager.AddLoginAsync(user, externalLoginInfo);
                    await _signInManager.SignInAsync(user, false);

                    return LocalRedirect(returnUrl);
                }
            }

            ViewData["ErrorMessage"] = "متاسفانه خطایی رخ داد!";
            return View("Login", model);
        }

        [Route("register")]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid) return View(model);

            var (result, userId) =
                await _identityService.CreateUserAsync(model.Email, model.FirstName, model.LastName, model.Password);
            var user = await _userManager.FindByIdAsync(userId);

            if (result.Succeeded)
            {
                _logger.LogInformation("user with email " + model.Email + " has created a new account");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action("ConfirmEmail", "User", new {userId, code},
                    Request.Scheme);

                //await _emailSender.SendEmailAsync(model.Email, "تاییدیه ایمیل", "روی لینک رو به رو کلیک کنید : " + callBackUrl);
                await _identityService.SendConfirmationEmailAsync(model.Email, callBackUrl);

                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    return RedirectToAction("RegisterConfirmation", new {email = model.Email});
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(model);
        }

        [Route("email-confirmation")]
        public IActionResult RegisterConfirmation(string email)
        {
            ViewData["Email"] = email;
            return View();
        }

        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("کاربر یافت نشد!");

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return RedirectToAction("Index", "Dashboard", new {area = "Panel"});

            _logger.LogError("error occured while confirming email " + user.Email + " with error : " + result.Errors);
            return View();
        }

        [Route("not-allowed")]
        public IActionResult NotAllowed(string email)
        {
            ViewData["Email"] = email;
            return View();
        }

        [Route("logout")]
        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}