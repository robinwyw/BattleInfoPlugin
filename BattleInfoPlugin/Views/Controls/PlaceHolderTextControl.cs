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

namespace BattleInfoPlugin.Views.Controls
{
    public class PlaceHolderTextControl : Control
    {
        static PlaceHolderTextControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceHolderTextControl), new FrameworkPropertyMetadata(typeof(PlaceHolderTextControl)));
        }

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public static DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(PlaceHolderTextControl), new UIPropertyMetadata(""));

        public string PlaceHolder
        {
            get { return (string)this.GetValue(PlaceHolderProperty); }
            set { this.SetValue(PlaceHolderProperty, value); }
        }

        public static DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register(nameof(PlaceHolder), typeof(string), typeof(PlaceHolderTextControl), new UIPropertyMetadata(""));

        public Brush PlaceHolderBrush
        {
            get { return (Brush)this.GetValue(PlaceHolderBrushProperty); }
            set { this.SetValue(PlaceHolderBrushProperty, value); }
        }

        public static DependencyProperty PlaceHolderBrushProperty =
            DependencyProperty.Register(nameof(PlaceHolderBrush), typeof(Brush), typeof(PlaceHolderTextControl), new UIPropertyMetadata(Brushes.Gray));
    }
}
