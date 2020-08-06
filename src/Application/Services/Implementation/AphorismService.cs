using System.Linq;
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

        /// <inheritdoc />
        public async Task<AphorismDto> GetNextAsync()
        {
            var newAphorism = await _aphorismRepository.FindByCondition(ar => !ar.IsCurrent);
            if(newAphorism == null)
            {
               var aphorismes = _aphorismRepository.GetAll();
                aphorismes.ToList().ForEach(a => {
                    a.IsCurrent = !a.IsCurrent;
                    _aphorismRepository.Update(a);
                });
                await _aphorismRepository.SaveChangesAsync();
                newAphorism = await _aphorismRepository.FindByCondition(ar => !ar.IsCurrent);
            }
            newAphorism.IsCurrent = !newAphorism.IsCurrent;
            _aphorismRepository.Update(newAphorism);
            await _aphorismRepository.SaveChangesAsync();
            return _mapper.Map<AphorismDto>(newAphorism);
        }
    }
}
