﻿using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusOutputStandardController : ControllerBase
    {
        private readonly ISyllabusOutputStandardService _syllabusOutputStandardService;
        public SyllabusOutputStandardController(ISyllabusOutputStandardService syllabusOutputStandardService)
        {
            _syllabusOutputStandardService = syllabusOutputStandardService;
        }

        [HttpGet("GetAllSyllabusOutputStandard")]
        public async Task<Response> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10)
        {
            return await _syllabusOutputStandardService.GetAllSyllabusOutputStandards(pageIndex, pageSize);
        }

        [HttpPost("AddMultipleOutputStandardsToSyllabus/{syllabusId}"), Authorize(policy: "AuthUser")]
        public async Task<Response> AddMultipleOutputStandardsToSyllabus(Guid syllabusId, List<Guid> outputStandardIds)
        {
            return await _syllabusOutputStandardService.AddMultipleOutputStandardsToSyllabus(syllabusId, outputStandardIds);
        }

    }
}
