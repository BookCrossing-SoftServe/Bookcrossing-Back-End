using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
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
        public GenreService(IRepository<Genre> genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
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
        public async Task<int> Add(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            _genreRepository.Add(genre);
            await _genreRepository.SaveChangesAsync();
            return genre.Id;
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
        public async Task Update(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            _genreRepository.Update(genre);
            await _genreRepository.SaveChangesAsync();
        }
    }
}