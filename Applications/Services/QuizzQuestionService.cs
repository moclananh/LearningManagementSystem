using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Net;
using Applications.ViewModels.QuizzQuestionViewModels;
using ClosedXML.Excel;

namespace Applications.Services
{
    public class QuizzQuestionService : IQuizzQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QuizzQuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<QuizzQuestionViewModel> AddQuestion(QuizzQuestionViewModel question)
        {
            var questionObj = _mapper.Map<QuizzQuestion>(question);
            await _unitOfWork.QuizzQuestionRepository.AddAsync(questionObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<QuizzQuestionViewModel>(questionObj);
            }
            return null;
        }

        public async Task<Pagination<QuizzQuestionViewModel>> GetQuizzQuestionByQuizzId(Guid QuizzId, int pageIndex = 0, int pageSize = 10)
        {
            var questionObj = await _unitOfWork.QuizzQuestionRepository.GetQuestionByQuizzId(QuizzId, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<QuizzQuestionViewModel>>(questionObj);
            return result;
        }

        public async Task<QuizzQuestionViewModel> UpdateQuestion(Guid QuizzQuestionId, QuizzQuestionViewModel question)
        {
            var questionObj = await _unitOfWork.QuizzQuestionRepository.GetByIdAsync(QuizzQuestionId);
            if (questionObj != null)
            {
                _mapper.Map(question, questionObj);
                _unitOfWork.QuizzQuestionRepository.Update(questionObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<QuizzQuestionViewModel>(questionObj);
                }
            }
            return null;
        }

        public async Task<Response> UploadQuizzQuestion(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0) return new Response(HttpStatusCode.Conflict, "File is empty");

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)) return new Response(HttpStatusCode.Conflict, "Not Support file extension");

            var questionList = new List<QuizzQuestion>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var QuizzID = Guid.Parse(worksheet.Cells[1, 2].Value.ToString());
                    for (int row = 4; row <= rowCount; row++)
                    {
                        questionList.Add(new QuizzQuestion
                        {
                            Question = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            Answer = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Note = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            QuizzId = QuizzID,
                        });
                    }
                }
            }
            await _unitOfWork.QuizzQuestionRepository.AddRangeAsync(questionList);
            await _unitOfWork.SaveChangeAsync();
            return new Response(HttpStatusCode.OK, "OK");
        }
        public async Task<byte[]> ExportQuizzQuestionByQuizzId(Guid quizzId)
        {
            var questions = await _unitOfWork.QuizzQuestionRepository.GetQuizzQuestionListByQuizzId(quizzId);

            if (questions == null || questions.Count == 0)
            {
                throw new ArgumentException("Invalid QuizzId.");
            }

            var questionViewModels = _mapper.Map<List<QuizzQuestionViewModel>>(questions);

            // Create a new Excel workbook and worksheet
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Quizz Questions");

            // Add the headers to the worksheet
            worksheet.Cell(1, 1).Value = "QuizzID";
            worksheet.Cell(2, 1).Value = "Question";
            worksheet.Cell(2, 2).Value = "Answer";
            worksheet.Cell(2, 3).Value = "Note";

            var questionss = questionViewModels[0];
            string stringValue = questionss.QuizzId.ToString();
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

        public async Task<Response> DeleteQuizzQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid QuizzId)
        {
            var quizz = await _unitOfWork.QuizzQuestionRepository.GetQuizzQuestionListByCreationDate(startDate, endDate, QuizzId);
            if (quizz.Count() < 1)
                return new Response(HttpStatusCode.NoContent, "Not Found");
            else
            {
                _unitOfWork.QuizzQuestionRepository.SoftRemoveRange(quizz);
                _unitOfWork.SaveChangeAsync();
                return new Response(HttpStatusCode.OK, "Delete Succeed");
            }
        }
    }
}
