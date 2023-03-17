using Applications.Interfaces;
using Applications.Services;
using AutoFixture;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Enum.AttendenceEnum;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Mappers;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Applications.Tests.Services.AttendanceServices
{
    public class AttendanceServiceTests : SetupTest
    {
        private readonly AttendanceService _attendanceService;
        public AttendanceServiceTests()
        {
            _attendanceService = new AttendanceService(_unitOfWorkMock.Object, _mapperConfig);
        }
        
        [Fact]
        public async Task ExportAttendanceByClassCodeandDate_ShouldReturnExcelFile()
        {
            // Arrange
            var classCode = "ABC123";
            var date = DateTime.Now.Date;
            var expected = GetExpectedResult();

            var mockService = new Mock<IAttendanceService>();
            mockService.Setup(x => x.ExportAttendanceByClassCodeandDate(classCode, date)).ReturnsAsync(expected);

            var service = mockService.Object;

            // Act
            var result = await service.ExportAttendanceByClassCodeandDate(classCode, date);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<byte[]>();
            result.Should().BeEquivalentTo(expected, options => options
                .WithStrictOrdering());
        }

        private byte[] GetExpectedResult()
        {
            var classObj = new Class { Id = Guid.NewGuid(), ClassCode = "ABC123"};
            var userObj = new User { Id = Guid.NewGuid(), firstName = "John Doe", Email = "john.doe@example.com" };
            var attendanceObj = new Attendance { Id = Guid.NewGuid(), ClassId = classObj.Id, UserId = userObj.Id, Status = AttendenceStatus.Present};

            var attendanceList = new List<Attendance> { attendanceObj };

            using var expectedWorkbook = new XLWorkbook();
            var expectedWorksheet = expectedWorkbook.Worksheets.Add("Attendance");
            expectedWorksheet.Cell(1, 1).Value = "Class Code";
            expectedWorksheet.Cell(1, 2).Value = "Student Name";
            expectedWorksheet.Cell(1, 3).Value = "Student Email";
            expectedWorksheet.Cell(1, 4).Value = "Attendance Status";
            expectedWorksheet.Cell(1, 5).Value = "Attendance Date";
            expectedWorksheet.Cell(2, 1).Value = classObj.ClassCode;
            expectedWorksheet.Cell(2, 2).Value = userObj.firstName;
            expectedWorksheet.Cell(2, 3).Value = userObj.Email;
            expectedWorksheet.Cell(2, 4).Value = attendanceObj.Status.ToString();

            using var expectedStream = new MemoryStream();
            expectedWorkbook.SaveAs(expectedStream);
            var expectedContent = expectedStream.ToArray();

            return expectedContent;
        }
    }
}
