using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Crypto.Application.Common.Interfaces
{
    public interface IImageAccessor
    {
        Task<string> Upload(IFormFile file, string userId, string fileName);
    }
}