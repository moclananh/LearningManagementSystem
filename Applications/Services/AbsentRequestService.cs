using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AbsentRequest;
using Applications.ViewModels.Response;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net;

namespace Applications.Services
{
    public class AbsentRequestService : IAbsentRequestServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AbsentRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       
        public async Task<Response> GetAllAbsentRequestByEmail(string Email, int pageIndex = 0, int pageSize = 10)
        {
            var AbsentRequestObj = await _unitOfWork.AbsentRequestRepository.GetAllAbsentRequestByEmail(Email, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AbsentRequestViewModel>>(AbsentRequestObj);

            var guidList = AbsentRequestObj.Items.Select(x => x.CreatedBy).ToList();
            var users = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);
            foreach(var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) continue;

                var createdBy = users.FirstOrDefault(x => x.Id == Guid.Parse(item.CreatedBy));
                if (createdBy != null)
                {
                    item.CreatedBy = createdBy.Email;
                }
                return new Response(HttpStatusCode.OK, "Search succeed", result);
            }

            return new Response(HttpStatusCode.OK, "Not found"); ;
        }

        public async Task<Response> GetAbsentById(Guid AbsentId)
        {
            var absObj = await _unitOfWork.AbsentRequestRepository.GetByIdAsync(AbsentId);
            var result = _mapper.Map<AbsentRequestViewModel>(absObj);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(absObj?.CreatedBy);
            if (createBy != null)
            {
                result.CreatedBy = createBy.Email;
            }
            if (absObj == null) return new Response(HttpStatusCode.BadRequest, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", result);
        }

    }
}
