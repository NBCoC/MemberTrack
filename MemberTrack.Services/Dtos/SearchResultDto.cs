using System.Collections.Generic;
using System.Linq;

namespace MemberTrack.Services.Dtos
{
    public class SearchResultDto<T>
    {
        public SearchResultDto(IEnumerable<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public int Count => Data.Count();

        public int TotalCount { get; }

        public IEnumerable<T> Data { get; }
    }
}