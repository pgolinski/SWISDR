using System;
using System.Collections.Generic;
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

namespace SWISDR.Controls
{
    public partial class TimeTextBox : TextBox
    {
        public TimeTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text.Contains(":") || Text.Length <= 2)
            {
                e.Handled = false;
                return;
            }
            
            var caretIndex = CaretIndex;
            Text = Text.Insert(2, ":");
            CaretIndex = caretIndex + 1;
            e.Handled = true;
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectAll();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                SelectAll();
                e.Handled = true;
            }
            else
                e.Handled = false;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Text.Contains(":") || Text.Length <= 2)
                return;

            Text = Text.Insert(2, ":");
            e.Handled = true;
        }
    }
}
