using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes
{
	/// <summary>
	/// Форма добавления в Types.
	/// </summary>
	internal partial class AddToTypesForm : Form
	{
		/// <summary>
		/// Классы из файла.
		/// </summary>
		internal List<ClassMetadata> Classes { get; private set; }

		/// <summary>
		/// Команда.
		/// </summary>
		internal AddToTypesXmlCommand AddToTypesXmlCommand { get; private set; }

		/// <summary>
		/// Форма добавления в Types.
		/// </summary>
		/// <param name="classes">Классы из файла.</param>
		/// <param name="addToTypesXmlCommand">Фабрика.</param>
		public AddToTypesForm(IEnumerable<ClassMetadata> classes, AddToTypesXmlCommand addToTypesXmlCommand)
		{
			if (classes == null) throw new ArgumentNullException(nameof(classes));
			this.Classes = new List<ClassMetadata>(classes);
			this.AddToTypesXmlCommand = addToTypesXmlCommand ?? throw new ArgumentNullException(nameof(addToTypesXmlCommand));

			this.InitializeComponent();
			this.Init();
		}

		/// <summary>
		/// Инициализирует форму.
		/// </summary>
		private void Init()
		{
			//заполняем классы.
			foreach (ClassMetadata classMetadata in this.Classes)
				ClassesBox.Items.Add(classMetadata);

			//выбираем первый
			ClassesBox.SelectedIndex = 0;

			//заполняем фабрики
			ListFormWebControlDefinition.Factory listFormFactory = new ListFormWebControlDefinition.Factory();
			ItemTypeBox.Items.AddRange(new object[]
			{
				listFormFactory,
				new DataObjectDefinition.Factory(),
				new FieldDefinition.Factory(),
				new ItemHandlerDefinition.Factory(),
				new JobDefinition.Factory(),
				new ReportProgramColumnDefinition.Factory()
			});

			//выбираем первую
			ItemTypeBox.SelectedIndex = 0;

			//добавляем обработчик события выбора
			ItemTypeBox.SelectedIndexChanged += this.ItemTypeBox_SelectedIndexChanged;

			//инициализируем для первого типа
			_CurrentDefinition = listFormFactory.Create(this.Classes.First());
			this.InitFormFactory();
		}

		private bool __init_TypesManager;
		private TypesXmlManager _TypesManager;
		/// <summary>
		/// Объект для работы с файлом Types.xml.
		/// </summary>
		internal TypesXmlManager TypesManager
		{
			get
			{
				if (!__init_TypesManager)
				{
					_TypesManager = new TypesXmlManager(this.AddToTypesXmlCommand);
					__init_TypesManager = true;
				}
				return _TypesManager;
			}
		}

		/// <summary>
		/// Текущее выбранное определение.
		/// </summary>
		private ItemDefinition _CurrentDefinition;

		/// <summary>
		/// Обработка смены типа.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemTypeBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			GroupBox controlsPanel = paramsGroupBox;

			if (_CurrentDefinition != null && _CurrentDefinition.PageControls != null && _CurrentDefinition.PageControls.Length > 0)
			{
				foreach (Control ctrl in _CurrentDefinition.PageControls)
					ctrl.Dispose();
			}
			controlsPanel.Controls.Clear();

			ItemDefinitionFactory factory = (ItemDefinitionFactory)ItemTypeBox.SelectedItem;
			_CurrentDefinition = factory.Create((ClassMetadata)ClassesBox.SelectedItem);
			this.InitFormFactory();
		}

		/// <summary>
		/// Инициализирует интерфейс для выбранной фабрики типа.
		/// </summary>
		private void InitFormFactory()
		{
			this.AutoSize = true;

			//размещаем контролы
			GroupBox controlsPanel = paramsGroupBox;
			Control[] pageControls = _CurrentDefinition.PageControls;
			if (pageControls != null)
			{
				int offsetY = 0;
				foreach (Control ctrl in pageControls)
				{
					Point prevLocation = ctrl.Location;
					ctrl.Location = new Point(prevLocation.X, prevLocation.Y + offsetY);
					offsetY += ctrl.Size.Height;
					controlsPanel.Controls.Add(ctrl);
					ctrl.Dock = DockStyle.Top;
				}
			}

			//авторазмер формы при изменении типа
			Size size = this.Size;
			this.AutoSize = false;
			this.Size = size;
			this.Height += panel1.Height;
		}

		/// <summary>
		/// Отмена.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// ОК.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			try
			{
				_CurrentDefinition.Validate();
			}
			catch (EmptyFormFieldException eex)
			{
				if (!WSSDeveloperPackage.ShowUserConfirmOkCancel(eex.Message + "\nПродолжить?", "Не заполнены поля"))
					return;
			}
			this.TypesManager.Add(_CurrentDefinition);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
