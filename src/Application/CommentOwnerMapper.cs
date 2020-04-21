using AutoMapper;
using Domain.NoSQL.Entities;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public class CommentOwnerMapper : ICommentOwnerMapper
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public CommentOwnerMapper(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Dto.Comment.Book.RootDto>> MapAsync(IEnumerable<BookRootComment> rootEntities)
        {
            IEnumerable<Dto.Comment.Book.RootDto> rootDtos = _mapper.Map<IEnumerable<Dto.Comment.Book.RootDto>>(rootEntities);
            Dictionary<int, Dto.Comment.OwnerDto> users = new Dictionary<int, Dto.Comment.OwnerDto>();

            foreach (Dto.Comment.Book.RootDto dto in rootDtos)
            {
                dto.Comments = await MapChildRecursively(dto.Comments, users);
                if (!users.ContainsKey(dto.Owner.Id))
                {
                    users.Add(dto.Owner.Id, _mapper.Map<Dto.Comment.OwnerDto>(
                        await _userRepository.GetAll().Include(entity => entity.Role).FirstOrDefaultAsync(entity => entity.Id == dto.Owner.Id)));
                }
                dto.Owner = users[dto.Owner.Id];
            }
            return rootDtos;
        }

        public async Task<Dto.Comment.Book.RootDto> MapAsync(BookRootComment rootEntity)
        {
            Dto.Comment.Book.RootDto rootDto = _mapper.Map<Dto.Comment.Book.RootDto>(rootEntity);
            Dictionary<int, Dto.Comment.OwnerDto> users = new Dictionary<int, Dto.Comment.OwnerDto>();

            rootDto.Comments = await MapChildRecursively(rootDto.Comments, users);
            if (!users.ContainsKey(rootDto.Owner.Id))
            {
                users.Add(rootDto.Owner.Id, _mapper.Map<Dto.Comment.OwnerDto>(
                    await _userRepository.GetAll().Include(entity => entity.Role).FirstOrDefaultAsync(entity => entity.Id == rootDto.Owner.Id)));
            }
            rootDto.Owner = users[rootDto.Owner.Id];

            return rootDto;
        }

        protected async Task<IEnumerable<Dto.Comment.Book.ChildDto>> MapChildRecursively(IEnumerable<Dto.Comment.Book.ChildDto> childDtos, Dictionary<int, Dto.Comment.OwnerDto> users)
        {
            foreach (Dto.Comment.Book.ChildDto dto in childDtos)
            {
                dto.Comments = await MapChildRecursively(dto.Comments, users);
                if (!users.ContainsKey(dto.Owner.Id))
                {
                    users.Add(dto.Owner.Id, _mapper.Map<Dto.Comment.OwnerDto>(
                        await _userRepository.GetAll().Include(entity => entity.Role).FirstOrDefaultAsync(entity => entity.Id == dto.Owner.Id)));
                }
                dto.Owner = users[dto.Owner.Id];
            }
            return childDtos;
        }

    }
}
