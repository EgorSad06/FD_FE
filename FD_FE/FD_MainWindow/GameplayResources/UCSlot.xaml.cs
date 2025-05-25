using FD_FE;
using FD_MainWindow.GameplayPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static System.Collections.Specialized.BitVector32;

namespace FD_MainWindow
{
    public partial class UCSlot : UserControl
    {
        public UCSlot(short board_grid_i, float scale = (float)1.8)
        {
            InitializeComponent();
            BoardGridI = board_grid_i;
            Width = slotVB.Width *= scale; Height = slotVB.Height *= scale;
        }

        public short BoardGridI;

        public delegate void SlotSelectedEventHandler(short selected_slotI);
        static public event SlotSelectedEventHandler SlotSelected;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SlotSelected?.Invoke(BoardGridI);
        }
    }
}
