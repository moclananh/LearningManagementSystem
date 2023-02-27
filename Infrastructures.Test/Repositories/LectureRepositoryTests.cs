﻿using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructures.Tests.Repositories
{
    public class LectureRepositoryTests : SetupTest
    {
        private readonly ILectureRepository _lectureRepository;
        public LectureRepositoryTests()
        {
            _lectureRepository = new LectureRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }
        [Fact]
        public async Task LectureRepository_GetLectureByName_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Lecture>()
                            .Without(x => x.Unit)
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Lectures.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            int i = 0;
            foreach (var item in mockData)
            {
                item.LectureName = $"Mock{i}";
                i++;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.LectureName.Contains("Mock"))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _lectureRepository.GetLectureByName("Mock");
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task LectureRepository_GetLectureByUnitId_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Lecture>()
                            .Without(x => x.Unit)
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Lectures.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var i = Guid.NewGuid();
            foreach (var item in mockData)
            {
                item.UnitId = i;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.UnitId.Equals(i))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _lectureRepository.GetLectureByUnitId(i);
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task LectureRepository_GetEnableLectures_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Lecture>()
                            .Without(x => x.Unit)
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Lectures.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Enable;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _lectureRepository.GetEnableLectures();
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task ClassRepository_GetDiableLectures_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Lecture>()
                            .Without(x => x.Unit)
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Lectures.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Disable;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _lectureRepository.GetDisableLectures();
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }
    }
}

