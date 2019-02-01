using System.Windows;
using System.Windows.Input;

namespace PaintToolMvvm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var contourSvc = new ContourPersistenceService();
            var imageSvc = new ImagePersistenceService();
            _contourVm = new ContourViewModel(contourSvc, imageSvc);
            DataContext = _contourVm;

            var interactionBroker = new ContourInteractionBroker()
            {
                ViewModel = _contourVm,
                InteractionSource = this.interactionSource,
            };
            interactionBroker.IsEnabled = true;
        }

        ContourViewModel _contourVm;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // save the contour
            _contourVm.SaveContour();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {            
            // this is a kludge to keep the tracking point on the interaction source updated
            Point pt = e.GetPosition(this.interactionSource);
            this.interactionSource.TrackMousePosition(pt);
        }
    }
}
