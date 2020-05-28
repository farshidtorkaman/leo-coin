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

namespace Crypto.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IMediator _mediator;

        public IdentityService(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IMediator mediator, IApplicationDbContext context)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _mediator = mediator;
            _context = context;
        }

        public async Task<bool> SendConfirmationEmailAsync(string email, string callBackUrl)
        {
            try
            {
                await _emailSender.SendEmailAsync(email, "تاییدیه ایمیل", "روی لینک رو به رو کلیک کنید : " + callBackUrl);
                return true;
            }
            catch (Exception ex)
            {
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

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string firstName, string lastName, string password)
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

        public async Task<(Result Result, string UserId)> CreateExternalLoginUserAsync(string email, string firstName, string lastName)
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
