namespace PiggsCare.Domain.Services
{
    public class DateConverterService:IDateConverterService
    {
        public DateOnly GetDateOnly( DateTime dateTime )
        {
            return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public DateTime GetDateTime( DateOnly dateOnly )
        {
            return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
        }
    }
}
