using System;
using System.Configuration;
using System.Windows.Media.Imaging;

namespace PaintToolMvvm
{
    public class ImagePersistenceService : IImagePersistenceService
    {
        public BitmapImage LoadImage(string resourceName)
        {
#if FROM_RESOURCE
            var fullResourceName = string.Format("pack://application:,,,/PaintToolCs;component/Resources/{0}", resourceName);
            var bi = new BitmapImage(new Uri(fullResourceName, UriKind.Absolute));
#else
            var ptwBase = ConfigurationManager.AppSettings["paintToolWebBase"];
            var ilq = string.Format(ConfigurationManager.AppSettings["imageLoadQuery"], resourceName);
            var queryUrl = string.Format("{0}{1}", ptwBase, ilq);
            var bi = new BitmapImage(new Uri(queryUrl));
#endif
            return bi;
        }
    }
}
