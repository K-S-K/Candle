using System;
using System.Text;
using System.Collections.Generic;

namespace CandleApp.Data
{
    public class CandleTableLine
    {
        #region -> Data
        private CandleTableItem _data;
        #endregion


        #region -> Properties
        // Date, Close/Last, Volume, Open, High, Low
        // 06/09/2020, $181.75, 1245538, $180.95, $183.47, $179.36

        public string Date => _data.Date.ToString("yyyy.MM.dd");
        public string Open => _data.Open.ToString();
        public string Close => _data.Close.ToString();
        public string Volume => _data.Volume.ToString();

        public string High => _data.High.ToString();
        public string Low => _data.Low.ToString();

        public Direction Dir => _data.Dir;
        #endregion


        #region -> Methods
        public override string ToString() => $"[{Date}]: {High}->{Low} = {Volume}";
        public CandleTableItem GetData() => _data;
        #endregion


        #region -> Ctor
        public CandleTableLine(CandleTableItem data)
        {
            _data = data;
        }
        #endregion
    }
}
