namespace IELTSBlog.Service.Helpers;

public class MediaHelper
{
    public static string MakeImageName(string fileName)
    {
        FileInfo fileInfo = new FileInfo(fileName);
        string extension = fileInfo.Extension;
        string name = "IMG_" + Guid.NewGuid() + extension;
        return name;
    }

    public static string MakeVideoName(string fileName)
    {
        FileInfo fileInfo = new FileInfo(fileName);
        string extension = fileInfo.Extension;
        string name = "VID_" + Guid.NewGuid() + extension;
        return name;
    }
}
