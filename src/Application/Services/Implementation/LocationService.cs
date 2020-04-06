using Application.Dto;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.RDBMS;
using Domain.RDBMS.Entities;

namespace Application.Services.Implementation
{
    public class LocationService : Interfaces.ILocationService
    {
        private readonly IRepository<Location> _locationRepository;
        private readonly IMapper _mapper;
        public LocationService(IRepository<Location> bookRepository, IMapper mapper)
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
            var location = _mapper.Map<Location>(locationDto);
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
            var location = _mapper.Map<Location>(locationDto);
            _locationRepository.Add(location);
            await _locationRepository.SaveChangesAsync();
            return location.Id;
        }
    }
}
