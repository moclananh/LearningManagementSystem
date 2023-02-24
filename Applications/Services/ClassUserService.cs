using Application.Interfaces;
using Applications;
using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.EntityRelationship;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Net;

namespace Application.Services
{
    public class ClassUserService : IClassUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ClassUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new ClassUser
                        {
                            ClassId = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                            UserId = Guid.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()),
                            CreationDate = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString()),
                            IsDeleted = bool.Parse(worksheet.Cells[row, 4].Value.ToString()),
                        });
                    }
                }
            }
            await _unitOfWork.ClassUserRepository.UploadClassUserListAsync(list);
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
