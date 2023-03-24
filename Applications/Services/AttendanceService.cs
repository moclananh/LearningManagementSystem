using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AttendanceViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Enum.AttendenceEnum;
using System.Net;

namespace Applications.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response?> CheckAttendance(string ClassCode, string Email)
        {
            var Class = await _unitOfWork.ClassRepository.GetClassByClassCode(ClassCode);
            var User = await _unitOfWork.UserRepository.GetUserByEmail(Email);
            var AtdObj = await _unitOfWork.AttendanceRepository.GetSingleAttendance(Class.Id, User.Id);

            if (Class == null || User == null || AtdObj == null)
            {
                return new Response(HttpStatusCode.BadRequest, "Invalid ClassCode or User Email");
            }

            if (AtdObj != null)
            {
                AtdObj.Status = Domain.Enum.AttendenceEnum.AttendenceStatus.Present;
                _unitOfWork.AttendanceRepository.Update(AtdObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return new Response(HttpStatusCode.OK, "Check Attendance Succeed", AtdObj.Status);
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Check Attendance Failed");
        }

        public async Task<Response?> UpdateAttendance(DateTime Date, string ClassCode, string Email, AttendenceStatus Status)
        {
            var Class = await _unitOfWork.ClassRepository.GetClassByClassCode(ClassCode);
            var User = await _unitOfWork.UserRepository.GetUserByEmail(Email);
            var AtdObj = await _unitOfWork.AttendanceRepository.GetSingleAttendanceForUpdate(Date, Class.Id, User.Id);

            if (Class == null || User == null)
            {
                return new Response(HttpStatusCode.BadRequest, "Invalid ClassCode or User Email");
            }
            else if (AtdObj == null)
            {
                return new Response(HttpStatusCode.BadRequest, "Attendance date not exist");
            }

            if (AtdObj != null)
            {
                AtdObj.Status = Status;
                _unitOfWork.AttendanceRepository.Update(AtdObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return new Response(HttpStatusCode.OK, "Update Attendance Succeed", AtdObj.Status);
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Update Attendance Failed");
        }



        public async Task<Response> CreateAttendanceAsync(Guid ClassId)
        {

            var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            if (classObj != null)
            {
                await _unitOfWork.AttendanceRepository.AddListAttendanceAsync(ClassId, (DateTime)classObj.StartDate, (DateTime)classObj.EndDate);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return new Response(HttpStatusCode.OK, "Create Attendance Succeed");
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Create Attendance failed,check ClassId again");
        }

        public async Task<byte[]> ExportAttendanceByClassCodeandDate(string ClassCode, DateTime Date)
        {
            // Check if the ClassCode and Date are valid
            if (string.IsNullOrEmpty(ClassCode) || Date == default)
            {
                throw new ArgumentException("Please provide a valid ClassCode and Date.");
            }

            var questions = await _unitOfWork.AttendanceRepository.GetListAttendances(ClassCode, Date);

            // Validate if the questions list is not null or empty
            if (questions == null || !questions.Any())
            {
                throw new Exception("No attendance records found for the given ClassCode and Date.");
            }

            var questionViewModels = _mapper.Map<List<CreateAttendanceViewModel>>(questions);

            // Create a new Excel workbook and worksheet
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Attendance");

            // Add the headers to the worksheet
            worksheet.Cell(1, 1).Value = "ClassCode";
            worksheet.Cell(1, 3).Value = "Date";
            worksheet.Cell(3, 1).Value = "UserName";
            worksheet.Cell(3, 2).Value = "Note";
            worksheet.Cell(3, 3).Value = "Status";

            var questionss = questionViewModels[0];
            worksheet.Cell(1, 2).Value = questionss.ClassCode;
            worksheet.Cell(1, 4).Value = questionss.Date.ToString();

            // Add the assignment questions to the worksheet
            for (var i = 0; i < questionViewModels.Count; i++)
            {
                var question = questionViewModels[i];

                worksheet.Cell(i + 4, 1).Value = question.fullname;
                worksheet.Cell(i + 4, 2).Value = question.Note;
                worksheet.Cell(i + 4, 3).Value = question.Status.ToString();
            }

            // Convert the workbook to a byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return content;
        }

        public async Task<Response> GetAttendanceByFilter(AttendanceFilterViewModel filters, int pageNumber = 0, int pageSize = 10)
        {
            if (filters.StartDate == null)
            {
                filters.StartDate = new DateTime(1999, 1, 1);
            }
            if (filters.EndDate == null)
            {
                filters.EndDate = new DateTime(3999, 1, 1);
            }

            var attendance = await _unitOfWork.AttendanceRepository.GetAllAsync();
            var itemCount = attendance.Count();
            var baseQuery = attendance.Where(s => s.Date >= filters.StartDate && s.Date <= filters.EndDate);
            var user = await _unitOfWork.UserRepository.GetAllAsync();
            var users = user.Where(x => x.Email == filters.Email);

            if (filters.Status.HasValue)
            {
                baseQuery = baseQuery.Where(s => s.Status == filters.Status);
            }
            if (filters.IsDeleted.HasValue)
            {
                baseQuery = baseQuery.Where(s => s.IsDeleted == filters.IsDeleted);
            }
            if (filters.Email != null)
            {
                baseQuery = baseQuery.Where(x => x.User.Email == filters.Email);
            }

            var items = baseQuery.OrderByDescending(s => s.CreationDate)
                                 .Skip(pageNumber * pageSize)
                                 .Take(pageSize)
                                 .ToList();
            var mapping = new Pagination<Attendance>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            var result = _mapper.Map<Pagination<AttendanceViewModel>>(mapping);

            if (mapping.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Attendance found");
            return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
    }
}
