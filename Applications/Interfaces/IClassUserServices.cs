using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IClassUserServices
    {
        public Task<List<CreateClassUserViewModel>> ViewAllClassUserAsync();
        Task<Response> UploadClassUserFile(IFormFile formFile);
        public Task<Response> GetAllClassUsersAsync(int pageIndex = 0, int pageSize = 10);
        Task<byte[]> ExportClassUserByClassCode(Class Class);
    }
}
