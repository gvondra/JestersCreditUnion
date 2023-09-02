﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IPaymentSaver
    {
        Task Update(ISettings settings, IEnumerable<IPayment> payments);
        Task<IEnumerable<IPayment>> Save(ISettings settings, IEnumerable<IPayment> payments);
    }
}
