﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public Exception Exception { get; set; }
        public List<Exception> InnerExceptions { get; set; }

        public ErrorWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ErrorWindow(Exception exception) : this()
        {
            Exception = exception;
            if (exception.InnerException != null)
            {
                InnerExceptions = new List<Exception>();
                AppendInnerExceptions(exception, InnerExceptions);
            }
        }

        private static void AppendInnerExceptions(Exception exception, List<Exception> innerExceptions)
        {
            if (exception.InnerException != null)
            {
                innerExceptions.Add(exception.InnerException);
                AppendInnerExceptions(exception.InnerException, innerExceptions);
            }
        }

        public static void Open(Exception exception) => Open(exception, null);

        public static void Open(Exception exception, Window owner)
        {
            ErrorWindow errorWindow = new ErrorWindow(exception);
            errorWindow.Owner = owner;
            errorWindow.ShowDialog();
        }

        private void CopyCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Exception != null)
                Clipboard.SetText(Exception.ToString());
        }
    }
}
