using QuanLyKho.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class UnitViewModel : BaseViewModel
    {
        private ObservableCollection<Unit> _List;
        public ObservableCollection<Unit> List { get => _List; set { _List = value; OnPropertyChanged();} }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private Unit _SelectedItem;
        public Unit SelectedItem { get => _SelectedItem; 
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                }
            } 
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public UnitViewModel()
        {
            List = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);

            AddCommand = new RelayCommand<object>((p) => 
            {
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                var check = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == DisplayName);
                if (check == null
                || check.Count() == 0)
                    return true;

                return false;
            }, 
            (p) =>
            {
                var newUnit = new Unit()
                {
                    DisplayName = DisplayName,
                };

                //Save to view
                List.Add(newUnit);

                //Save to DB
                DataProvider.Ins.DB.Units.Add(newUnit);
                DataProvider.Ins.DB.SaveChanges();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName)
                || SelectedItem == null)
                    return false;

                var check = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == DisplayName);
                if (check == null
                || check.Count() != 0)
                    return false;

                return true;
            },
            (p) =>
            {
                var unit = DataProvider.Ins.DB.Units.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                unit.DisplayName = DisplayName;
                //Save to DB
                DataProvider.Ins.DB.SaveChanges();

                // Update to SelectedItem
                SelectedItem.DisplayName = DisplayName;
            });
        }
    }
}
