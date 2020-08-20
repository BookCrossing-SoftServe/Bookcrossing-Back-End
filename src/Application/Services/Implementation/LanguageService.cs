using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> _languageRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public LanguageService(IRepository<Language> languageRepository, IMapper mapper, IPaginationService paginationService)
        {
            _languageRepository = languageRepository;
            _paginationService = paginationService;
            _mapper = mapper;
        }

        public async Task<LanguageDto> GetById(int languageId)
        {
            return _mapper.Map<LanguageDto>(await _languageRepository.FindByIdAsync(languageId));
        }

        public async Task<List<LanguageDto>> GetAll()
        {
            return _mapper.Map<List<LanguageDto>>(await _languageRepository.GetAll().ToListAsync());
        }

        public async Task<LanguageDto> Add(LanguagePostDto languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);
            _languageRepository.Add(language);
            await _languageRepository.SaveChangesAsync();
            return _mapper.Map<LanguageDto>(language);
        }

        public async Task<bool> Remove(int languageId)
        {
            var language = await _languageRepository.FindByIdAsync(languageId);
            if (language == null)
            {
                return false;
            }
            _languageRepository.Remove(language);
            await _languageRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(LanguageDto languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);
            _languageRepository.Update(language);
            var affectedRows = await _languageRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<PaginationDto<LanguageDto>> GetAll(FullPaginationQueryParams parameters)
        {
            var query = _languageRepository.GetAll();
            return await _paginationService.GetPageAsync<LanguageDto, Language>(query, parameters);
        }

    }
}
