using log4net;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EasyProject.Util
{
    public static class PasswordHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));


        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        public static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            log.Info("SetPassword(DependencyObject, string) invoked.");

            try
            {
                dp.SetValue(PasswordProperty, value);
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch
        }//SetPassword

        private static bool GetIsUpdating(DependencyObject dp)
        {
            log.Info("GetIsUpdating(DependencyObject) invoked.");

            try
            {
                return (bool)dp.GetValue(IsUpdatingProperty);
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return (bool)dp.GetValue(IsUpdatingProperty);
            }//catch
        }//GetIsUpdating

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            log.Info("SetIsUpdating(DependencyObject, bool) invoked.");

            try
            {
                dp.SetValue(IsUpdatingProperty, value);
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//SetIsUpdating

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            log.Info("OnPasswordPropertyChanged(DependencyObject, DependencyPropertyChangedEventArgs) invoked.");

            try
            {
                PasswordBox passwordBox = sender as PasswordBox;
                passwordBox.PasswordChanged -= PasswordChanged;

                if (!(bool)GetIsUpdating(passwordBox))
                {
                    passwordBox.Password = (string)e.NewValue;
                }
                passwordBox.PasswordChanged += PasswordChanged;
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//OnPasswordPropertyChanged

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            log.Info("Attach(DependencyObject, DependencyPropertyChangedEventArgs) invoked.");

            try
            {
                PasswordBox passwordBox = sender as PasswordBox;

                if (passwordBox == null)
                    return;

                if ((bool)e.OldValue)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }

                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//Attach

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            log.Info("PasswordChanged(object, RoutedEventArgs) invoked.");

            try
            {
                PasswordBox passwordBox = sender as PasswordBox;
                SetIsUpdating(passwordBox, true);
                SetPassword(passwordBox, passwordBox.Password);
                SetIsUpdating(passwordBox, false);
            }//try
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }//catch

        }//PasswordChanged
    }//class
}//namespace