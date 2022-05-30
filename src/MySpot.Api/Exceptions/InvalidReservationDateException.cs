namespace MySpot.Api.Exceptions;

public sealed class InvalidReservationDateException : CustomException
{
    public DateTime Date { get; }

    public InvalidReservationDateException(DateTime date) : base($"reservation date: {date} is invalid.")
    {
        Date = date;
    }
}