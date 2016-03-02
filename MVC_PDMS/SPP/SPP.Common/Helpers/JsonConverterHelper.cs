using Newtonsoft.Json.Converters;
using SPP.Common.Constants;

namespace SPP.Common.Helpers
{
    public class SPPDateTimeConverter : IsoDateTimeConverter
    {
        public SPPDateTimeConverter()
        {
            base.DateTimeFormat = FormatConstants.DateTimeFormatString;
        }
    }

    public class SPPDateTimeConverterToDate : IsoDateTimeConverter
    {
        public SPPDateTimeConverterToDate()
        {
            base.DateTimeFormat = FormatConstants.DateTimeFormatStringByDate;
        }
    }
}
