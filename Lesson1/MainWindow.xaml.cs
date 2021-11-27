﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Lesson1
{
    public partial class MainWindow
    {
        private readonly IList<long> _numbers = new List<long>();
        private const int _kof = 1000;
        private int _milliseconds;
        private Thread? _thread;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            Numbers.Text = "";
            _thread = new Thread(() => EnumerableFibbonaci());
            _thread.Start();
        }

        private void Button_Click_Stop(object sender, RoutedEventArgs e) => _thread?.Interrupt();

        private void EnumerableFibbonaci()
        {
            try
            {
                foreach (var item in GetNumberFibbonaci())
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, () =>
                    {
                        Thread.Sleep(_milliseconds);
                        Numbers.Text += $" {item}";
                    });
                }
            }
            catch (ThreadInterruptedException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private IEnumerable<string> GetNumberFibbonaci()
        {
            for (int i = 0; ; i++)
            {
                if (i is 0 or 1)
                {
                    _numbers.Add(i);
                    yield return i.ToString();
                }
                else
                {
                    _numbers.Add(_numbers[i - 2] + _numbers[i - 1]);
                    yield return (_numbers[i - 2] + _numbers[i - 1]).ToString();
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
          =>  new Thread(() =>
              {
                  Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, () =>
                  {
                      _milliseconds = (int)(Slider.Value * _kof);
                      Interval.Text = $"{Slider.Value.ToString("0.0")} с";
                  });
              }) { Name = "Slider" }.Start();
    }
}
