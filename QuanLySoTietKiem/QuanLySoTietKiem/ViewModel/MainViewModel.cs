﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLySoTietKiem.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private String[] _backgroundColor = { "#5EB2FF", "", "", "", "" };
        public String[] BackgroundColor { get => _backgroundColor; set { _backgroundColor = value; OnPropertyChanged(); } }
        private String[] _textIconColor = { "White", "#A5A5B3", "#A5A5B3", "#A5A5B3", "#A5A5B3"};
        public String[] TextIconColor { get => _textIconColor; set { _textIconColor = value; OnPropertyChanged(); } }
        public string userInside;
        public ICommand HomeCommand { get; set; }
        public ICommand SavingAccountCommand { get; set; }
        public ICommand DepositCommand { get; set; }
        public ICommand WithdrawCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        private object _currentView = new HomeView();
        public object CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }        
        public MainViewModel()
        {
            LoginWindow loginWD = new LoginWindow();
            loginWD.ShowDialog();
            userInside = loginWD._username;
            MessageBox.Show("User vừa đăng nhập: " + userInside);
            HomeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                BackgroundColor = new String[] { "#5EB2FF", "",  "", "", "", };
                TextIconColor = new String[] { "White", "#A5A5B3", "#A5A5B3", "#A5A5B3", "#A5A5B3" };
                CurrentView = new HomeView();
            });
            SavingAccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = new SavingAccountView();
                BackgroundColor = new String[] { "","#5EB2FF", "", "", "",};
                TextIconColor = new String[] { "#A5A5B3", "White", "#A5A5B3", "#A5A5B3", "#A5A5B3" };
            });
            DepositCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = new DepositView();
                BackgroundColor = new String[] { "", "", "#5EB2FF", "", "", };
                TextIconColor = new String[] { "#A5A5B3", "#A5A5B3", "White", "#A5A5B3", "#A5A5B3" };
            });
            WithdrawCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = new WithdrawView();
                BackgroundColor = new String[] { "",  "", "", "#5EB2FF", "", };
                TextIconColor = new String[] { "#A5A5B3", "#A5A5B3", "#A5A5B3", "White", "#A5A5B3" };
            });
            ReportCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = new ReportView();
                BackgroundColor = new String[] { "", "", "", "", "#5EB2FF", };
                TextIconColor = new String[] { "#A5A5B3", "#A5A5B3", "#A5A5B3", "#A5A5B3", "White", };
            });
            LogoutCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                var result = MessageBox.Show("Bạn có muốn đăng xuất không?", "Đăng xuất", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes) p.Close();
            });
        }
    }
}
