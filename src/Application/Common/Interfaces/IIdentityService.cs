using System.Collections.Generic;
using Crypto.Application.Common.Models;
using System.Threading.Tasks;

namespace Crypto.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        string GetFullName(string userId);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string firstName, string lastName,
            string password);

        Task<(Result Result, string UserId)> CreateExternalLoginUserAsync(string email, string firstName,
            string lastName);
        
        Task<Result> DeleteUserAsync(string userId);

        Task<bool> SendConfirmationEmailAsync(string email, string callBackUrl);

        Task<bool> SendConfirmationSmsAsync(string userId, string phoneNumber);

        Task<bool> ConfirmPhoneAsync(string userId, string phoneNumber, string token);
        
        Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task<List<string>> GetUsers();
    }
}
