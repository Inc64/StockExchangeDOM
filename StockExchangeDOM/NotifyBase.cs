using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockExchangeDOM
{
    [Serializable]
	public class NotifyBase : INotifyPropertyChanged
	{
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string info = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
