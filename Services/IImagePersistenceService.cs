using System.Windows.Media.Imaging;

namespace PaintToolMvvm
{
    public interface IImagePersistenceService
    {
        BitmapImage LoadImage(string resourceName);
    }
}