using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace CandleApp.Data
{
    public class CandleTableItem
    {
        #region -> Properties
        // Date, Close/Last, Volume, Open, High, Low
        // 06/09/2020, $181.75, 1245538, $180.95, $183.47, $179.36
        // MM/dd/yyyy

        public DateTime Date { get; private set; }
        public decimal Open { get; private set; }
        public decimal Close { get; private set; }

        public long Volume { get; private set; }

        public decimal High { get; private set; }
        public decimal Low { get; private set; }

        public Direction Dir => Open < Close ? Direction.Up : Close < Open ? Direction.Dn : Direction.No;
        #endregion


        #region -> Methods
        public override string ToString() => $"[{Date.ToString("yyyy.MM.dd")}]: {High}->{Low} = {Volume}";
        #endregion


        #region -> Ctor
        public CandleTableItem(string input)
        {
            string[] inputs = input.Split(',');

            NumberFormatInfo MyNFI = new NumberFormatInfo();
            MyNFI.NegativeSign = "-";
            MyNFI.NumberDecimalSeparator = ".";
            MyNFI.NumberGroupSeparator = ",";
            MyNFI.CurrencySymbol = "$";

            Date = DateTime.ParseExact(inputs[0], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            Close = decimal.Parse(inputs[1], NumberStyles.Currency, MyNFI);
            Volume = long.Parse(inputs[2]);
            Open = decimal.Parse(inputs[3], NumberStyles.Currency, MyNFI);
            High = decimal.Parse(inputs[4], NumberStyles.Currency, MyNFI);
            Low = decimal.Parse(inputs[5], NumberStyles.Currency, MyNFI);
        }
        #endregion
    }
}
