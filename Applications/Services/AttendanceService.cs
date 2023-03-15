using Applications.Interfaces;
using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
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

        public async Task<Response?> UpdateAttendance(DateTime Date,string ClassCode, string Email, AttendenceStatus Status)
        {
            var Class = await _unitOfWork.ClassRepository.GetClassByClassCode(ClassCode);
            var User = await _unitOfWork.UserRepository.GetUserByEmail(Email);
            var AtdObj = await _unitOfWork.AttendanceRepository.GetSingleAttendanceForUpdate(Date, Class.Id, User.Id);

            if (Class == null || User == null)
            {
                return new Response(HttpStatusCode.BadRequest, "Invalid ClassCode or User Email");
            } else if (AtdObj == null)
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
            var Class = new Class();
            Class = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            if (Class != null)
            {
                await _unitOfWork.AttendanceRepository.AddListAttendanceAsync(ClassId, Class.StartDate, Class.EndDate);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return new Response(HttpStatusCode.OK, "Create Attendance Succeed");
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Create Attendance failed,check ClassId again");
        }
    }
}
