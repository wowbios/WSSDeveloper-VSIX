using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Вспомогательный объект для получения статуса DTE.
	/// </summary>
	internal class DTEInfo
	{
		/// <summary>
		/// DTE.
		/// </summary>
		public DTE DTE { get; }

		/// <summary>
		/// Вспомогательный объект для получения статуса DTE.
		/// </summary>
		/// <param name="dte">DTE.</param>
		internal DTEInfo(DTE dte)
		{
			this.DTE = dte ?? throw new ArgumentNullException(nameof(dte));
		}

		private bool __init_SelectedProject;
		private Project _SelectedProject;
		/// <summary>
		/// Выбранный проект.
		/// </summary>
		internal Project SelectedProject
		{
			get
			{
				if (!__init_SelectedProject)
				{
					if (this.ItemsFromMultipleProjects)
						throw new Exception("Попытка получить текущий проект, когда выбраны элементы из разных проектов");

					_SelectedProject = this.SelectedProjects.Single();
					if (_SelectedProject == null)
						throw new Exception("Не удалось получить текущий проект");

					__init_SelectedProject = true;
				}
				return _SelectedProject;
			}
		}

		private bool __init_SelectedProjects;
		private ReadOnlyCollection<Project> _SelectedProjects;
		/// <summary>
		/// Выбранные проекты.
		/// </summary>
		private ReadOnlyCollection<Project> SelectedProjects
		{
			get
			{
				Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
				if (!__init_SelectedProjects)
				{
					List<Project> projects = this.DTE.SelectedItems.OfType<SelectedItem>().Where(x =>
					{
						Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
						return x.Project != null;
					}).Select(x =>
					  {
						  Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
						  return x.Project;
					  })
						.ToList();

					if (projects.Count == 0)
					{
						if (this.SelectedItems.Count > 0)
						{
							ProjectItem projectItem = this.SelectedItems.FirstOrDefault(x =>
							{
								Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
								return x.ContainingProject != null;
							});

							if (projectItem != null)
								projects.Add(projectItem.ContainingProject);
						}
					}
					_SelectedProjects = new ReadOnlyCollection<Project>(projects);

					__init_SelectedProjects = true;
				}
				return _SelectedProjects;
			}
		}

		private bool __init_SelectedItems;
		private List<ProjectItem> _SelectedItems;
		/// <summary>
		/// Выбранные элементы.
		/// </summary>
		internal List<ProjectItem> SelectedItems
		{
			get
			{
				Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
				if (!__init_SelectedItems)
				{
					SelectedItems selectedItems = this.DTE.SelectedItems;
					if (selectedItems != null && selectedItems.Count > 0)
					{
						_SelectedItems = selectedItems.OfType<SelectedItem>()
							.Select(x =>
							{
								Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
								return x.ProjectItem;
							})
							.Where(x => x != null).ToList();
					}
					else
						_SelectedItems = new List<ProjectItem>();
					__init_SelectedItems = true;
				}
				return _SelectedItems;
			}
		}

		private bool __init_ItemsFromMultipleProjects;
		private bool _ItemsFromMultipleProjects;
		/// <summary>
		/// True, если выбранные элементы из разных проектов.
		/// </summary>
		internal bool ItemsFromMultipleProjects
		{
			get
			{
				Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
				if (!__init_ItemsFromMultipleProjects)
				{
					Project project = null;
					foreach (ProjectItem selectedItem in this.SelectedItems)
					{
						Project selectedProject = selectedItem.ContainingProject;
						if (project != null && selectedProject != null && selectedProject.FullName != project.FullName)
						{
							_ItemsFromMultipleProjects = true;
							break;
						}

						project = selectedProject;
					}

					__init_ItemsFromMultipleProjects = true;
				}
				return _ItemsFromMultipleProjects;
			}
		}
	}
}
