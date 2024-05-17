using MaterialDesignThemes.Wpf;
using QuanLyKho.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<TonKho> _TonKhoList;
        public ObservableCollection<TonKho> TonKhoList { get {  return _TonKhoList; } set { _TonKhoList = value; OnPropertyChanged(); } }
        public bool IsLoaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand SuplierCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ObjectCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand InputCommand { get; set; }
        public ICommand OutputCommand { get; set; }
        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                if(p is null)
                    return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();

                if (loginWindow.DataContext is null)
                    return;

                var loginVM = loginWindow.DataContext as LoginViewModel;

                if (loginVM.IsLogin)
                {
                    p.Show();
                    LoadTonKhoData();
                }
                else
                {
                    p.Close();
                }

            });

            UnitCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UnitWindow wd = new UnitWindow();
                wd.ShowDialog();
            });

            SuplierCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SuplierWindow wd = new SuplierWindow();
                wd.ShowDialog();
            });

            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CustomerWindow wd = new CustomerWindow();
                wd.ShowDialog();
            });

            ObjectCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ObjectWindow wd = new ObjectWindow();
                wd.ShowDialog();
            });

            UserCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                UserWindow wd = new UserWindow();
                wd.ShowDialog();
            });

            InputCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                InputWindow wd = new InputWindow();
                wd.ShowDialog();
            });

            OutputCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OutputWindow wd = new OutputWindow();
                wd.ShowDialog();
            });
        }

        void LoadTonKhoData()
        {
            TonKhoList = new ObservableCollection<TonKho>();
            var objectList = DataProvider.Ins.DB.Objects.ToList();
            int i = 1;
            foreach(var item in objectList)
            {
                var inputList = DataProvider.Ins.DB.InputInfoes.Where(p => p.IdObject == item.Id).ToList();
                var outputList = DataProvider.Ins.DB.OutPutInfoes.Where(p => p.IdObject == item.Id).ToList();

                int sumInput = 0;
                int sumOutput = 0;

                if (inputList != null)
                    sumInput = (int)inputList.Sum(x => x.Count);
                if (outputList != null)
                    sumOutput = (int)outputList.Sum(x => x.Count);

                TonKho tonKho = new TonKho();
                tonKho.STT = i++;
                tonKho.Count = sumInput - sumOutput;
                tonKho.Object = item;

                TonKhoList.Add(tonKho);
            }
        }
    }
}
