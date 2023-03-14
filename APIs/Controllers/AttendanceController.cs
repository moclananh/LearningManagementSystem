﻿using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.Response;
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
    }
}
