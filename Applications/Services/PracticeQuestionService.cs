using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeQuestionViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using ClosedXML.Excel;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Net;

namespace Applications.Services
{
    public class PracticeQuestionService : IPracticeQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PracticeQuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> GetPracticeQuestionByPracticeId(Guid PracticeId, int pageIndex = 0, int pageSize = 10)
        {
            var practiceObj = await _unitOfWork.PracticeQuestionRepository.GetAllPracticeQuestionById(PracticeId, pageIndex, pageSize);
            if (practiceObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<PracticeQuestionViewModel>>(practiceObj));
        }

        public async Task<Response> UploadPracticeQuestions(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0) return new Response(HttpStatusCode.Conflict, "File is empty");

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)) return new Response(HttpStatusCode.Conflict, "Not Support file extension");

            var practiceList = new List<PracticeQuestion>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var PracticeID = Guid.Parse(worksheet.Cells[1, 2].Value.ToString());
                    /*                  var isDelete = bool.Parse(worksheet.Cells[2, 2].Value.ToString());*/
                    for (int row = 4; row <= rowCount; row++)
                    {
                        practiceList.Add(new PracticeQuestion
                        {
                            Question = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            Answer = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Note = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            PracticeId = PracticeID,

                        });
                    }
                }
            }
            await _unitOfWork.PracticeQuestionRepository.UploadPracticeListAsync(practiceList);
            await _unitOfWork.SaveChangeAsync();
            return new Response(HttpStatusCode.OK, "OK");
        }

        public async Task<List<PracticeQuestionViewModel>> PracticeQuestionByPracticeId(Guid practiceId)
        {
            var praQObj = await _unitOfWork.PracticeQuestionRepository.GetAllPracticeQuestionByPracticeId(practiceId);
            var result = _mapper.Map<List<PracticeQuestionViewModel>>(praQObj);
            return result;
        }

        public async Task<byte[]> ExportPracticeQuestionByPracticeId(Guid practiceId)
        {
            var practices = await _unitOfWork.PracticeQuestionRepository.GetAllPracticeQuestionByPracticeId(practiceId);
            var practiceQuestionViewModels = _mapper.Map<List<PracticeQuestionViewModel>>(practices);

            // Create a new Excel workbook and worksheet
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Practice Questions");

            // Add the headers to the worksheet
            worksheet.Cell(1, 1).Value = "PracticeID";
            worksheet.Cell(2, 1).Value = "Question";
            worksheet.Cell(2, 2).Value = "Answer";
            worksheet.Cell(2, 3).Value = "Note";

            var questionss = practiceQuestionViewModels[0];
            string stringValue = questionss.PracticeId.ToString();
            worksheet.Cell(1, 2).Value = stringValue;
            // Add the assignment questions to the worksheet
            for (var i = 0; i < practiceQuestionViewModels.Count; i++)
            {
                var question = practiceQuestionViewModels[i];
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

        public async Task<Response> DeletePracticeQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid PracticeId)
        {
            var practice = await _unitOfWork.PracticeQuestionRepository.GetPracticeQuestionListByCreationDate(startDate, endDate, PracticeId);
            if (practice.Count() < 1)
            {
                return new Response(HttpStatusCode.NoContent, "No Practice Question Found");
            }
            _unitOfWork.PracticeQuestionRepository.SoftRemoveRange(practice);
            _unitOfWork.SaveChangeAsync();
            return new Response(HttpStatusCode.OK, "Delete Succeed");
        }
    }
}

