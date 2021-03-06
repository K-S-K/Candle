﻿using CandleApp.Data;
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

namespace CandleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <remarks>
    /// https://www.wpf-tutorial.com/common-interface-controls/toolbar-control/
    /// </remarks>
    public partial class MainWindow : Window
    {
        private CandleTableData _dataTT;

        public MainWindow()
        {
            _dataTT = new CandleTableData("MSFT");

            InitializeComponent();

            tblCandles.ItemsSource = _dataTT.Items;

            candleControl.SetData(_dataTT.Items);
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
