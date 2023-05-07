﻿using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;

namespace JestersCreditUnion.Framework.Constants
{
    public static class WorkTaskConfigurationFields
    {
        public const string NewLoanApplicationTaskTypeCode = "NewLoanApplicationTaskTypeCode";
        public const string SendDenialCorrespondenceTaskTypeCode = "SendDenialCorrespondenceTaskTypeCode";
    }

    public static class EnumerationDesriptionLookup
    {
        public static readonly List<(string, Type)> Map = new List<(string, Type)>
        {
            ("loan-application-status", typeof(LoanApplicationStatus)),
            ("loan-app-denial-reason", typeof(LoanApplicationDenialReason))
        };
    }
}
