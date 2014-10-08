using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace KrossKlient.Services
{
    public interface IUserService
    {
        Task<BitmapImage> LoadUserImageAsync();
        Task<string> GetCurrentUserAsync();
    }
}