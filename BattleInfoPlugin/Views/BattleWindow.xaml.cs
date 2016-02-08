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
using System.Windows.Shapes;
using MetroRadiance.UI.Controls;

namespace BattleInfoPlugin.Views
{
    /// <summary>
    /// BattleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BattleWindow : MetroWindow
    {
        public BattleWindow()
        {
            InitializeComponent();
            WeakEventManager<Window, EventArgs>.AddHandler(
                Application.Current.MainWindow,
                "Closed",
                (_, __) => this.Close());
        }
    }
}
