using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PaintToolCs
{
    /// <summary>
    /// broker to set up the paint tool for contouring
    /// </summary>
    public class ContourInteractionBroker
    {
        /// <summary>
        /// contour view model
        /// </summary>
        public ContourViewModel ViewModel
        {
            get;
            set;
        }

        /// <summary>
        /// paint tool interaction source
        /// </summary>
        public PaintToolInteractionSource InteractionSource
        {
            get;
            set;
        }

        /// <summary>
        /// turn on/off painting
        /// </summary>
        public bool IsEnabled
        {
            get { return InteractionSource.IsEnabled; }
            set
            {
                // turn on the source
                InteractionSource.IsEnabled = value;

                // hook up binding if we are turned on
                if (InteractionSource.IsEnabled)
                { 
                    // hook up the binding
                    var binding = new Binding("ContourGeometry");
                    binding.Source = ViewModel;
                    binding.Mode = BindingMode.TwoWay;

                    InteractionSource.SetBinding(
                        PaintToolInteractionSource.CurrentPathGeometryProperty,
                        binding);
                }
            }
        }
    }
}
