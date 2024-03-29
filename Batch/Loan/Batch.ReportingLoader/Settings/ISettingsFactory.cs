﻿namespace JestersCreditUnion.Batch.ReportingLoader
{
    public interface ISettingsFactory
    {
        DataSettings CreateSourceData();
        DataSettings CreateDestinationData();
        LoanApiSettings CreateLoanApiSettings();
        AuthorizationSettings CreateAuthorizationSettings();
        WorkTaskSettings CreateWorkTaskSettings();
    }
}
