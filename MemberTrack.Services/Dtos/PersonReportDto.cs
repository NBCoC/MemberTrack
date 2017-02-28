namespace MemberTrack.Services.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class PersonReportDto
    {
        public PersonReportDto()
        {
            var items = new List<PersonReportItemDto>();

            var month = DateTimeOffset.Now.Month;

            for (var i = 1; i < 13; i++)
            {
                month = month == 12 ? 1 : month + 1;

                items.Add(
                    new PersonReportItemDto
                    {
                        Month = month,
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)
                    });
            }

            Items = items;
        }

        public IEnumerable<PersonReportItemDto> Items { get; private set; }
    }
}