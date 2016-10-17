using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace zopalib
{
    /// <summary>
    /// Represents a single loan from one lender
    /// </summary>
    public class Loan
    {
        public string Lender { get; private set; }
        public float Rate { get; private set; }
        public float MonthlyPayments { get; private set; }
        public int LoanAmount { get; private set; }
        public float TotalRepayment { get; private set; }
        public int LoanDuration { get; private set; }
        public string Monthly
        {
            get { return MonthlyPayments.ToString("n2"); }
            private set { }
        }
        public string Total
        {
            get { return TotalRepayment.ToString("n2"); }
            private set { }
        }

        public Loan(string lender, float rate, int amount, int duration)
        {
            Lender = lender;
            Rate = rate;
            LoanAmount = amount;
            LoanDuration = duration;
        }

        /// <summary>
        /// Calculates monthly payments based on monthly compound interest
        /// </summary>
        /// <remarks>
        /// Uses the formula P = (Lr.(1+r)^n)/((1+r)^n -1)
        /// where P is the monthly payment,
        /// L is the loan amount
        /// r is the monthly interest rate
        /// n is the number of months of the loan
        /// </remarks>
        public void CalculatePayments()
        {
            // Precompute part of the calculation, for clarity as much as performance
            // Note that it is Rate/12 as the formula calls for monthly rate, not APR
            float monthlyRate = Rate/12;
            float temp = 1 + (monthlyRate);
            temp = (float)Math.Pow(temp, (float) LoanDuration);

            MonthlyPayments = (LoanAmount*monthlyRate*temp)/(temp - 1);
            TotalRepayment = MonthlyPayments*LoanDuration;
        }
    }
}
