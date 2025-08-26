using FD_FE;
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

namespace FD_FE
{
    public partial class UCSlot : UserControl
    {
        public UCSlot(short board_grid_i, float scale = (float)1.8)
        {
            InitializeComponent();
            board_i = board_grid_i;
            Width = slotB.Width *= scale; Height = slotB.Height *= scale;
        }

        public short board_i;

        public delegate void SlotSelectedEventHandler(short selected_slotI);
        static public event SlotSelectedEventHandler SlotSelected;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SlotSelected?.Invoke(board_i);
        }
    }
}
