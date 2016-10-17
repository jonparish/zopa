using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zopalib;

namespace zopalibtests
{
    /// <summary>
    /// Summary description for LoanAggregatorTests
    /// </summary>
    [TestClass]
    public class LoanAggregatorTests
    {
        [TestMethod]
        public void TestInsufficientLoans()
        {
            MarketRate rate = new MarketRate{AmountAvailable = 100, Lender = "Bob", Rate = (float)0.07};
            List<MarketRate> rates = new List<MarketRate>();
            rates.Add(rate);

            LoanAggregator agg = new LoanAggregator();
            agg.FindLoans(rates, 1000, 36);

            Assert.AreEqual(false, agg.IsLoanPossible);
        }

        [TestMethod]
        public void TestSingleLoanPossible()
        {
            MarketRate rate = new MarketRate { AmountAvailable = 100, Lender = "Bob", Rate = (float)0.07 };
            List<MarketRate> rates = new List<MarketRate>();
            rates.Add(rate);

            LoanAggregator agg = new LoanAggregator();
            agg.FindLoans(rates, 50, 36);

            Assert.AreEqual(true, agg.IsLoanPossible);
        }

        [TestMethod]
        public void TestMultipleLoadsPossible()
        {
            MarketRate rate1 = new MarketRate { AmountAvailable = 100, Lender = "Bob", Rate = (float)0.07 };
            MarketRate rate2 = new MarketRate { AmountAvailable = 100, Lender = "Alice", Rate = (float)0.06 };
            List<MarketRate> rates = new List<MarketRate>();
            rates.Add(rate1);
            rates.Add(rate2);

            LoanAggregator agg = new LoanAggregator();
            agg.FindLoans(rates, 50, 36);

            Assert.AreEqual(true, agg.IsLoanPossible);
        }

        [TestMethod]
        public void TestLowestRateChosen()
        {
            MarketRate rate1 = new MarketRate { AmountAvailable = 100, Lender = "Bob", Rate = (float)0.07 };
            MarketRate rate2 = new MarketRate { AmountAvailable = 100, Lender = "Alice", Rate = (float)0.03 };
            List<MarketRate> rates = new List<MarketRate>();
            rates.Add(rate1);
            rates.Add(rate2);

            LoanAggregator agg = new LoanAggregator();
            agg.FindLoans(rates, 50, 36);

            Assert.AreEqual(true, agg.IsLoanPossible);
        }

        [TestMethod]
        public void TestMixedRateChosen()
        {
            MarketRate rate = new MarketRate { AmountAvailable = 100, Lender = "Bob", Rate = (float)0.07 };
            List<MarketRate> rates = new List<MarketRate>();
            rates.Add(rate);

            LoanAggregator agg = new LoanAggregator();
            agg.FindLoans(rates, 50, 36);

            Assert.AreEqual(true, agg.IsLoanPossible);
        }

        [TestMethod]
        public void TestSingleLoanCalculation()
        {
            MarketRate rate = new MarketRate { AmountAvailable = 1000, Lender = "Alice", Rate = (float)0.07 };
            List<MarketRate> rates = new List<MarketRate>();
            rates.Add(rate);

            LoanAggregator agg = new LoanAggregator();
            agg.FindLoans(rates, 1000, 36);

            Assert.AreEqual(true, agg.IsLoanPossible);

            Assert.AreEqual(1000, agg.LoanAmount, "LoanAmount check failed");
            Assert.AreEqual("30.88", agg.Monthly, "MonthlyPayments check failed");
            Assert.AreEqual("1,111.57", agg.Total, "TotalRepayment check failed");
        }

        [TestMethod]
        public void TestMultipleLoanCalculation()
        {
            MarketRate rate = new MarketRate { AmountAvailable = 1000, Lender = "Alice", Rate = (float)0.07 };
            MarketRate rate1 = new MarketRate { AmountAvailable = 1000, Lender = "Bob", Rate = (float)0.07 };
            MarketRate rate2 = new MarketRate { AmountAvailable = 1000, Lender = "Charlie", Rate = (float)0.07 };
            List<MarketRate> rates = new List<MarketRate>();
            rates.Add(rate);
            rates.Add(rate1);
            rates.Add(rate2);

            LoanAggregator agg = new LoanAggregator();
            agg.FindLoans(rates, 3000, 36);

            Assert.AreEqual(true, agg.IsLoanPossible);

            Assert.AreEqual(3000, agg.LoanAmount, "LoanAmount check failed");
            Assert.AreEqual("92.63", agg.Monthly, "MonthlyPayments check failed");
            Assert.AreEqual("3,334.70", agg.Total, "TotalRepayment check failed");

            Assert.AreEqual((float)0.07, agg.Rate, "Rate check failed");
        }
    }
}
