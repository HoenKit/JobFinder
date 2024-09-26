namespace JobFinder.Models.PayOs
{
    public record Response(
     int error,
     String message,
     object? data
 );
}
