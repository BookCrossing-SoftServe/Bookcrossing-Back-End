using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;

namespace Application.Services.Implementation
{
    public class AphorismService : IAphorismService
    {
        private readonly IRepository<Aphorism> _aphorismRepository;
        private readonly IMapper _mapper;

        public AphorismService(IRepository<Aphorism> aphorismRepository, IMapper mapper)
        {
            _aphorismRepository = aphorismRepository;
            _mapper = mapper;
        }

        public async Task<AphorismDto> GetById(int aphorismId)
        {
            return _mapper.Map<AphorismDto>(await _aphorismRepository.FindByIdAsync(aphorismId));
        }
    }
}
