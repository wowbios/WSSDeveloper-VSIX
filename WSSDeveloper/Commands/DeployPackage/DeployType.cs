using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Вид деплоя.
	/// </summary>
	internal abstract class DeployType
	{
		/// <summary>
		/// Переключатель.
		/// </summary>
		public RadioButton Switcher { get; }

		/// <summary>
		/// Дочерние элементы.
		/// </summary>
		public Control[] ChildControls { get; }

		/// <summary>
		/// Родительская форма.
		/// </summary>
		public DeployPackageDestinationForm Form { get; }

		private bool _IsActive;

		/// <summary>
		/// Определяет, активен ли выбранный тип деплоя в данный момент.
		/// </summary>
		public bool IsActive
		{
			get { return _IsActive; }
			set
			{
				_IsActive = value;
				this.OnActiveChanged(null, null);
			}
		}

		/// <summary>
		/// Вид деплоя.
		/// </summary>
		/// <param name="switcher">Переключатель.</param>
		/// <param name="childControls">Дочерние элементы.</param>
		/// <param name="form">Родительская форма.</param>
		public DeployType(RadioButton switcher, Control[] childControls, DeployPackageDestinationForm form)
		{
			this.Switcher = switcher ?? throw new ArgumentNullException(nameof(switcher));
			this.ChildControls = childControls ?? throw new ArgumentNullException(nameof(childControls));
			this.Form = form ?? throw new ArgumentNullException(nameof(form));
			this.Switcher.CheckedChanged += (s, e) =>
											{
												this.IsActive = this.Switcher.Checked;
											};
		}

		/// <summary>
		/// Выполняет действия при инициализации контролов для данного вида деплоя.
		/// </summary>
		internal virtual void OnInit()
		{

		}

		/// <summary>
		/// Выполняет деплой модуля.
		/// </summary>
		/// <param name="deployInfo"></param>
		internal abstract bool Deploy(DeployInfo deployInfo);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="args"></param>
		private void OnActiveChanged(object obj, EventArgs args)
		{
			foreach (Control ctrl in this.ChildControls)
			{
				ctrl.Enabled = this.IsActive;
			}
		}
	}
}
