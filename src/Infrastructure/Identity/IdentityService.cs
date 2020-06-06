using System;
using System.Collections.Generic;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Users.Profile.Commands;
using Kavenegar;
using MediatR;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Crypto.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IMediator _mediator;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IMediator mediator,
            IApplicationDbContext context, ILogger<IdentityService> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        public bool SendConfirmationEmailAsync(string email, string callBackUrl)
        {
            try
            {
                var htmlMessage = @"<!DOCTYPE html><html lang='fa' dir='rtl'>
                    <head>
	<meta charset='UTF-8'>
	<meta name='viewport' content='width=device-width, initial-scale=1'>
	<meta http-equiv='X-UA-Compatible' content='ie=edge'>
	<title>لئو کوین</title>

	<!-- Favicon -->
	<link rel='shortcut icon' href='assets/media/image/favicon.png'>

	<!-- Theme Color -->
	<meta name='theme-color' content='#5867dd'>
</head>

<body style='background-color: #eaf0f7;'>

	<table style='font-family: Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; background-color: #eaf0f7; margin: 0; line-height: 2; direction: rtl;' bgcolor='#eaf0f7'>
		<tr style='box-sizing: border-box; margin: 0;'>
			<td style='box-sizing: border-box; vertical-align: top; display: block !important; max-width: 500px !important; clear: both !important; margin: 0 auto;' valign='top'>
				<div style='box-sizing: border-box; max-width: 500px; display: block; margin: 0 auto; padding: 50px 0;'>
					<table width='100%' cellpadding='0' cellspacing='0' itemprop='action' itemscope itemtype='http://schema.org/ConfirmAction' style='box-sizing: border-box; border-radius: 3px; background-color: #fff; margin: 0; border: 1px dashed #4d79f6;' bgcolor='#fff'>
						<tr style='box-sizing: border-box; margin: 0;'>
							<td style='box-sizing: border-box; vertical-align: top; margin: 0; padding: 20px;' valign='top'>
								<meta itemprop='name' content='Confirm Email' style='box-sizing: border-box; margin: 0;'>
								<table width='100%' cellpadding='0' cellspacing='0' style='box-sizing: border-box; margin: 0;'>
									<tr>
										<td><a href='#'><img src='~/assets/media/image/logo-sm.png' alt='image' style='margin-left: auto; margin-right: auto; display:block; margin-bottom: 10px; height: 40px;'></a></td>
									</tr>
									<tr style='box-sizing: border-box; margin: 0;'>
										<td style='box-sizing: border-box; color: #5867dd; font-size: 24px; font-weight: 700; text-align: center; vertical-align: top; margin: 0; padding: 0 0 10px;' valign='top'>به لئو-کوین خوش آمدید</td>
									</tr>
									<tr style='box-sizing: border-box; margin: 0;'>
										<td style='box-sizing: border-box; color: #3f5db3; vertical-align: top; margin: 0; padding: 10px 10px;' valign='top'>لطفا با کلیک بر روی لینک زیر ایمیل خود را تایید کنید.</td>
									</tr>
									<tr style='box-sizing: border-box; margin: 0;'>
										<td style='box-sizing: border-box; vertical-align: top; margin: 0; padding: 10px 10px;' valign='top'>ما نیاز به ارسال اطلاعات حساس سرویسمان به ایمیل شما داریم. لذا وارد کردن ایمیل صحیح اهمیت بالایی دارد.</td>
									</tr>
									<tr style='box-sizing: border-box; margin: 0;'>
										<td itemprop='handler' itemscope itemtype='http://schema.org/HttpActionHandler' style='box-sizing: border-box; vertical-align: top; margin: 0; padding: 10px 10px;' valign='top'><a href='" +
                                  callBackUrl +
                                  @"' itemprop='url' style='box-sizing: border-box; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold; text-align: center; cursor: pointer; display: block; border-radius: 5px; text-transform: capitalize; background-color: #5867dd; margin: 0; border-color: #5867dd; border-style: solid; border-width: 10px 20px;'>تایید آدرس ایمیل</a></td>
									</tr>
									<tr style='box-sizing: border-box; margin: 0;'>
										<td style='box-sizing: border-box; padding-top: 5px; vertical-align: top; margin: 0; text-align: right;' valign='top'><b>Leo-Coin</b></td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
	</table>

</body>

</html>";
                _emailSender.SendEmailAsync(email, "تاییدیه ایمیل", htmlMessage);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while sending confirmation email to " + email + " : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SendConfirmationSmsAsync(string userId, string phoneNumber)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);

                var api = new KavenegarApi("API CODE");
                var result = api.Send("SENDER CODE", phoneNumber, "MESSAGE TEXT " + token);

                return result.Status == 200;
            }
            catch
            {
                throw new Exception("error!");
            }
        }

        public async Task<bool> ConfirmPhoneAsync(string userId, string phoneNumber, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber);
                if (!result) return false;

                await AddConfirmsClaim(user.Id, "PhoneNumber");
                return true;
            }
            catch
            {
                throw new Exception("error");
            }
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public string GetFullName(string userId)
        {
            var profile = _context.UsersProfiles.FirstOrDefault(f => f.UserId == userId);
            return profile?.FirstName + " " + profile?.LastName;
        }

        public string GetSheba(string userId)
        {
            var financial = _context.FinancialInformation.FirstOrDefault(f => f.UserId == userId);
            return financial?.Sheba;
        }

        public string GetCardNumber(string userId)
        {
            var financial = _context.FinancialInformation.FirstOrDefault(f => f.UserId == userId);
            return financial?.CardNumber;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string firstName,
            string lastName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return (result.ToApplicationResult(), user.Id);


            await _userManager.AddClaimAsync(user, new Claim("identifier", user.Id));

            var command = new CreateProfileCommand
            {
                FirstName = firstName,
                LastName = lastName,
                UserId = user.Id
            };
            await _mediator.Send(command);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<(Result Result, string UserId)> CreateExternalLoginUserAsync(string email, string firstName,
            string lastName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded) return (result.ToApplicationResult(), user.Id);
                var command = new CreateProfileCommand
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserId = user.Id
                };
                await _mediator.Send(command);

                return (result.ToApplicationResult(), user.Id);
            }

            throw new NotFoundException();
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), userId);

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.ToApplicationResult();
        }

        public async Task<List<string>> GetUsers()
        {
            return await _userManager.Users.Select(f => f.Id).ToListAsync();
        }

        public async Task AddConfirmsClaim(string userId, string value)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddClaimAsync(user, new Claim("ConfirmationType", value));
        }

        public async Task RemoveConfirmsClaim(string userId, string value)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveClaimAsync(user, new Claim("ConfirmationType", value));
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}