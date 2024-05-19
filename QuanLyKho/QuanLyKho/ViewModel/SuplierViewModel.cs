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
        public ObservableCollection<Suplier> List { get => _List; set { _List = value; OnPropertyChanged(); }
        }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _MoreInfo;
        public string MoreInfo { get => _MoreInfo; set { _MoreInfo = value; OnPropertyChanged(); } }
        
        private DateTime? _ContractDate;
        public DateTime? ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }

        private Suplier _SelectedItem;
        public Suplier SelectedItem { get => _SelectedItem; 
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Phone = SelectedItem.Phone;
                    Email = SelectedItem.Email;
                    Address = SelectedItem.Address;
                    MoreInfo = SelectedItem.MoreInfo;
                    ContractDate = SelectedItem.ContractDate;
                }
            } 
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public SuplierViewModel()
        {
            List = new ObservableCollection<Suplier>(DataProvider.Ins.DB.Supliers);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                return true;
            },
            (p) =>
            {
                var newSuplier = new Suplier()
                {
                    DisplayName = DisplayName,
                    Phone = Phone,
                    Address = Address,
                    Email = Email,
                    MoreInfo = MoreInfo,
                    ContractDate = ContractDate,
                };

                //Save to view
                List.Add(newSuplier);

                //Save to DB
                DataProvider.Ins.DB.Supliers.Add(newSuplier);
                DataProvider.Ins.DB.SaveChanges();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName)
                || SelectedItem == null)
                    return false;

                var check = DataProvider.Ins.DB.Units.Where(x => x.Id == SelectedItem.Id);
                if (check == null
                || check.Count() == 0)
                    return false;

                return true;
            },
            (p) =>
            {
                var suplier = DataProvider.Ins.DB.Supliers.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                suplier.DisplayName = DisplayName;
                suplier.Phone = Phone;
                suplier.Address = Address;
                suplier.Email = Email;
                suplier.MoreInfo = MoreInfo;
                suplier.ContractDate = ContractDate;
                //Save to DB
                DataProvider.Ins.DB.SaveChanges();

                // Update to SelectedItem
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.Phone = Phone;
                SelectedItem.Address = Address;
                SelectedItem.Email = Email;
                SelectedItem.MoreInfo = MoreInfo;
                SelectedItem.ContractDate = ContractDate;
            });
        }
    }
}
