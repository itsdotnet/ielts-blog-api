namespace IELTSBlog.Domain.Constants;

public class TimeConstants
{
    public static int UTC = 5;
    
    public static DateTime Now() 
        => DateTime.UtcNow.AddHours(UTC);
}