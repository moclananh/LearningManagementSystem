using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using Applications.ViewModels.SyllabusViewModels;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing;
using Domain.Entities;
using Domain.EntityRelationship;
using MimeKit.Cryptography;
using System.Net;
using Applications.ViewModels.UnitModuleViewModel;

namespace Applications.Services
{
    public class SyllabusServices : ISyllabusServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;

        public SyllabusServices(IUnitOfWork unitOfWork, IMapper mapper, IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<SyllabusModuleViewModel> AddSyllabusModule(Guid SyllabusId, Guid ModuleId)
        {
            var moduleOjb = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            var unitObj = await _unitOfWork.ModuleRepository.GetByIdAsync(ModuleId);
            if (moduleOjb != null && unitObj != null)
            {
                var SyllabusModule = new SyllabusModule()
                {
                    Syllabus = moduleOjb,
                    Module = unitObj
                };
                await _unitOfWork.SyllabusModuleRepository.AddAsync(SyllabusModule);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<SyllabusModuleViewModel>(SyllabusModule);
                }
            }
            return null;
        }

        public async Task<SyllabusViewModel?> CreateSyllabus(CreateSyllabusViewModel SyllabusDTO)
        {
            var syllabus = _mapper.Map<Syllabus>(SyllabusDTO);
            await _unitOfWork.SyllabusRepository.AddAsync(syllabus);
            var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSucces)
            {
                return _mapper.Map<SyllabusViewModel>(syllabus);
            }
            return null;
        }

        public async Task<Response> GetAllSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.ToPagination(pageNumber, pageSize);
            var result = _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
            var guidList = syllabus.Items.Select(s => s.CreatedBy).ToList();
            var user = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);
            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) { continue; }
                var createBy = user.FirstOrDefault(s => s.Id == Guid.Parse(item.CreatedBy));
                if (createBy != null)
                {
                    item.CreatedBy = createBy.Email;
                }
            }
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetDisableSyllabus(pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<Response> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetEnableSyllabus(pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<Response> GetSyllabusById(Guid SyllabusId)
        {

            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            var result = _mapper.Map<SyllabusViewModel>(syllabus);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(syllabus?.CreatedBy);
            if (createBy != null)
            {
                result.CreatedBy = createBy.Email;
            }
            if (syllabus == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", result);
        }

        public async Task<Response> GetSyllabusByName(string Name, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByName(Name, pageNumber, pageSize);
            var result = _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
            var guidList = syllabus.Items.Select(s => s.CreatedBy).ToList();
            var user = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);
            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) { continue; }
                var createBy = user.FirstOrDefault(s => s.Id == Guid.Parse(item.CreatedBy));
                if (createBy != null)
                {
                    item.CreatedBy = createBy.Email;
                }
            }
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByOutputStandardId(OutputStandardId, pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<Response> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByTrainingProgramId(TrainingProgramId, pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<UpdateSyllabusViewModel?> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusDTO)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            if (syllabus != null)
            {
                _mapper.Map(SyllabusDTO, syllabus);
                _unitOfWork.SyllabusRepository.Update(syllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateSyllabusViewModel>(syllabus);
                }
            }
            return null;
        }

        public async Task<Response> UpdateStatusOnlyOfSyllabus(Guid SyllabusId, UpdateStatusOnlyOfSyllabus SyllabusDTO)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            if (syllabus != null)
            {
                _mapper.Map(SyllabusDTO, syllabus);
                _unitOfWork.SyllabusRepository.Update(syllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return new Response(HttpStatusCode.OK, "update success", syllabus);
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Update failed");
        }

        public async Task<SyllabusModuleViewModel> RemoveSyllabusModule(Guid SyllabusId, Guid ModuleId)
        {
            var SyllabusModule = await _unitOfWork.SyllabusModuleRepository.GetSyllabusModule(SyllabusId, ModuleId);
            if (SyllabusModule != null && !SyllabusModule.IsDeleted)
            {
                SyllabusModule.IsDeleted = true;
                _unitOfWork.SyllabusModuleRepository.Update(SyllabusModule);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<SyllabusModuleViewModel>(SyllabusModule);
                }
            }
            return null;
        }

        public async Task<Response> GetSyllabusDetails(Guid syllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusDetail(syllabusId);
            var result = _mapper.Map<SyllabusViewModel>(syllabus);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(syllabus?.CreatedBy);
            if (createBy != null)
            {
                result.CreatedBy = createBy.Email;
            }
            if (syllabus == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", result);
        }

        public async Task<Response> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetAllSyllabusDetail(pageNumber, pageSize);
            var result = _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
            var guidList = syllabus.Items.Select(s => s.CreatedBy).ToList();
            var user = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);
            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) { continue; }
                var createBy = user.FirstOrDefault(s => s.Id == Guid.Parse(item.CreatedBy));
                if (createBy != null)
                {
                    item.CreatedBy = createBy.Email;
                }
            }
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> GetSyllabusByCreationDate(DateTime startDate, DateTime endDate, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByCreationDate(startDate, endDate, pageNumber, pageSize);
            var result = _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
            var guidList = syllabus.Items.Select(x => x.CreatedBy).ToList();
            var users = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);
            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) continue;

                var createdBy = users.FirstOrDefault(x => x.Id == Guid.Parse(item.CreatedBy));
                if (createdBy != null)
                {
                    item.CreatedBy = createdBy.Email;
                }
            }
            if (syllabus.Items.Count() < 1)
                return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else
                return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> CreateSyllabusDetail(CreateSyllabusDetailModel SyllabusDTO)
        {
            // create mapping for Syllabus
            var syllabus = _mapper.Map<Syllabus>(SyllabusDTO);

            // add List Module and it Realationship
            var listModule = new List<Module>();
            var listModuleSylla = new List<SyllabusModule>();

            // add List Unit and it Realationship
            var listUnit = new List<Unit>();
            var listModuleUnit = new List<ModuleUnit>();

            // add List Quizz, Assignment, Lecture, Practice
            var listQuizz = new List<Quizz>();
            var listAssignment = new List<Assignment>();
            var listLecture = new List<Lecture>();
            var listPractice = new List<Practice>();
            double TotalDurationUnit;
            // loop for mapping and add entity
            if (SyllabusDTO.Modules != null)
            {

                foreach (var module in SyllabusDTO.Modules)
                {
                    // map Unit
                    if (module.Units != null)
                    {
                        foreach (var unit in module.Units)
                        {
                            // map Quizz
                            double checkDurationOnEachUnit = unit.Duration;
                            TotalDurationUnit = checkDurationOnEachUnit;
                            double checkDurationQuizz = 0;
                            var unitmapper = _mapper.Map<Unit>(unit);
                            if (unit.Quizzs != null)
                            {
                                foreach (var quizzes in unit.Quizzs)
                                {
                                    var QuizzMapper = _mapper.Map<Quizz>(quizzes);
                                    QuizzMapper.Unit = unitmapper;
                                    checkDurationQuizz = checkDurationQuizz + QuizzMapper.Duration;
                                }
                            }
                            // map Assignment
                            double CheckDurationAssignment = 0;
                            if (unit.Assignments != null)
                            {
                                foreach (var assignments in unit.Assignments)
                                {
                                    var AssignmentMapper = _mapper.Map<Assignment>(assignments);
                                    AssignmentMapper.Unit = unitmapper;
                                    CheckDurationAssignment = CheckDurationAssignment + AssignmentMapper.Duration;
                                }
                            }

                            // map Lecture
                            double CheckDurationLecture = 0;
                            if (unit.Lectures != null)
                            {
                                foreach (var lectures in unit.Lectures)
                                {
                                    var LecturesMapper = _mapper.Map<Lecture>(lectures);
                                    LecturesMapper.Unit = unitmapper;
                                    CheckDurationLecture = CheckDurationLecture + LecturesMapper.Duration;
                                }
                            }

                            // map Practice
                            double CheckDurationPractice = 0;
                            if (unit.Practices != null)
                            {
                                foreach (var practices in unit.Practices)
                                {
                                    var PracticesMapper = _mapper.Map<Practice>(practices);
                                    PracticesMapper.Unit = unitmapper;
                                    CheckDurationPractice = CheckDurationPractice + PracticesMapper.Duration;
                                }
                            }

                            double SumCheck = CheckDurationAssignment + CheckDurationPractice + CheckDurationLecture + checkDurationQuizz;

                            if (checkDurationOnEachUnit != SumCheck)
                            {
                                return new Response(HttpStatusCode.BadRequest, "Invalid Duration between Unit and it's contain");
                            }

                            foreach(var quizzs in unitmapper.Quizzs)
                            {
                                quizzs.CreationDate = DateTime.Now;
                                quizzs.CreatedBy = _claimService.GetCurrentUserId;
                            }

                            foreach (var assignments in unitmapper.Assignments)
                            {
                                assignments.CreationDate = DateTime.Now;
                                assignments.CreatedBy = _claimService.GetCurrentUserId;
                            }

                            foreach (var lectures in unitmapper.Lectures)
                            {
                                lectures.CreationDate = DateTime.Now;
                                lectures.CreatedBy = _claimService.GetCurrentUserId;
                            }

                            foreach (var practices in unitmapper.Practices)
                            {
                                practices.CreationDate = DateTime.Now;
                                practices.CreatedBy = _claimService.GetCurrentUserId;
                            }
                            listUnit.Add(unitmapper);
                            syllabus.Duration = syllabus.Duration + TotalDurationUnit;
                        }
                    }

                    // map Module
                    var moduleMap = _mapper.Map<Module>(module);
                    listModule.Add(moduleMap);

                    //// add realtionship for ModuleUnit
                    foreach (var unit in listUnit)
                    {
                        var moduleUnit = new ModuleUnit()
                        {
                            Module = moduleMap,
                            Unit = unit,
                        };
                        // add to list
                        listModuleUnit.Add(moduleUnit);
                    }
                }
                // add realtionship for SyllabusModule
                foreach (var item in listModule)
                {
                    var syllabusModule = new SyllabusModule()
                    {
                        Syllabus = syllabus,
                        Module = item,
                    };
                    listModuleSylla.Add(syllabusModule);
                }

                await _unitOfWork.UnitRepository.AddRangeAsync(listUnit);
                await _unitOfWork.ModuleRepository.AddRangeAsync(listModule);
                await _unitOfWork.ModuleUnitRepository.AddRangeAsync(listModuleUnit);
                await _unitOfWork.SyllabusRepository.AddAsync(syllabus);
                await _unitOfWork.SyllabusModuleRepository.AddRangeAsync(listModuleSylla);
                var Succeed = await _unitOfWork.SaveChangeAsync() > 0;

                if (Succeed)
                {
                    return new Response(HttpStatusCode.OK, "Create succeed", _mapper.Map<SyllabusViewModel>(syllabus));
                }
                return new Response(HttpStatusCode.BadRequest, "Create Failed");
            }
            return new Response(HttpStatusCode.BadRequest, "Create Failed");
        }
    }
}