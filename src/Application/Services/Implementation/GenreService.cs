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
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _genreRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        public GenreService(IRepository<Genre> genreRepository, IMapper mapper, IPaginationService paginationService)
        {
            _genreRepository = genreRepository;
            _paginationService = paginationService;
            _mapper = mapper;
        }

        public async Task<GenreDto> GetById(int genreId)
        {
            return _mapper.Map<GenreDto>(await _genreRepository.FindByIdAsync(genreId));
        }

        public async Task<List<GenreDto>> GetAll()
        {
            return _mapper.Map<List<GenreDto>>(await _genreRepository.GetAll().ToListAsync());
        }

        public async Task<GenreDto> Add(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            _genreRepository.Add(genre);
            await _genreRepository.SaveChangesAsync();
            return _mapper.Map<GenreDto>(genre);
        }
        public async Task<bool> Remove(int genreId)
        {
            var genre = await _genreRepository.FindByIdAsync(genreId);
            if (genre == null)
            {
                return false;
            }
            _genreRepository.Remove(genre);
            await _genreRepository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            _genreRepository.Update(genre);
            var affectedRows = await _genreRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<PaginationDto<GenreDto>> GetAll(FullPaginationQueryParams parameters)
        {
            var query = _genreRepository.GetAll();
            return await _paginationService.GetPageAsync<GenreDto, Genre>(query, parameters);
        }
    }
}