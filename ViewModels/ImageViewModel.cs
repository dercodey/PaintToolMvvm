using System.Windows.Media.Imaging;

namespace PaintToolMvvm
{
    /// <summary>
    /// represents the base image
    /// </summary>
    public class ImageViewModel
    {
        IImagePersistenceService _imageSvc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ips"></param>
        public ImageViewModel(IImagePersistenceService ips)
        {
            _imageSvc = ips;
        }

        /// <summary>
        /// creates a new ImageViewModel from a static resource
        /// </summary>
        /// <param name="resourceName"></param>
        public static 
            ImageViewModel 
                CreateFromResource(IImagePersistenceService ips, string resourceName) =>

            // create new image view model
            new ImageViewModel(ips)
            {
                BackgroundLayer = ips.LoadImage(resourceName)
            };

        /// <summary>
        /// expose it as a pseudo-background layer
        /// </summary>
        public BitmapSource BackgroundLayer
        {
            get;
            set;
        }
    }
}
