using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace PropertyManager.Api.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, Guid entityId);

        Task<string> FileToBase64String(string fileLocation);
    }
}