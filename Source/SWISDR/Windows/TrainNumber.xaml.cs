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
using System.Windows.Shapes;

namespace SWISDR.Windows
{
    /// <summary>
    /// Interaction logic for TrainNumber.xaml
    /// </summary>
    public partial class TrainNumber : Window
    {
        public int Number { get; private set; }
        public bool Result { get; private set; }
        public TrainNumber()
        {
            InitializeComponent();
        }

        private void NumberTxtBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetResult();
            else if (e.Key == Key.Escape)
                SetCancelled();
        }

        private void SetCancelled()
        {
            Number = 0;
            Result = false;
            Close();
        }

        private void SetResult()
        {
            
            if (!int.TryParse(NumberTxtBox.Text, out var number))
                return;
            Number = number;
            Result = true;
            Close();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            SetResult();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            SetCancelled();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            NumberTxtBox.Focus();
        }
    }
}
