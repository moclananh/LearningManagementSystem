using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using ClosedXML.Excel;
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

        public async Task<Response> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10)
        {
            var asmObj = await _unitOfWork.AssignmentQuestionRepository.GetAllAssignmentQuestionByAssignmentId(AssignmentId, pageIndex, pageSize);
            if (asmObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No AssignmentQuestion Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AssignmentQuestionViewModel>>(asmObj));
        }

        public async Task<Response> UploadAssignmentQuestions(IFormFile formFile)
        {
            if (formFile is not object || formFile.Length <= 0) return new Response(HttpStatusCode.Conflict, "File is empty");

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
                    for (int row = 4; row <= rowCount; row++)
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


        public async Task<byte[]> ExportAssignmentQuestionByAssignmentId(Guid assignmentId)
        {
            var questions = await _unitOfWork.AssignmentQuestionRepository.GetAssignmentQuestionListByAssignmentId(assignmentId);
            var questionViewModels = _mapper.Map<List<AssignmentQuestionViewModel>>(questions);

            // Create a new Excel workbook and worksheet
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Assignment Questions");

            // Add the headers to the worksheet
            worksheet.Cell(1, 1).Value = "AsignmentID";
            worksheet.Cell(2, 1).Value = "Question";
            worksheet.Cell(2, 2).Value = "Answer";
            worksheet.Cell(2, 3).Value = "Note";

            var questionss = questionViewModels[0];
            string stringValue = questionss.AssignmentId.ToString();
            worksheet.Cell(1, 2).Value = stringValue;
            // Add the assignment questions to the worksheet
            for (var i = 0; i < questionViewModels.Count; i++)
            {
                var question = questionViewModels[i];
                worksheet.Cell(i + 3, 1).Value = question.Question;
                worksheet.Cell(i + 3, 2).Value = question.Answer;
                worksheet.Cell(i + 3, 3).Value = question.Note;
            }

            // Convert the workbook to a byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return content;
        }

        public async Task<Response> DeleteAssignmentQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid AssignmenId)
        {
            var assignment = await _unitOfWork.AssignmentQuestionRepository.GetAssignmentQuestionListByCreationDate(startDate, endDate, AssignmenId);
            if (assignment.Count() < 1)
            {
                return new Response(HttpStatusCode.NoContent, "Not Found");
            }
            _unitOfWork.AssignmentQuestionRepository.SoftRemoveRange(assignment);
            _unitOfWork.SaveChangeAsync();
            return new Response(HttpStatusCode.OK, "Delete Succeed");
        }
    }
}
