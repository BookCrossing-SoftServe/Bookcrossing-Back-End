using Application.Dto.QueryParams.Enums;
using Application.QueryableExtension;

namespace Application.Dto.QueryParams
{
    public class FilterParameters
    {
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public FilterMethod Method { get; set; } = FilterMethod.Contains;
        public FilterOperand Operand { get; set; } = FilterOperand.Or;
    }
}