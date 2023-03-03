using Application.Interfaces;
using Application.ViewModels.QuizzViewModels;
using Applications;
using Applications.Commons;
using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusViewModels;
using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.EntityRelationship;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Net;
using Applications.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ClassUserService : IClassUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClassService _classService;
        public ClassUserService(IUnitOfWork unitOfWork, IMapper mapper,IClassService classService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _classService = classService;
        }

        public async Task<Response> GetAllClassUsersAsync(int pageIndex = 0, int pageSize = 10)
        {
            var classUser = await _unitOfWork.ClassUserRepository.ToPagination(pageIndex, pageSize);
            if (classUser.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No ClassUsers Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<CreateClassUserViewModel>>(classUser));
        }

        public async Task<Response> UploadClassUserFile(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0) return new Response(HttpStatusCode.Conflict, "File is empty");

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)) return new Response(HttpStatusCode.Conflict, "Not Support file extension");

            var list = new List<ClassUser>();
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
            
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var clas = await _classService.GetClassByClassCode(worksheet.Cells[1,2].Value.ToString().Trim());
                    if (clas is null)
                    {
                        return new Response(HttpStatusCode.Conflict, "code fail");
                    }
                    // get list user in excel file
                    for (int row = 3; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row,1].Value is null)
                        {
                            break;
                        }
                        var user = await _unitOfWork.UserRepository.GetUserByEmail(worksheet.Cells[row, 3].Value.ToString().Trim());
                        if (user == null) return new Response(HttpStatusCode.BadRequest, $"user with email {worksheet.Cells[row, 3].Value.ToString().Trim()} not exit in system");
                        var clasUser = new ClassUser()
                        {
                            Class = clas,
                            User = user,
                        };
                        list.Add(clasUser);
                    }
                }
            }
            await _unitOfWork.ClassUserRepository.AddRangeAsync(list);
            await _unitOfWork.SaveChangeAsync();
            return new Response(HttpStatusCode.OK, "OK");
        }

        public async Task<List<CreateClassUserViewModel>> ViewAllClassUserAsync()
        {
            var classusers = await _unitOfWork.ClassUserRepository.GetAllAsync();
            var result = _mapper.Map<List<CreateClassUserViewModel>>(classusers);
            return result;
        }
    }    
}
