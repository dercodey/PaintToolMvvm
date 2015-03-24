using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PaintToolCs
{
    /// <summary>
    /// represents the base image
    /// </summary>
    public class ImageViewModel
    {
        /// <summary>
        /// creates a new ImageViewModel from a static resource
        /// </summary>
        /// <param name="resourceName"></param>
        public static ImageViewModel CreateFromResource(string resourceName)
        {
            var fullResourceName = string.Format("pack://application:,,,/PaintToolCs;component/Resources/{0}", resourceName);
            var bi = new BitmapImage(new Uri(fullResourceName, UriKind.Absolute));
            return new ImageViewModel()
            {
                BackgroundLayer = bi
            };
        }

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
