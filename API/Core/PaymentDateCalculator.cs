using JestersCreditUnion.Framework.Enumerations;
using System;

namespace JestersCreditUnion.Core
{
    public static class PaymentDateCalculator
    {
        // dueAfter is a date from which the payment date is calculated

        private delegate DateTime NextPaymentDateDelegate(DateTime dueAfter);

        public static DateTime FirstPaymentDate(LoanPaymentFrequency frequency) => FirstPaymentDate(DateTime.Today, frequency);

        public static DateTime FirstPaymentDate(DateTime dueAfter, LoanPaymentFrequency frequency) => NextPaymentDate(dueAfter.AddDays(21), frequency);

        public static DateTime NextPaymentDate(DateTime dueAfter, LoanPaymentFrequency frequency)
        {
            NextPaymentDateDelegate nextPaymentDate;
            switch (frequency)
            {
                case LoanPaymentFrequency.Annual:
                    nextPaymentDate = NextAnnualPaymentDate;
                    break;
                case LoanPaymentFrequency.Semiannual:
                    nextPaymentDate = NextSemiAnnualPaymentDate;
                    break;
                case LoanPaymentFrequency.Semimonthly:
                    nextPaymentDate = NextSemiMonthlyPaymentDate;
                    break;
                case LoanPaymentFrequency.Fortnightly:
                    nextPaymentDate = NextFortnightlyPaymentDate;
                    break;
                default:
                    nextPaymentDate = NextMonthlyPaymentDate;
                    break;
            }
            return nextPaymentDate.Invoke(dueAfter);
        }

        private static DateTime NextAnnualPaymentDate(DateTime dueAfter)
        {
            DateTime paymentDate = new DateTime(dueAfter.Year, dueAfter.Month, 1);
            return paymentDate.AddYears(1);
        }

        private static DateTime NextSemiAnnualPaymentDate(DateTime dueAfter)
        {
            DateTime paymentDate = new DateTime(dueAfter.Year, dueAfter.Month, 1);
            return paymentDate.AddMonths(6);
        }

        private static DateTime NextMonthlyPaymentDate(DateTime dueAfter)
        {
            DateTime paymentDate = new DateTime(dueAfter.Year, dueAfter.Month, 1);
            return paymentDate.AddMonths(1);
        }

        private static DateTime NextSemiMonthlyPaymentDate(DateTime dueAfter)
        {
            DateTime paymentDate;
            if (dueAfter.Day < 15)
            {
                paymentDate = new DateTime(dueAfter.Year, dueAfter.Month, 15);
            }
            else
            {
                paymentDate = new DateTime(dueAfter.Year, dueAfter.Month, 1);
                paymentDate = paymentDate.AddMonths(1);
            }
            return paymentDate;
        }

        private static DateTime NextFortnightlyPaymentDate(DateTime dueAfter)
        {
            DateTime paymentDate = dueAfter;
            while (paymentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                paymentDate = paymentDate.AddDays(-1);
            }
            return paymentDate.AddDays(14);
        }
    }
}
