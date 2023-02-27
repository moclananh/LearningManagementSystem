using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeQuestionViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
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

        public async Task<Pagination<PracticeQuestionViewModel>> GetPracticeQuestionByPracticeId(Guid PracticeId, int pageIndex = 0, int pageSize = 10)
        {
            var practiceObj = await _unitOfWork.PracticeQuestionRepository.GetAllPracticeQuestionById(PracticeId, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<PracticeQuestionViewModel>>(practiceObj);
            return result;
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
                    var isDelete = bool.Parse(worksheet.Cells[2, 2].Value.ToString());
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

    }
}
