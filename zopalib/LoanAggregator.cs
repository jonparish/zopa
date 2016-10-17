using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zopalib
{
    /// <summary>
    /// Combines individual loans into an overall loan for the customer
    /// </summary>
    public class LoanAggregator
    {
        private List<Loan> loans;
        public bool IsLoanPossible { get; private set; }
        public float MonthlyPayments { get; private set; }
        public float Rate { get; private set; }
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


        public LoanAggregator()
        {
            IsLoanPossible = false;
            MonthlyPayments = 0;
            Rate = 0;
            LoanAmount = 0;
            TotalRepayment = 0;
            LoanDuration = 0;
            loans = new List<Loan>();
        }

        /// <summary>
        /// Finds the lowest rate loans for the requested amount
        /// </summary>
        /// <param name="marketRates">List of MarketRates detailing the loan offers</param>
        /// <param name="loanAmount">How much should be loaned</param>
        /// <param name="loanDuration">How long will the loan be for</param>
        public void FindLoans(List<MarketRate> marketRates, int loanAmount, int loanDuration)
        {
            // Double check there's enough available to cover this loan
            if (marketRates.Sum(s => s.AmountAvailable) < loanAmount)
            {
                IsLoanPossible = false;
                return;
            }

            LoanAmount = loanAmount;
            LoanDuration = loanDuration;

            // If here then offering a loan is possible
            var rates =
                marketRates.OrderBy(o => o.Rate)
                    .Select(rate => new MarketRate {Rate = rate.Rate, AmountAvailable = rate.AmountAvailable, Lender = rate.Lender});

            foreach (var rate in rates)
            {
                int amount = (rate.AmountAvailable > loanAmount) ? rate.AmountAvailable : loanAmount;
                Loan loan = new Loan(rate.Lender, rate.Rate, amount, loanDuration);
                loans.Add(loan);

                loanAmount -= amount;
                if (loanAmount == 0)
                {
                    break;
                }
            }

            IsLoanPossible = true;
            CalculateRepayments();
        }

        /// <summary>
        /// Once the loans are located, calculate repayments from each, and the total repayments, and overall API
        /// </summary>
        private void CalculateRepayments()
        {
            MonthlyPayments = 0;
            TotalRepayment = 0;
            foreach (Loan loan in loans)
            {
                loan.CalculatePayments();
                MonthlyPayments += loan.MonthlyPayments;
                TotalRepayment += loan.TotalRepayment;
            }

            // Compute the APR as a weighted average of the component APRs
            // PLEASE NOTE: this is an estimate of the rate, there appear to be multiple definitions of APR
            // and I am unsure which are 'correct' for the UK market.
            float apr = 0;
            foreach (Loan loan in loans)
            {
                float a = loan.Rate;
                a *= (loan.LoanAmount/LoanAmount);
                apr += a;
            }

            Rate = apr;
        }
    }
}
