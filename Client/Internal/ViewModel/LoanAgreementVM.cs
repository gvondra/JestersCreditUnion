﻿using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel
{
    public class LoanAgreementVM : ViewModelBase
    {
        private readonly LoanAgreement _innerLoanAgreement;
        private readonly ObservableCollection<KeyValuePair<short, string>> _terms = new ObservableCollection<KeyValuePair<short, string>>
        {
            new KeyValuePair<short, string>(1, "Annual"),
            new KeyValuePair<short, string>(2, "Semiannual"),
            new KeyValuePair<short, string>(12, "Monthly"),
            new KeyValuePair<short, string>(24, "Semimonthly"),
            new KeyValuePair<short, string>(26, "Fortnightly")
        };

        private LoanAgreementVM(LoanAgreement innerLoanAgreement)
        {
            _innerLoanAgreement = innerLoanAgreement;
        }

        public static LoanAgreementVM Create(LoanAgreement loanAgreement)
        {
            LoanAgreementVM vm = new LoanAgreementVM(loanAgreement);
            return vm;
        }

        public ObservableCollection<KeyValuePair<short, string>> Terms => _terms;

        public string BorrowerName
        {
            get => _innerLoanAgreement.BorrowerName ?? string.Empty;
            set
            {
                if (_innerLoanAgreement.BorrowerName != value)
                {
                    _innerLoanAgreement.BorrowerName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime? BorrowerBirthDate
        {
            get => _innerLoanAgreement.BorrowerBirthDate;
            set
            {
                if (_innerLoanAgreement.BorrowerBirthDate.HasValue != value.HasValue
                    || (value.HasValue && _innerLoanAgreement.BorrowerBirthDate.Value != value.Value))
                {
                    _innerLoanAgreement.BorrowerBirthDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerEmailAddress
        {
            get => _innerLoanAgreement.BorrowerEmailAddress ?? string.Empty;
            set
            {
                if ((_innerLoanAgreement.BorrowerEmailAddress ?? string.Empty) != (value ?? string.Empty))
                {
                    _innerLoanAgreement.BorrowerEmailAddress = value ?? string.Empty;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerPhone
        {
            get => _innerLoanAgreement.BorrowerPhone ?? string.Empty;
            set
            {
                if ((_innerLoanAgreement.BorrowerPhone ?? string.Empty) != (value ?? string.Empty))
                {
                    _innerLoanAgreement.BorrowerPhone = value ?? string.Empty;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerAddress
        {
            get => _innerLoanAgreement?.BorrowerAddress?.Delivery ?? string.Empty;
            set
            {
                if ((_innerLoanAgreement?.BorrowerAddress?.Delivery ?? string.Empty) != (value ?? string.Empty))
                {
                    if (_innerLoanAgreement.BorrowerAddress == null)
                        _innerLoanAgreement.BorrowerAddress = new Address();
                    _innerLoanAgreement.BorrowerAddress.Delivery = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerCity
        {
            get => _innerLoanAgreement?.BorrowerAddress?.City ?? string.Empty;
            set
            {
                if ((_innerLoanAgreement?.BorrowerAddress?.City ?? string.Empty) != (value ?? string.Empty))
                {
                    if (_innerLoanAgreement.BorrowerAddress == null)
                        _innerLoanAgreement.BorrowerAddress = new Address();
                    _innerLoanAgreement.BorrowerAddress.City = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerState
        {
            get => _innerLoanAgreement?.BorrowerAddress?.State ?? string.Empty;
            set
            {
                if ((_innerLoanAgreement?.BorrowerAddress?.State ?? string.Empty) != (value ?? string.Empty))
                {
                    if (_innerLoanAgreement.BorrowerAddress == null)
                        _innerLoanAgreement.BorrowerAddress = new Address();
                    _innerLoanAgreement.BorrowerAddress.State = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string BorrowerPostalCode
        {
            get => _innerLoanAgreement?.BorrowerAddress?.PostalCode ?? string.Empty;
            set
            {
                if ((_innerLoanAgreement?.BorrowerAddress?.PostalCode ?? string.Empty) != (value ?? string.Empty))
                {
                    if (_innerLoanAgreement.BorrowerAddress == null)
                        _innerLoanAgreement.BorrowerAddress = new Address();
                    _innerLoanAgreement.BorrowerAddress.PostalCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? OriginalAmount
        {
            get => _innerLoanAgreement.OriginalAmount;
            set
            {
                if ((_innerLoanAgreement.OriginalAmount ?? 0.0M) != (value ?? 0.0M))
                {
                    _innerLoanAgreement.OriginalAmount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal InterestPercentage
        {
            get => (_innerLoanAgreement.InterestRate ?? 0.01M) * 100.0M;
            set
            {
                if (!_innerLoanAgreement.InterestRate.HasValue || _innerLoanAgreement.InterestRate.Value != value)
                {
                    _innerLoanAgreement.InterestRate = value / 100.0M;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? PaymentAmount
        {
            get => _innerLoanAgreement.PaymentAmount;
            set
            {
                if (_innerLoanAgreement.PaymentAmount.HasValue != value.HasValue 
                    || (_innerLoanAgreement.PaymentAmount.HasValue && _innerLoanAgreement.PaymentAmount.Value != value.Value))
                {
                    _innerLoanAgreement.PaymentAmount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public short? OriginalTerm
        {
            get => _innerLoanAgreement.OriginalTerm;
            set
            {
                if (_innerLoanAgreement.OriginalTerm.HasValue != value.HasValue 
                    || (_innerLoanAgreement.OriginalTerm.HasValue && _innerLoanAgreement.OriginalTerm.Value != value.Value))
                {
                    _innerLoanAgreement.OriginalTerm = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public short? PaymentFrequency
        {
            get => _innerLoanAgreement.PaymentFrequency;
            set
            {
                if (_innerLoanAgreement.PaymentFrequency.HasValue != value.HasValue
                    || (_innerLoanAgreement.PaymentFrequency.HasValue && _innerLoanAgreement.PaymentFrequency.Value != value.Value))
                {
                    _innerLoanAgreement.PaymentFrequency = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
