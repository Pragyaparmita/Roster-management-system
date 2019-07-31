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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for HomeScheduleDisplay.xaml
    /// </summary>
    public partial class HomeScheduleDisplay : UserControl
    {
        public HomeScheduleDisplay()
        {
            InitializeComponent();
        }

        public HomeScheduleDisplay(TimeSchedule _TimeSchedule, int GridRow)
        {
            InitializeComponent();
            this.homeTxbEmployee.Text = _TimeSchedule.Name;
            this.homeTxbTime.Text = _TimeSchedule.Time;

            this.scheduleButton.BorderThickness = new Thickness(0, 0, 0, 0);
            if (GridRow % 2 == 0)
            {
                this.scheduleButton.Background = Brushes.Azure;
                this.scheduleButton.BorderBrush = Brushes.Azure;
            }
            else
            {
                var new_color = new BrushConverter();
                this.scheduleButton.Background = (Brush)new_color.ConvertFrom("#FFBDE5E5");
                this.scheduleButton.BorderBrush = (Brush)new_color.ConvertFrom("#FFBDE5E5");
            }
            

            Grid.SetRow(this, GridRow);
            Grid.SetColumn(this, 0);

        }
    }
}
