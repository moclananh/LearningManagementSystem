using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.QuizzQuestionViewModels;
using AutoMapper;
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
        public async Task<byte[]> ExportQuizzQuestionByQuizzId(Guid quizzId)
        {
            var questions = await _unitOfWork.QuizzQuestionRepository.GetQuizzQuestionListByQuizzId(quizzId);
            var questionViewModels = _mapper.Map<List<QuizzQuestionViewModel>>(questions);

            // Create a new Excel workbook and worksheet
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Quizz Questions");

            // Add the headers to the worksheet
            worksheet.Cell(1, 1).Value = "Question";
            worksheet.Cell(1, 2).Value = "Answer";
            worksheet.Cell(1, 3).Value = "Note";

            // Add the assignment questions to the worksheet
            for (var i = 0; i < questionViewModels.Count; i++)
            {
                var question = questionViewModels[i];
                worksheet.Cell(i + 2, 1).Value = question.Question;
                worksheet.Cell(i + 2, 2).Value = question.Answer;
                worksheet.Cell(i + 2, 3).Value = question.Note;
            }

            // Convert the workbook to a byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return content;
        }
    }
}
