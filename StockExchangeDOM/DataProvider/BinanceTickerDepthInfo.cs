using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeDOM.DataProvider
{
    public class BinanceTickerDepthInfo
    {
        [JsonProperty("lastUpdateId")]
        public long LastUpdateId { get; set; }
       
        [JsonProperty("bids")]
        public IList<string[]> Bids { get; set; }
        
        [JsonProperty("asks")]
        public IList<string[]> Asks { get; set; }
    }
}
