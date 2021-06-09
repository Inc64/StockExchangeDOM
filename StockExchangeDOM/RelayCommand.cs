using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockExchangeDOM
{
	/// <summary>
	/// Делегат CommandEventHandler.
	/// </summary>
	public delegate void CommandEventHandler(object sender, CommandEventArgs args);

	/// <summary>
	/// Делегат CancelCommandEvent.
	/// </summary>
	public delegate void CancelCommandEventHandler(object sender, CancelCommandEventArgs args);

	/// <summary>
	/// SimpleCommand класс,
	/// Реализует ICommand,
	/// Зпускает связанную с коммандой функцию.
	/// </summary>
	public class RelayCommand : ICommand
	{
		/// <summary>
		/// Инициализирует новый объект класса <see cref="RelayCommand"/>
		/// </summary>
		/// <param name="action">Действие </param>
		/// <param name="canExecute">если указано <c>true</c> [can execute].</param>
		public RelayCommand(Action action, bool canExecute = true)
		{
			// Set the action.
			this.action = action;
			this.canExecute = canExecute;
		}

		/// <summary>
		/// Инициализирует новый объект класса <see cref="RelayCommand"/>
		/// </summary>
		/// <param name="parameterizedAction">Параметризированное действие </param>
		/// <param name="canExecute">если указано <c>true</c> [can execute].</param>
		public RelayCommand(Action<object> parameterizedAction, bool canExecute = true)
		{
			// Set the action.
			this.parameterizedAction = parameterizedAction;
			this.canExecute = canExecute;
		}

		/// <summary>
		/// Событие при изменении возможности выполнения
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Событие возникает при выполнении
		/// </summary>
		public event CancelCommandEventHandler Executing;

		/// <summary>
		/// Событие возникает, когда комманда исполнена
		/// </summary>
		public event CommandEventHandler Executed;

		/// <summary>
		/// Выполнение комманды.
		/// </summary>
		/// <param name="param">Параметр.</param>
		public virtual void DoExecute(object param)
		{
			// Вызов выполнения комманды с возможностью отмены.
			CancelCommandEventArgs args = new CancelCommandEventArgs() { Parameter = param, Cancel = false };
			InvokeExecuting(args);

			// Если выполнение отменено - выход.
			if (args.Cancel)
			{
				return;
			}

			// Вызов действия c параметром или без.
			InvokeAction(param);

			// Вызов функции выполнения.
			InvokeExecuted(new CommandEventArgs() { Parameter = param });
		}

		protected void InvokeAction(object param)
		{
			Action theAction = action;
			Action<object> theParameterizedAction = parameterizedAction;
			if (theAction != null)
			{
				theAction();
			}
			else
			{
				theParameterizedAction?.Invoke(param);
			}
		}

		protected void InvokeExecuted(CommandEventArgs args)
		{
			Executed?.Invoke(this, args);
		}

		protected void InvokeExecuting(CancelCommandEventArgs args)
		{
			Executing?.Invoke(this, args);
		}

		/// <summary>
		/// Действие (или параметризованное действие), вызываемое при вызове команды.
		/// </summary>
		protected Action action = null;
		protected Action<object> parameterizedAction = null;

		/// <summary>
		/// Индикатор может ли выполниться комманда.
		/// </summary>
		private bool canExecute = false;

		/// <summary>
		/// Задания индикатора возможности выполнения комманды.
		/// </summary>
		/// <value>
		/// 	<c>true</c> если выполнение возможно; иначе - <c>false</c>.
		/// </value>
		public bool CanExecute
		{
			get => canExecute;
			set
			{
				if (canExecute != value)
				{
					canExecute = value;
					CanExecuteChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		#region ICommand Members

		/// <summary>
		/// Определяет метод, сигнализирующий, может ли команда выполняться в текущем состоянии.
		/// </summary>
		/// <param name="parameter">Параметр комманды. Значение может быть null.</param>
		/// <returns>
		/// true если комманда может быть выполнена, иначе - false.
		/// </returns>
		bool ICommand.CanExecute(object parameter)
		{
			return canExecute;
		}

		/// <summary>
		/// Определяет метод, вызываемый при вызове команды.
		/// </summary>
		/// <param name="parameter">Параметр комманды. Значение может быть null.</param>
		void ICommand.Execute(object parameter)
		{
			DoExecute(parameter);
		}

		#endregion
	}

	/// <summary>
	/// CommandEventArgs - объёртка параматра комманды.
	/// </summary>
	public class CommandEventArgs : EventArgs
	{
		/// <summary>
		/// Указание, получение параметра.
		/// </summary>
		/// <value>Параметр.</value>
		public object Parameter { get; set; }
	}

	/// <summary>
	/// CancelCommandEventArgs - объёртка параматра комманды с возможностью отмены.
	/// </summary>
	public class CancelCommandEventArgs : CommandEventArgs
	{
		/// <summary>
		/// Задаёт или получает значение, указывающее нужно ли отменить эту <see cref="CancelCommandEventArgs"/> комманду.
		/// </summary>
		/// <value><c>true</c> если отменить, иначе - <c>false</c>.</value>
		public bool Cancel { get; set; }
	}
}
