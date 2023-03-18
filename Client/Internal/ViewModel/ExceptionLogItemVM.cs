using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = JestersCreditUnion.Interface.Models;

namespace JCU.Internal.ViewModel
{
    public class ExceptionLogItemVM : ViewModelBase
    {
        private readonly Models.Exception _innerException;

        public ExceptionLogItemVM(Models.Exception innerException)
        {
            _innerException = innerException;
        }

        public DateTime? CreateTimestamp => _innerException.CreateTimestamp?.ToLocalTime();
        public string Message => _innerException.Message;
        public string TypeName => _innerException.TypeName;
        public string Source => _innerException.Source;
        public string AppDomain => _innerException.AppDomain;
        public string TargetSite => _innerException.TargetSite;
        public string StackTrace => _innerException.StackTrace;
    }
}
