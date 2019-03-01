using Afonsoft.m3u;
using System.Threading.Tasks;

namespace Afonsoft.Video.Interface
{
    public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
        string GetVideoFile();
        M3UFile GetM3UFile();
        Task<M3UFile> GetM3UFileAsync();
    }
}
