using Applications.Interfaces;
using Applications.ViewModels.AttendanceViewModels;
using Applications.ViewModels.Response;
using Domain.Enum.AttendenceEnum;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class AttendanceController : ControllerBase
    {

        private readonly IAttendanceService _attendanceService;
        private readonly IValidator<AttendanceFilterViewModel> _validatorFilter;

        public AttendanceController(IAttendanceService attendanceService,
            IValidator<AttendanceFilterViewModel> validatorFilter)
        {
            _attendanceService = attendanceService;
            _validatorFilter = validatorFilter;
        }

        [HttpPost("GetAttendanceByFilter")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> GetAttendanceByFilter(AttendanceFilterViewModel filter, int pageNumber = 0, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var validation = _validatorFilter.Validate(filter);
                if (validation.IsValid)
                {
                    var attendance = await _attendanceService.GetAttendanceByFilter(filter, pageNumber = 0, pageSize = 10);
                    if (attendance != null)
                    {
                        return Ok(attendance);
                    }
                    return BadRequest("Not found");
                }
            }
            return BadRequest("GetAttendanceByFilter Fail");
        }

        [HttpPost("CreateAttendanceByClassId/{ClassId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> CreateAttendanceByClassId(Guid ClassId) => await _attendanceService.CreateAttendanceAsync(ClassId);

        [HttpPut("CheckAttendance/{ClassCode}/{Email}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> CheckAttendance(string ClassCode, string Email) => await _attendanceService.CheckAttendance(ClassCode, Email) ;

        [HttpGet("ExportAttendance/{ClassCode}/{Date}")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> ExportAttendanceByClassCodeandDate(string ClassCode, DateTime Date)
        {
            if (string.IsNullOrEmpty(ClassCode) || Date == default)
            {
                return BadRequest("Please provide a valid ClassCode and Date.");
            }
            try
            {
                var content = await _attendanceService.ExportAttendanceByClassCodeandDate(ClassCode, Date);

                var fileName = $"Attendance_ClassCode{ClassCode}_{Date}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                // Log the exception or return an error response
                return BadRequest($"An error occurred while exporting attendance: {ex.Message}");
            }
        }

        [HttpPut("UpdateAttendance/{ClassCode}/{Email}/{Status}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> UpdateAttendance(DateTime Date, string ClassCode, string Email,AttendenceStatus Status) => await _attendanceService.UpdateAttendance(Date, ClassCode, Email, Status);
    }
}
