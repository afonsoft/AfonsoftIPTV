using Afonsoft.m3u;
using System.Threading.Tasks;

namespace AfonsoftIPTV.Interface
{
    public interface IM3UPicker
    {
        M3UFile GetM3UFile();
        Task<M3UFile> GetM3UFileAsync();
    }
}
