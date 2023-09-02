using CsvHelper;
using CsvHelper.Configuration;
using JestersCreditUnion.Interface.Models;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationFileWriter : ILoanApplicationFileWriter, IDisposable
    {
        private readonly IEnumerable<LoanApplication> _loanApplications;
        private readonly CsvWriter _csvWriter;
        private readonly FileStream _fileStream;
        private readonly StreamWriter _streamWriter;
        private bool _disposedValue;

        public LoanApplicationFileWriter(
            IEnumerable<LoanApplication> loanApplications,
            string fileName)
        {
            _loanApplications = loanApplications;
            _fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            _streamWriter = new StreamWriter(_fileStream, Encoding.UTF8);
            _csvWriter = new CsvWriter(_streamWriter, CultureInfo.InvariantCulture, true);
            _csvWriter.Context.RegisterClassMap<LoanApplicationMap>();
            _csvWriter.WriteHeader<LoanApplication>();
            _csvWriter.NextRecord();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _csvWriter?.Dispose();
                    _streamWriter?.Dispose();
                    _fileStream?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LoanApplicationFileWriter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<LoanApplication> GetEnumerator() => new Enumerator(_loanApplications.GetEnumerator(), _csvWriter);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        internal class Enumerator : IEnumerator<LoanApplication>
        {
            private readonly IEnumerator<LoanApplication> _itemsSource;
            private readonly CsvWriter _csvWriter;

            public Enumerator(IEnumerator<LoanApplication> itemsSource, CsvWriter csvWriter)
            {
                _itemsSource = itemsSource;
                _csvWriter = csvWriter;
            }

            public LoanApplication Current => _itemsSource.Current;

            object IEnumerator.Current => _itemsSource.Current;

            public void Dispose()
            {
                _itemsSource.Dispose();
            }

            public bool MoveNext()
            {
                bool result = _itemsSource.MoveNext();
                if (result)
                {
                    _csvWriter.WriteRecord(Current);
                    _csvWriter.NextRecord();
                }   
                return result;
            }

            public void Reset()
            { }
        }

        internal class LoanApplicationMap : ClassMap<LoanApplication>
        {
            public LoanApplicationMap() 
            {
                Map(a => a.BorrowerName);
                Map(a => a.BorrowerBirthDate).Convert(d => d.Value.BorrowerBirthDate?.ToString("yyyy-MM-dd") ?? string.Empty);
                Map(a => a.BorrowerEmailAddress);
                Map(a => a.BorrowerPhone);
                Map(a => a.BorrowerEmployerName);
                Map(a => a.BorrowerEmploymentHireDate).Convert(d => d.Value.BorrowerBirthDate?.ToString("yyyy-MM-dd") ?? string.Empty);
                Map(a => a.BorrowerIncome);
                Map(a => a.Amount);
                Map(a => a.Purpose);
                Map(a => a.MortgagePayment);
                Map(a => a.RentPayment);
            }
        }
    }
}
