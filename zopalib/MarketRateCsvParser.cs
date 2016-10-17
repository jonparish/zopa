using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace zopalib
{
    /// <summary>
    /// Parses a market rate CSV file
    /// </summary>
    public class MarketRateCsvParser
    {
        public enum MarketDataFields { Lender, Rate, Available };

        /// <summary>
        /// Parses a given market rate csv stream
        /// </summary>
        /// <param name="marketStream">The stream to parse</param>
        /// <returns>List of MarketRate objects</returns>
        public List<MarketRate> Parse(StreamReader marketStream)
        {
            List<MarketRate> rates = new List<MarketRate>();
            Dictionary<MarketDataFields, int> fieldMapping;

            string header = marketStream.ReadLine();
            fieldMapping = ParseHeader(header);

            string line;
            while ((line = marketStream.ReadLine()) != null)
            {
                string[] fields = line.Split(',');
                MarketRate marketRate = new MarketRate();
                marketRate.Lender = fields[fieldMapping[MarketDataFields.Lender]];
                marketRate.Rate = float.Parse(fields[fieldMapping[MarketDataFields.Rate]]);
                marketRate.AmountAvailable = int.Parse(fields[fieldMapping[MarketDataFields.Available]]);

                rates.Add(marketRate);
            }

            return rates;
        }

        /// <summary>
        /// Parses the header line of a csv to comprehend the columns
        /// </summary>
        /// <param name="header">Header line of a csv stream</param>
        /// <returns>Dictionary linking the expected fields with the column in the csv stream</returns>
        public Dictionary<MarketDataFields, int> ParseHeader(string header)
        {
            Dictionary<MarketDataFields, int> fieldMapping = new Dictionary<MarketDataFields, int>();
            string[] fields = header.Split(',');

            fieldMapping[MarketDataFields.Lender] = Array.IndexOf(fields, "Lender");
            fieldMapping[MarketDataFields.Rate] = Array.IndexOf(fields, "Rate");
            fieldMapping[MarketDataFields.Available] = Array.IndexOf(fields, "Available");

            if (fieldMapping.ContainsValue(-1))
            {
                throw new ArgumentException();
            }

            return fieldMapping;
        }
    }
}
