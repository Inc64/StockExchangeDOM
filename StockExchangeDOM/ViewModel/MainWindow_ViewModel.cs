using StockExchangeDOM.DataProvider;
using StockExchangeDOM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace StockExchangeDOM.ViewModel
{
    public class MainWindow_ViewModel : NotifyBase
    {
        private BinanceExchangeProvider binanceEP = null;
        private MList<string> tickerList = new MList<string>();
        private string tickerSelected = String.Empty;
        private object _itemsLock = new object ();
        bool _IsUpdating = false;
        bool isContentEnabled = false;


        private static Lazy<Dispatcher> dispatcher = new Lazy<Dispatcher>(() => Application.Current.Dispatcher);

        public MainWindow_ViewModel()
        {
            InitExchangeProvider();
            OnWindowClosing = new RelayCommand(o =>
            {
                if(binanceEP!=null)
                {
                    binanceEP.CloseConnection();
                }
            });

        }

        public RelayCommand OnWindowClosing { get; set; }


        public MList<string> TickerList 
        {
            get => tickerList;
            private set
            {
                tickerList = value;
                OnPropertyChanged();
            }
        }

        public bool IsContentEnabled
        {
            get => isContentEnabled;
            set
            {
                if (isContentEnabled != value)
                {
                    isContentEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public MList<TickerDepth> TickersDepth { get; set; } = new MList<TickerDepth>();

        public string TickerSelected 
        {
            get => tickerSelected;
            set
            {
                if (tickerSelected != value)
                {
                    tickerSelected = value;
                    if (binanceEP != null)
                    {
                        IsContentEnabled = false;
                        binanceEP.Ticker = value;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private async void InitExchangeProvider()
        {
            binanceEP = new BinanceExchangeProvider();
            await binanceEP.ReceiveExchangeMarketTikers();
            TickerList.Clear();
            TickerList.AddRange(binanceEP.ExchangeMarketTikers);
            TickerSelected = TickerList.Count > 0 ? TickerList[0] : String.Empty;

            binanceEP.CallBackChanges = OnExchangeDataUpdate;
            binanceEP.StartConnection();
        }

        private void OnExchangeDataUpdate(BinanceTickerDepthInfo tickerInfo)
        {
            IsContentEnabled = true;
            CreateTickersDepth(tickerInfo);
            //var count = tickerInfo.Asks.Count + tickerInfo.Bids.Count;

            //if (TickerList == null || TickerList.Count != count)
            //{
            //    CreateTickersDepth(tickerInfo);
            //}
            //else
            //{
            //    UpdateTickersDepth(tickerInfo);
            //}
        }

        private void CreateTickersDepth(BinanceTickerDepthInfo tickerInfo)
        {
            if (_IsUpdating) return;

            _IsUpdating = true;
            var bids = tickerInfo.Bids.Aggregate(new List<TickerDepth>(), (lst, val) =>
            {
                var p = new TickerDepth()
                {
                    Offer = eTickerOffer.bid,
                    Price = val[0],
                    Quantity = val[1],
                };

                lst.Add(p);

                return lst;
            });
            var ascs = tickerInfo.Asks.Aggregate(new List<TickerDepth>(), (lst, val) =>
            {
                var p = new TickerDepth()
                {
                    Offer = eTickerOffer.ask,
                    Price = val[0],
                    Quantity = val[1],
                };

                lst.Add(p);

                return lst;
            });

            if (dispatcher.Value.CheckAccess())
            {
                TickersDepth.Clear();
                TickersDepth.AddRange(bids);
                TickersDepth.AddRange(ascs);
            }
            else
            {
                dispatcher.Value.InvokeAsync(() =>
                {
                    TickersDepth.Clear();
                    TickersDepth.AddRange(bids);
                    TickersDepth.AddRange(ascs);
                });
            }

            _IsUpdating = false;
        }

        private void UpdateTickersDepth(BinanceTickerDepthInfo tickerInfo)
        {

        }

    }
}
