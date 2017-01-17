using System.IO;

namespace Business.CommonInfrastructure.Interfaces
{
    public interface IFileDataSource
    {
        FileStream Stream(string path);
    }
}