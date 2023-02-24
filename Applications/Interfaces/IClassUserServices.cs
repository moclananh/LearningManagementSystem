using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IClassUserServices
    {
        public Task<List<CreateClassUserViewModel>> ViewAllClassUserAsync();
        Task<Response> UploadClassUserFile(IFormFile formFile);
    }
}
