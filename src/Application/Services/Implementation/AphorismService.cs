using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<AphorismDto> GetAphorismAsync()
        {
            var newAphorism = await _aphorismRepository.FindByCondition(ar => ar.IsCurrent);
            if (newAphorism == null)
            {
                newAphorism = _aphorismRepository.GetAll().FirstOrDefault();
            }
            return _mapper.Map<AphorismDto>(newAphorism);
        }

        public async Task GetNextAsync()
        {
            var oldAphorism = await _aphorismRepository.FindByCondition(ar => ar.IsCurrent);
            Aphorism newAphorism = null;
            if (oldAphorism != null)
            {
                newAphorism = await _aphorismRepository.FindByCondition(aphorism => aphorism.Id > oldAphorism.Id);
                oldAphorism.IsCurrent = false;
                _aphorismRepository.Update(oldAphorism);
            }
            if (newAphorism == null)
            {
                newAphorism = _aphorismRepository.GetAll().FirstOrDefault();
            }
            newAphorism.IsCurrent = true;
            _aphorismRepository.Update(newAphorism);
            await _aphorismRepository.SaveChangesAsync();
        }
    }
}
