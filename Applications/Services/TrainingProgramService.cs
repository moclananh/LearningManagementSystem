using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.TrainingProgramModels;
using Applications.ViewModels.TrainingProgramSyllabi;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainingProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateTrainingProgramSyllabi> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            var SyllabusObj = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProgramId);
            if (SyllabusObj is not null && trainingProgram is not null)
            {
                var trainingProgramSyllabus = new TrainingProgramSyllabus()
                {
                    Syllabus = SyllabusObj,
                    TrainingProgram = trainingProgram
                };
                await _unitOfWork.TrainingProgramSyllabiRepository.AddAsync(trainingProgramSyllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabus);
                }
            }
            return null;
        }

        public async Task<CreateTrainingProgramViewModel?> CreateTrainingProgramAsync(CreateTrainingProgramViewModel TrainingProgramDTO)
        {
            var TrainingProgramObj = _mapper.Map<TrainingProgram>(TrainingProgramDTO);
            await _unitOfWork.TrainingProgramRepository.AddAsync(TrainingProgramObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateTrainingProgramViewModel>(TrainingProgramObj);
            }
            return null;
        }

        public async Task<Pagination<TrainingProgramViewModel>> GetByName(string name, int pageIndex = 0, int pageSize = 10)
        {
            var trainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByName(name, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<TrainingProgramViewModel>>(trainingPrograms);
            return result;
        }

        public async Task<Response> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByClassId(ClassId, pageIndex, pageSize);
            if (TrainingPrograms.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms));
        }

        public async Task<Response> GetTrainingProgramById(Guid TrainingProramId)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProramId);
            var result = _mapper.Map<TrainingProgramViewModel>(TrainingPrograms);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(TrainingPrograms.CreatedBy);
            result.CreatedBy = createBy.Email;
            if (TrainingPrograms is null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<CreateTrainingProgramSyllabi> RemoveSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            var trainingProgramSyllabus = await _unitOfWork.TrainingProgramSyllabiRepository.GetTrainingProgramSyllabus(SyllabusId, TrainingProgramId);
            if (trainingProgramSyllabus is not null)
            {
                _unitOfWork.TrainingProgramSyllabiRepository.SoftRemove(trainingProgramSyllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabus);
                }
            }
            return null;
        }

        public async Task<UpdateTrainingProgramViewModel?> UpdateTrainingProgramAsync(Guid TrainingProgramId, UpdateTrainingProgramViewModel TrainingProgramDTO)
        {
            var TrainingProgramObj = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProgramId);
            if (TrainingProgramObj is not null)
            {
                _mapper.Map(TrainingProgramDTO, TrainingProgramObj);
                _unitOfWork.TrainingProgramRepository.Update(TrainingProgramObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateTrainingProgramViewModel>(TrainingProgramObj);
                }
            }
            return null;
        }

        public async Task<Response> ViewAllTrainingProgramAsync(int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms);
            var guidList = TrainingPrograms.Items.Select(x => x.CreatedBy).ToList();
            foreach (var item in result.Items)
            {
                foreach (var user in guidList)
                {
                    var createBy = await _unitOfWork.UserRepository.GetByIdAsync(user);
                    item.CreatedBy = createBy.Email; 
                }
            }

            if (TrainingPrograms.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No TrainingProgram found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> ViewTrainingProgramDisableAsync(int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramDisable(pageIndex, pageSize);
            if (TrainingPrograms.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms));
        }

        public async Task<Response> ViewTrainingProgramEnableAsync(int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramEnable(pageIndex, pageSize);
            if (TrainingPrograms.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms));
        }

    }
}
