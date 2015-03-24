using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintToolCs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var persistService = new ContourPersistenceService();
            _contourVm = new ContourViewModel(persistService);
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
            // save the contour @ 2.0mm pixel spacing, shifted by 10.0mm
            _contourVm.SaveContour(2.0, 10.0);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {            
            // this is a kludge to keep the tracking point on the interaction source updated
            Point pt = e.GetPosition(this.interactionSource);
            this.interactionSource.TrackMousePosition(pt);
        }
    }
}
