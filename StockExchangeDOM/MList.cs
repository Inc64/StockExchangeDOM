using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeDOM
{
	[Serializable]
	public class MList<T> : ObservableCollection<T>
	{
		private bool suppressNotification;

		public override event NotifyCollectionChangedEventHandler CollectionChanged;

		protected virtual void OnCollectionChangedMultiItem(NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventHandler handlers = CollectionChanged;
			if (handlers != null)
			{
				foreach (NotifyCollectionChangedEventHandler handler in
					handlers.GetInvocationList())
				{
					if (handler.Target is System.Windows.Data.CollectionView view)
					{
						view.Refresh();
					}
					else
					{
						handler(this, e);
					}
				}
			}
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!suppressNotification)
			{
				base.OnCollectionChanged(e);
				if (CollectionChanged != null)
				{
					CollectionChanged.Invoke(this, e);
				}
			}
		}

		public void AddRange(IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			suppressNotification = true;

			foreach (T item in list)
			{
				Add(item);
			}

			suppressNotification = false;

			OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
		}

		public void RemoveRange(IEnumerable<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			suppressNotification = true;

			List<T> removed = new List<T>();
			foreach (T item in list)
			{
				if (IndexOf(item) > -1)
				{
					Remove(item);
					removed.Add(item);
				}
			}
			suppressNotification = false;

			OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
		}
	}
}
