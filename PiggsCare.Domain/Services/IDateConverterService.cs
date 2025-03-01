namespace PiggsCare.Domain.Services
{
    public interface IDateConverterService
    {
        DateOnly GetDateOnly( DateTime dateTime );

        DateTime GetDateTime( DateOnly dateOnly );
    }
}
