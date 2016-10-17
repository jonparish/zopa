using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zopalib;


namespace zopa
{
    class Zopa
    {
        static void Main(string[] args)
        {
            // Parse command line
            string marketFile = "";
            int loanAmount = 0;
            const int loanDuration = 36; // Not parameterised for the purposes of this test

            if (!parseCommandLine(args, ref marketFile, ref loanAmount))
            {
                Console.WriteLine("Usage: zopa <location of market rate file> <requested loan amount>");
                return;
            }

            Console.WriteLine("Processing market file " + marketFile + " for a loan of " + loanAmount.ToString());

            // Process market file
            MarketRateCsvParser parser = new MarketRateCsvParser();
            List<MarketRate> marketRates;
            StreamReader marketStream = new System.IO.StreamReader(marketFile);
            marketRates = parser.Parse(marketStream);

            // Pass this to the loan aggregator to compute the loans
            LoanAggregator loanAgg = new LoanAggregator();
            loanAgg.FindLoans(marketRates, loanAmount, loanDuration);

            // Finally, output the results
            Console.WriteLine("Requested Amount: " + loanAgg.LoanAmount.ToString("c2"));
            Console.WriteLine("Rate: " + (loanAgg.Rate*100).ToString("n1") + "%");
            Console.WriteLine("Monthly Repayment: " + loanAgg.MonthlyPayments.ToString("c2"));
            Console.WriteLine("Total Replayment: " + loanAgg.TotalRepayment.ToString("c2"));
        }

        // Simple command line parser. In reality, this would be more sophisticated and in its own
        // class or library, but this will suffice for this coding test
        static bool parseCommandLine(string[] args, ref string marketFile, ref int loanAmount)
        {
            if (args.Length == 2)
            {
                marketFile = args[0];
                if (int.TryParse(args[1], out loanAmount))
                {
                    return true;
                }
            }

            // Here means we couldnt parse command line
            return false;
        }
    }
}
