using CandleApp.Data;
using System;
using Xunit;

namespace Candle.Test
{
    public class CandleFileLineTest
    {
        [Fact]
        public void Parse()
        {
            string input = "06/09/2020, $181.75, 1245538, $180.95, $183.47, $179.36";

            CandleTableItem line = new CandleTableItem(input);
        }
    }
}
