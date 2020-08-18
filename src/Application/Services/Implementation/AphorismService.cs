using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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

        public async Task<AphorismDto> GetCurrentAphorismAsync(bool current)
        {
            Aphorism newAphorism = null;
            if (current)
            {
                newAphorism = await _aphorismRepository.FindByCondition(ar => ar.IsCurrent);
                if (newAphorism == null)
                {
                    newAphorism = _aphorismRepository.GetAll().FirstOrDefault();
                }
            }
            else
            {
                newAphorism = await _aphorismRepository.FindByCondition(ar => !ar.IsCurrent);
                if (newAphorism == null)
                {
                    newAphorism = _aphorismRepository.GetAll().FirstOrDefault();
                }
            }
            return _mapper.Map<AphorismDto>(newAphorism);
        }

        public async Task MoveToNextAsync()
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

        public async Task<AphorismDto> GetAphorismByIdAsync(int aphorismId)
        {
            return _mapper.Map<AphorismDto>(await _aphorismRepository.FindByIdAsync(aphorismId));
        }

        public async Task<List<AphorismDto>> GetAllAphorismsAsync()
        {
            return _mapper.Map<List<AphorismDto>>(await _aphorismRepository.GetAll().ToListAsync());
        }

        public async Task<AphorismDto> AddAphorismAsync(AphorismDto aphorismDto)
        {
            var aphorism = _mapper.Map<Aphorism>(aphorismDto);
            _aphorismRepository.Add(aphorism);
            await _aphorismRepository.SaveChangesAsync();
            return _mapper.Map<AphorismDto>(aphorism);
        }

        public async Task<bool> RemoveAphorismAsync(int aphorismId)
        {
            var aphorism = await _aphorismRepository.FindByIdAsync(aphorismId);
            if (aphorism == null)
            {
                return false;
            }
            _aphorismRepository.Remove(aphorism);
            await _aphorismRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAphorismAsync(AphorismDto aphorismDto)
        {
            var aphorism = _mapper.Map<Aphorism>(aphorismDto);
            _aphorismRepository.Update(aphorism);
            var affectedRows = await _aphorismRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
