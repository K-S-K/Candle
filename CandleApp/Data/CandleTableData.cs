using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CandleApp.Data
{
    public class CandleTableData
    {
        #region -> Data
        private SortedDictionary<DateTime, CandleTableItem> _data;
        #endregion


        #region -> Properties
        public string Ticker { get; private set; }
        public IEnumerable<CandleTableLine> Items => _data
            .Values.OrderBy(i => i.Date).Reverse()
            .Select(i => new CandleTableLine(i));
        #endregion


        #region -> Methods
        public void Load(string fileName)
        {

        }
        #endregion


        #region -> Implementation
        private void Add(CandleTableItem item)
        {
            _data[item.Date] = item;
        }
        #endregion


        #region -> Ctor
        public CandleTableData(string ticker)
        {
            Ticker = ticker;

            _data = new SortedDictionary<DateTime, CandleTableItem>();

            Add(new CandleTableItem("06/09/2020, $181.75, 1245538, $180.95, $183.47, $179.36"));
            Add(new CandleTableItem("06/08/2020, $183.9, 1353562, $178.9, $184.94, $178.9"));
            Add(new CandleTableItem("06/05/2020, $180.01, 1795509, $179.47, $181.51, $176.84"));
            Add(new CandleTableItem("06/04/2020, $174.81, 906520, $172.66, $175.345, $172.66"));
            Add(new CandleTableItem("06/03/2020, $174.96, 1557877, $173.77, $176.81, $173.02"));
            Add(new CandleTableItem("06/02/2020, $171.47, 1156300, $170.19, $171.94, $169.29"));
            Add(new CandleTableItem("06/01/2020, $169.83, 1283967, $169.49, $173.32, $168.6972"));
            Add(new CandleTableItem("05/29/2020, $169.6, 2755247, $168.21, $171.0194, $167.49"));
            Add(new CandleTableItem("05/28/2020, $169.79, 1667449, $173.02, $173.25, $169.15"));
            Add(new CandleTableItem("05/27/2020, $171.63, 1659188, $170.71, $172.75, $170.05"));
            Add(new CandleTableItem("05/26/2020, $167.11, 2048356, $159.49, $168.39, $158.92"));
            Add(new CandleTableItem("05/22/2020, $155.04, 1209161, $157.58, $157.63, $154.67"));
            Add(new CandleTableItem("05/21/2020, $157.55, 950917, $159.85, $160.545, $157.18"));
            Add(new CandleTableItem("05/20/2020, $161.84, 1069300, $159.64, $163.5, $158.75"));
            Add(new CandleTableItem("05/19/2020, $157.79, 1277607, $161.25, $162.39, $157.64"));
            Add(new CandleTableItem("05/18/2020, $162.03, 1761395, $160.93, $164.8, $160.68"));
            Add(new CandleTableItem("05/15/2020, $155.64, 2119560, $154.38, $156.92, $153.31"));
            Add(new CandleTableItem("05/14/2020, $155.51, 1857293, $145.32, $155.82, $143.32"));
            Add(new CandleTableItem("05/13/2020, $147.68, 1387787, $152.56, $153.27, $146.88"));
            Add(new CandleTableItem("05/12/2020, $153.61, 1559240, $159.1, $160.995, $153.5307"));
            Add(new CandleTableItem("05/11/2020, $161.81, 1123897, $158.72, $162.36, $158.02"));
        }
        #endregion
    }
}
