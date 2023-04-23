using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class LoanApplicationVM : ViewModelBase
    {
        private readonly LoanApplication _loanApplication;
        private Visibility _busyVisibility = Visibility.Collapsed;

        private LoanApplicationVM(LoanApplication loanApplication)
        {
            _loanApplication = loanApplication;
        }

        public Visibility BusyVisibility 
        {
            get => _busyVisibility;
            set
            {
                if (_busyVisibility != value)
                {
                    _busyVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ApplicationStatusDescription => _loanApplication.StatusDescription;

        public DateTime? ApplicationDate 
        { 
            get => _loanApplication.ApplicationDate;
            set
            {
                if (_loanApplication.ApplicationDate.HasValue != value.HasValue 
                    || (value.HasValue && _loanApplication.ApplicationDate.Value != value.Value))
                {
                    _loanApplication.ApplicationDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? Amount
        {
            get => _loanApplication.Amount;
            set
            {
                if (_loanApplication.Amount.HasValue != value.HasValue || (value.HasValue && _loanApplication.Amount.Value != value.Value))
                {
                    _loanApplication.Amount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Purpose
        {
            get => _loanApplication.Purpose;
            set
            {
                if (_loanApplication.Purpose != value)
                {
                    _loanApplication.Purpose = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerName 
        { 
            get => _loanApplication.BorrowerName ?? string.Empty;
            set
            {
                if (_loanApplication.BorrowerName != value)
                {
                    _loanApplication.BorrowerName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime? BorrowerBirthDate 
        { 
            get => _loanApplication.BorrowerBirthDate;
            set
            {
                if (_loanApplication.BorrowerBirthDate.HasValue != value.HasValue
                    || (value.HasValue && _loanApplication.BorrowerBirthDate.Value != value.Value))
                {
                    _loanApplication.BorrowerBirthDate = value;
                    NotifyPropertyChanged();
                }
            } 
        }

        public string BorrowerEmailAddress
        {
            get => _loanApplication.BorrowerEmailAddress;
            set
            {
                if (_loanApplication.BorrowerEmailAddress != value)
                {
                    _loanApplication.BorrowerEmailAddress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerPhone
        {
            get => Regex.IsMatch(_loanApplication.BorrowerPhone, @"^[0-9]{10}$") 
                ? string.Format("{0}-{1}-{2}", _loanApplication.BorrowerPhone.Substring(0, 3), _loanApplication.BorrowerPhone.Substring(3, 3), _loanApplication.BorrowerPhone.Substring(6)) 
                : _loanApplication.BorrowerPhone;
            set
            {
                if (_loanApplication.BorrowerPhone != value)
                {
                    _loanApplication.BorrowerPhone = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerAddress
        {
            get => _loanApplication.BorrowerAddress?.Delivery ?? string.Empty;
            set
            {
                if (_loanApplication.BorrowerAddress?.Delivery != value)
                {
                    _loanApplication.BorrowerAddress.Delivery = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerCity
        {
            get => _loanApplication.BorrowerAddress?.City ?? string.Empty;
            set
            {
                if (_loanApplication.BorrowerAddress?.City != value)
                {
                    _loanApplication.BorrowerAddress.City = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerState
        {
            get => _loanApplication.BorrowerAddress?.State ?? string.Empty;
            set
            {
                if (_loanApplication.BorrowerAddress?.State != value)
                {
                    _loanApplication.BorrowerAddress.State = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerPostalCode
        {
            get => _loanApplication.BorrowerAddress?.PostalCode ?? string.Empty;
            set
            {
                if (_loanApplication.BorrowerAddress?.PostalCode != value)
                {
                    _loanApplication.BorrowerAddress.PostalCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerEmployerName
        {
            get => _loanApplication.BorrowerEmployerName;
            set
            {
                if (_loanApplication.BorrowerEmployerName != value)
                {
                    _loanApplication.BorrowerEmployerName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime? BorrowerEmploymentHireDate
        {
            get => _loanApplication.BorrowerEmploymentHireDate;
            set
            {
                if (_loanApplication.BorrowerEmploymentHireDate.HasValue != value.HasValue
                    || (value.HasValue && _loanApplication.BorrowerEmploymentHireDate.Value != value.Value))
                {
                    _loanApplication.BorrowerEmploymentHireDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? BorrowerIncome
        {
            get => _loanApplication.BorrowerIncome;
            set
            {
                if (_loanApplication.BorrowerIncome.HasValue != value.HasValue || (value.HasValue && _loanApplication.BorrowerIncome.Value != value.Value))
                {
                    _loanApplication.BorrowerIncome = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? MortgagePayment
        {
            get => _loanApplication.MortgagePayment;
            set
            {
                if (_loanApplication.MortgagePayment.HasValue != value.HasValue || (value.HasValue && _loanApplication.MortgagePayment.Value != value.Value))
                {
                    _loanApplication.MortgagePayment = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? RentPayment
        {
            get => _loanApplication.RentPayment;
            set
            {
                if (_loanApplication.RentPayment.HasValue != value.HasValue || (value.HasValue && _loanApplication.RentPayment.Value != value.Value))
                {
                    _loanApplication.RentPayment = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerName
        {
            get => _loanApplication.CoBorrowerName ?? string.Empty;
            set
            {
                if (_loanApplication.CoBorrowerName != value)
                {
                    _loanApplication.CoBorrowerName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime? CoBorrowerBirthDate
        {
            get => _loanApplication.CoBorrowerBirthDate;
            set
            {
                if (_loanApplication.CoBorrowerBirthDate.HasValue != value.HasValue
                    || (value.HasValue && _loanApplication.CoBorrowerBirthDate.Value != value.Value))
                {
                    _loanApplication.CoBorrowerBirthDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerEmailAddress
        {
            get => _loanApplication.CoBorrowerEmailAddress;
            set
            {
                if (_loanApplication.CoBorrowerEmailAddress != value)
                {
                    _loanApplication.CoBorrowerEmailAddress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerPhone
        {
            get => Regex.IsMatch(_loanApplication.CoBorrowerPhone, @"^[0-9]{10}$")
                ? string.Format("{0}-{1}-{2}", _loanApplication.CoBorrowerPhone.Substring(0, 3), _loanApplication.CoBorrowerPhone.Substring(3, 3), _loanApplication.CoBorrowerPhone.Substring(6))
                : _loanApplication.CoBorrowerPhone;
            set
            {
                if (_loanApplication.CoBorrowerPhone != value)
                {
                    _loanApplication.CoBorrowerPhone = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerAddress
        {
            get => _loanApplication.CoBorrowerAddress?.Delivery ?? string.Empty;
            set
            {
                if (_loanApplication.CoBorrowerAddress?.Delivery != value)
                {
                    _loanApplication.CoBorrowerAddress.Delivery = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerCity
        {
            get => _loanApplication.CoBorrowerAddress?.City ?? string.Empty;
            set
            {
                if (_loanApplication.CoBorrowerAddress?.City != value)
                {
                    _loanApplication.CoBorrowerAddress.City = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerState
        {
            get => _loanApplication.CoBorrowerAddress?.State ?? string.Empty;
            set
            {
                if (_loanApplication.CoBorrowerAddress?.State != value)
                {
                    _loanApplication.CoBorrowerAddress.State = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerPostalCode
        {
            get => _loanApplication.CoBorrowerAddress?.PostalCode ?? string.Empty;
            set
            {
                if (_loanApplication.CoBorrowerAddress?.PostalCode != value)
                {
                    _loanApplication.CoBorrowerAddress.PostalCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string CoBorrowerEmployerName
        {
            get => _loanApplication.CoBorrowerEmployerName;
            set
            {
                if (_loanApplication.CoBorrowerEmployerName != value)
                {
                    _loanApplication.CoBorrowerEmployerName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime? CoBorrowerEmploymentHireDate
        {
            get => _loanApplication.CoBorrowerEmploymentHireDate;
            set
            {
                if (_loanApplication.CoBorrowerEmploymentHireDate.HasValue != value.HasValue
                    || (value.HasValue && _loanApplication.CoBorrowerEmploymentHireDate.Value != value.Value))
                {
                    _loanApplication.CoBorrowerEmploymentHireDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? CoBorrowerIncome
        {
            get => _loanApplication.CoBorrowerIncome;
            set
            {
                if (_loanApplication.CoBorrowerIncome.HasValue != value.HasValue || (value.HasValue && _loanApplication.CoBorrowerIncome.Value != value.Value))
                {
                    _loanApplication.CoBorrowerIncome = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static LoanApplicationVM Create(LoanApplication loanApplication)
        {
            LoanApplicationVM vm = new LoanApplicationVM(loanApplication);
            LoanApplicationValidator validator = new LoanApplicationValidator(vm);
            vm.AddBehavior(validator);
            return vm;
        }
    }
}
