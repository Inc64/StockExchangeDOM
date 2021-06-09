using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace StockExchangeDOM.DataProvider
{
    public class BinanceExchangeProvider
    {
        public enum eDepth : int
        {
            _5 = 5,
            _10 = 10,
            _20 = 20,
        }

        public enum eUpdateSpeed : int
        {
            _100 = 100,
            _1000 = 1000,
        }

        //@"wss://stream.binance.com:9443/ws/ethbtc@depth20@100ms";
        private const string urlBinance = @"wss://stream.binance.com:9443/ws/";
        private const string urlBinanceExchangeInfo = @"https://api.binance.com/api/v3/exchangeInfo";

        private string ticker = "ethbtc";
        private eDepth depth = eDepth._20;
        private eUpdateSpeed updateSpeed = eUpdateSpeed._100;
        private WebSocket websocket = null;

        public List<string> ExchangeMarketTikers { get; private set; } = new List<string>();

        public string Ticker
        {
            get => ticker;
            set
            {
                ticker = value;
                ModifyConnection();
            }
        }

        public eDepth Depth
        {
            get => depth;
            set
            {
                depth = value;
                ModifyConnection();
            }
        }

        public eUpdateSpeed UpdateSpeed
        {
            get => updateSpeed;
            set
            {
                updateSpeed = value;
                ModifyConnection();
            }
        }

        public delegate void ChangesEvent(BinanceTickerDepthInfo tickerInfo);

        public ChangesEvent CallBackChanges { get; set; }

        public async Task ReceiveExchangeMarketTikers()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(urlBinanceExchangeInfo);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject jdata = JObject.Parse(responseBody);
            JArray symbols = (JArray)jdata["symbols"];
            ExchangeMarketTikers = symbols.Select(c => ((string)c["symbol"]).ToLower()).ToList();
        }

        public void StartConnection() 
        {
            string urlString = $"{urlBinance}{ticker}@depth{(int)depth}@{(int)updateSpeed}ms";
            string paramString = $"{ticker}@depth{(int)depth}";
            long listenerId = 1;

            websocket = new WebSocket(urlString, sslProtocols: SslProtocols.Tls12);
            websocket.Opened += (sender, e) =>
            {
                websocket.Send(
                   JsonConvert.SerializeObject(
                       new
                       {
                           method = "SUBSCRIBE",
                           @params = new List<string> { paramString },
                           id = listenerId,
                       }
                   )
               );
            };
            websocket.MessageReceived += (sender, e) =>
            {
                dynamic data = JObject.Parse(e.Message);

                if (data.id == listenerId && data.result == null)
                {
                    //Console.WriteLine("subscribed!");
                }

                if (data.code != null)
                {
                    websocket.CloseAsync();
                }

                if (data.lastUpdateId > -1)
                {
                    var rs = ((JObject)data).ToObject(typeof(BinanceTickerDepthInfo)) as BinanceTickerDepthInfo;
                    CallBackChanges?.Invoke(rs);
                }

            };

            websocket.Closed += (sender, ё) =>
            {
                websocket.Send(
                   JsonConvert.SerializeObject(
                       new
                       {
                           method = "UNSUBSCRIBE",
                           @params = new List<string> { paramString },
                           id = listenerId,
                       }
                   )
               );
            };

            websocket.Open();
        }

        public void CloseConnection()
        {
            if (websocket != null)
            {
                websocket.Close();
                websocket.Dispose();
                websocket = null;
            }
        }

        private void ModifyConnection()
        {
            if(websocket != null)
            {
                CloseConnection();
                StartConnection();
            }
        }

    }
}
