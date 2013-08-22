using System;

namespace DateI18N.Models
{
    public class DateWithTimeZone
    {
        private readonly DateTime _dateTimeInfo;
        private readonly TimeZoneInfo _timeZoneInfo;

        public DateWithTimeZone(DateTime dateTimeInfo, TimeZoneInfo timeZoneInfo)
        {
            if (timeZoneInfo == null) throw new ArgumentNullException("timeZoneInfo");

            _timeZoneInfo = timeZoneInfo;
            _dateTimeInfo = ConvertDateToUtc(dateTimeInfo);
        }

        private DateTime ConvertDateToUtc(DateTime dateTimeInfo)
        {
            if (dateTimeInfo.Kind == DateTimeKind.Utc)
            {
                return dateTimeInfo;
            }

            var dateUnspecified = DateTime.SpecifyKind(dateTimeInfo, DateTimeKind.Unspecified);
            var dateUtc = TimeZoneInfo.ConvertTimeToUtc(dateUnspecified, _timeZoneInfo);

            return dateUtc;
        }

        public DateTime GetUtcDateTime()
        {
            return _dateTimeInfo;
        }

        public DateTime GetUserDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(_dateTimeInfo, _timeZoneInfo);
        }
    }
}
