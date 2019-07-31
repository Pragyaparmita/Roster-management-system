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
    /// Interaction logic for ScheduleDisplay.xaml
    /// </summary>
    public partial class ScheduleDisplay : UserControl
    {
        public ScheduleDisplay()
        {
            InitializeComponent();
        }


        public ScheduleDisplay(TimeSchedule _TimeSchedule, int GridRow, int GridColum)
        {
            InitializeComponent();
            this.txbEmployee.Text = _TimeSchedule.Name;
            this.txbTime.Text = _TimeSchedule.Time;

            Grid.SetRow(this, GridRow);
            Grid.SetColumn(this, GridColum);

        }
    }
}