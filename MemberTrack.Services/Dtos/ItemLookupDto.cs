using System;
using MemberTrack.Common;

namespace MemberTrack.Services.Dtos
{
    public class ItemLookupDto
    {
        public ItemLookupDto(Enum id) { Id = id; }

        public Enum Id { get; }

        public string Name => Id.ToDescription();
    }
}