using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using System.Net;

namespace Applications.Services
{
    public class PracticeService : IPracticeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PracticeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PracticeViewModel> GetPracticeById(Guid Id)
        {
            var praObj = await _unitOfWork.PracticeRepository.GetByIdAsync(Id);
            var result = _mapper.Map<PracticeViewModel>(praObj);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(praObj.CreatedBy);
            result.CreatedBy = createBy.Email;
            return result;
        }
        public async Task<Pagination<PracticeViewModel>> GetPracticeByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10)
        {
            var praObj = await _unitOfWork.PracticeRepository.GetPracticeByUnitId(UnitId);
            var result = _mapper.Map<Pagination<PracticeViewModel>>(praObj);            
            return result;
        }
        public async Task<CreatePracticeViewModel> CreatePracticeAsync(CreatePracticeViewModel PracticeDTO)
        {
            var practiceOjb = _mapper.Map<Practice>(PracticeDTO);
            await _unitOfWork.PracticeRepository.AddAsync(practiceOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreatePracticeViewModel>(practiceOjb);
            }
            return null;
        }
        public async Task<Response> GetAllPractice(int pageIndex = 0, int pageSize = 10)
        {
            var practiceOjb = await _unitOfWork.PracticeRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<PracticeViewModel>>(practiceOjb);
            var guidList = practiceOjb.Items.Select(x => x.CreatedBy).ToList();
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
            if (practiceOjb.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Practice Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
        public async Task<Response> GetPracticeByName(string Name, int pageIndex = 0, int pageSize = 10)
        {
            var practiceOjb = await _unitOfWork.PracticeRepository.GetPracticeByName(Name, pageIndex, pageSize);
            if (practiceOjb.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Practcie Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<PracticeViewModel>>(practiceOjb));
        }
        public async Task<Pagination<PracticeViewModel>> GetDisablePractice(int pageIndex = 0, int pageSize = 10)
        {
            var practices = await _unitOfWork.PracticeRepository.GetDisablePractices();
            var result = _mapper.Map<Pagination<PracticeViewModel>>(practices);
            return result;
        }
        public async Task<Pagination<PracticeViewModel>> GetEnablePractice(int pageIndex = 0, int pageSize = 10)
        {
            var practices = await _unitOfWork.PracticeRepository.GetEnablePractices();
            var result = _mapper.Map<Pagination<PracticeViewModel>>(practices);
            return result;
        }

        public async Task<UpdatePracticeViewModel> UpdatePractice(Guid UnitId, UpdatePracticeViewModel practiceDTO)
        {
            var classObj = await _unitOfWork.PracticeRepository.GetByIdAsync(UnitId);
            if (classObj != null)
            {
                _mapper.Map(practiceDTO, classObj);
                _unitOfWork.PracticeRepository.Update(classObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdatePracticeViewModel>(classObj);
                }
            }
            return null;
        }
    }
}
