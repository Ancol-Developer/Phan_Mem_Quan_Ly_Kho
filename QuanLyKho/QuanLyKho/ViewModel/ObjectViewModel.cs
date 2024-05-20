using QuanLyKho.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class ObjectViewModel : BaseViewModel
    {
        private ObservableCollection<Model.Object> _List;
        public ObservableCollection<Model.Object> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Model.Unit> _Unit;
        public ObservableCollection<Model.Unit> Unit { get => _Unit; set { _Unit = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _Suplier;
        public ObservableCollection<Suplier> Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _QRCode;
        public string QRCode { get => _QRCode; set { _QRCode = value; OnPropertyChanged(); } }

        private string _BarCode;
        public string BarCode { get => _BarCode; set { _BarCode = value; OnPropertyChanged(); } }

        private Model.Object _SelectedItem;
        public Model.Object SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    QRCode = SelectedItem.QRCode;
                    BarCode = SelectedItem.BarCode;
                    SelectedUnit = SelectedItem.Unit;
                    SelectedSuplier = SelectedItem.Suplier;
                }
            }
        }

        private Model.Unit _SelectedUnit;
        public Model.Unit SelectedUnit
        {
            get => _SelectedUnit;
            set
            {
                _SelectedUnit = value;
                OnPropertyChanged();
            }
        }

        private Model.Suplier _SelectedSuplier;
        public Model.Suplier SelectedSuplier
        {
            get => _SelectedSuplier;
            set
            {
                _SelectedSuplier = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ObjectViewModel()
        {
            List = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            Unit = new ObservableCollection<Model.Unit>(DataProvider.Ins.DB.Units);
            Suplier = new ObservableCollection<Suplier>(DataProvider.Ins.DB.Supliers);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName)
                || SelectedSuplier is null
                || SelectedUnit is null)
                    return false;

                return true;
            },
            (p) =>
            {
                var newObject = new Model.Object()
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = DisplayName,
                    QRCode = QRCode,
                    BarCode = BarCode,
                    IdSuplier = SelectedSuplier.Id,
                    IdUnit = SelectedUnit.Id,
                };

                //Save to view
                List.Add(newObject);

                //Save to DB
                DataProvider.Ins.DB.Objects.Add(newObject);
                DataProvider.Ins.DB.SaveChanges();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName)
                || SelectedItem == null
                || SelectedSuplier is null
                || SelectedUnit is null)
                    return false;

                var check = DataProvider.Ins.DB.Objects.Where(x => x.Id == SelectedItem.Id);
                if (check == null
                || check.Count() == 0)
                    return false;

                return true;
            },
            (p) =>
            {
                var objects = DataProvider.Ins.DB.Objects.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                objects.DisplayName = DisplayName;
                objects.QRCode = QRCode;
                objects.BarCode = BarCode;
                objects.IdSuplier = SelectedSuplier.Id;
                objects.IdUnit = SelectedUnit.Id;
                //Save to DB
                DataProvider.Ins.DB.SaveChanges();

                // Update to SelectedItem
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.QRCode = QRCode;
                SelectedItem.BarCode = BarCode;
                SelectedItem.IdSuplier = SelectedSuplier.Id;
                SelectedItem.IdUnit = SelectedUnit.Id;
            });
        }
    }
}
