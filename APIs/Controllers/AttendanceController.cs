using Applications.Interfaces;
using Applications.ViewModels.Response;
using Domain.Enum.AttendenceEnum;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {

        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("CreateAttendanceByClassId/{ClassId}")]
        public async Task<Response> CreateAttendanceByClassId(Guid ClassId) => await _attendanceService.CreateAttendanceAsync(ClassId);

        [HttpPut("CheckAttendance/{ClassCode}/{Email}")]
        public async Task<Response> CheckAttendance(string ClassCode, string Email) => await _attendanceService.CheckAttendance(ClassCode, Email) ;

        [HttpGet("ExportAttendance/{ClassCode}/{Date}")]
        public async Task<IActionResult> ExportAttendanceByClassIdandDate(string ClassCode, DateTime Date)
        {
            try
            {
                var content = await _attendanceService.ExportAttendanceByClassIDandDate(ClassCode, Date);

                var fileName = $"Attendance_{ClassCode}_{Date}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while exporting attendance: Plaese check Classcode and Date");
            }
        }
        [HttpPut("UpdateAttendance/{ClassCode}/{Email}/{Status}")]
        public async Task<Response> UpdateAttendance(DateTime Date, string ClassCode, string Email,AttendenceStatus Status) => await _attendanceService.UpdateAttendance(Date, ClassCode, Email, Status);
    }
}
