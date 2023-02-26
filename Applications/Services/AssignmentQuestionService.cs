using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Net;

namespace Applications.Services
{
    public class AssignmentQuestionService : IAssignmentQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AssignmentQuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10)
        {
            var asmQObj = await _unitOfWork.AssignmentQuestionRepository.GetAllAssignmentQuestionByAssignmentId(AssignmentId, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AssignmentQuestionViewModel>>(asmQObj);
            return result;
        }

        public async Task<Response> UploadAssignmentQuestions(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0) return new Response(HttpStatusCode.Conflict, "File is empty");

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)) return new Response(HttpStatusCode.Conflict, "Not Support file extension");

            var assignmentList = new List<AssignmentQuestion>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var AssienmentID = Guid.Parse(worksheet.Cells[1, 2].Value.ToString());
                    var isDelete = bool.Parse(worksheet.Cells[2, 2].Value.ToString());
                    for(int row = 4; row <= rowCount; row++)
                    {
                        assignmentList.Add(new AssignmentQuestion
                        {
                            Question = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            Answer = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Note = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            AssignmentId = AssienmentID,
                            
                        });
                    }
                }
            }
            await _unitOfWork.AssignmentQuestionRepository.UploadAssignmentListAsync(assignmentList);
            await _unitOfWork.SaveChangeAsync();
            return new Response(HttpStatusCode.OK, "OK");
        }
    }
}
