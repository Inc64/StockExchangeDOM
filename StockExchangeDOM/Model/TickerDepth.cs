using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeDOM.Model
{
    public enum eTickerOffer
    {
        bid,
        ask
    }
    
    public class TickerDepth : NotifyBase
    {
        private eTickerOffer offer = eTickerOffer.bid;
        private string price = String.Empty;
        private string quantity = String.Empty;

        public eTickerOffer Offer 
        {
            get => offer;
            set
            {
                offer = value;
                OnPropertyChanged();
            }
        }

        public string Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }

        public string Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged();
            }
        }
    }
}
