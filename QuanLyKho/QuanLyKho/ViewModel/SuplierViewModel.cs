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
    public class SuplierViewModel : BaseViewModel
    {
        private ObservableCollection<Suplier> _List;
        public ObservableCollection<Suplier> List
        {
            get => _List;
            set
            {
                _List = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Phone = SelectedItem.Phone;
                    Email = SelectedItem.Email;
                    Address = SelectedItem.Address;
                    MoreInfor = SelectedItem.MoreInfo;
                    ContractDate = SelectedItem.ContractDate;
                }
            }
        }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _MoreInfor;
        public string MoreInfor { get => _MoreInfor; set { _MoreInfor = value; OnPropertyChanged(); } }
        
        private DateTime? _ContractDate;
        public DateTime? ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }

        private Suplier _SelectedItem;
        public Suplier SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public SuplierViewModel()
        {
            List = new ObservableCollection<Suplier>(DataProvider.Ins.DB.Supliers);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                var check = DataProvider.Ins.DB.Supliers.Where(x => x.DisplayName == DisplayName);
                if (check == null
                || check.Count() == 0)
                    return true;

                return false;
            },
            (p) =>
            {
                var newUnit = new Suplier()
                {
                    DisplayName = DisplayName,
                };

                //Save to view
                List.Add(newUnit);

                //Save to DB
                DataProvider.Ins.DB.Supliers.Add(newUnit);
                DataProvider.Ins.DB.SaveChanges();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName)
                || SelectedItem == null)
                    return false;

                var check = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == DisplayName);
                if (check == null
                || check.Count() == 0)
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
