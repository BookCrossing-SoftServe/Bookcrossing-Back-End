using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.OuterSource;
using Application.Dto.QueryParams;

namespace Application.Services.Interfaces
{
    public interface IOuterBookSourceService
    {
        Task<PaginationDto<OuterBookDto>> SearchBooks(OuterSourceQueryParameters query);
        Task<OuterBookDto> GetBook(int? bookId);
    }
}
