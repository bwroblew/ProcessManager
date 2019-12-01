using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcessManager
{
    public partial class LabelKeyValueBox : UserControl
    {
        public static readonly DependencyProperty LabelKeyProperty = DependencyProperty
            .Register("Key",
                    typeof(string),
                    typeof(LabelKeyValueBox),
                    new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty LabelValueProperty = DependencyProperty
            .Register("Value",
                    typeof(string),
                    typeof(LabelKeyValueBox),
                    new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabelKeyValueBox()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        public string Key
        {
            get { return (string)GetValue(LabelKeyProperty); }
            set { SetValue(LabelKeyProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(LabelValueProperty); }
            set { SetValue(LabelValueProperty, value); }
        }
    }
}