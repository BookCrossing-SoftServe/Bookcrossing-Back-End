using Application.Dto;
using AutoMapper;
using Domain.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class Location : Interfaces.ILocation
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public Location(ILocationRepository bookRepository, IMapper mapper)
        {
            _locationRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<LocationDto> GetById(int locationId)
        {
            return _mapper.Map<LocationDto>(await _locationRepository.GetAll()
                                                               .Include(p => p.UserLocation)                                                              
                                                               .FirstOrDefaultAsync(p => p.Id == locationId));
        }

        public async Task<List<LocationDto>> GetAll()
        {
            return _mapper.Map<List<LocationDto>>(await _locationRepository.GetAll()
                                                                   .Include(p => p.UserLocation)                                                                 
                                                                   .ToListAsync());
        }

        public async Task Update(LocationDto locationDto)
        {
            var location = _mapper.Map<Entities.Location>(locationDto);
            _locationRepository.Update(location);
            await _locationRepository.SaveChangesAsync();
        }

        public async Task<LocationDto> Remove(int locationId)
        {
            var location = await _locationRepository.FindByIdAsync(locationId);
            if (location == null)
                return null;
            _locationRepository.Remove(location);
            await _locationRepository.SaveChangesAsync();
            return _mapper.Map<LocationDto>(location);
        }

        public async Task<int> Add(LocationDto locationDto)
        {
            var location = _mapper.Map<Entities.Location>(locationDto);
            _locationRepository.Add(location);
            await _locationRepository.SaveChangesAsync();
            return location.Id;
        }
    }
}
