namespace IELTSBlog.Service.DTOs.Users;

public class UserVerficationDto
{
    public int Code { get; set; }

    public int Attempt { get; set; }

    public DateTime CreatedAt { get; set; }
}