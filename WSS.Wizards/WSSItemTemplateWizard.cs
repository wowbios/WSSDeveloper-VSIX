using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;

namespace WSS.Wizards
{
	/// <summary>
	/// Базовый класс IWizard для шаблонов WSS.
	/// </summary>
	public class WSSItemTemplateWizard : IWizard
	{
		/// <summary>
		/// Runs custom wizard logic at the beginning of a template wizard run.
		/// </summary>
		/// <param name="automationObject">The automation object being used by the template wizard.</param><param name="replacementsDictionary">The list of standard parameters to be replaced.</param><param name="runKind">A <see cref="T:Microsoft.VisualStudio.TemplateWizard.WizardRunKind"/> indicating the type of wizard run.</param><param name="customParams">The custom parameters with which to perform parameter replacement in the project.</param>
		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			try
			{
				this.RunStart(automationObject, replacementsDictionary, runKind, customParams);
			}
			catch (Exception ex)
			{
				string msg = "Произошла ошибка в расширении WSS:\n" + ex.ToString();
				MessageBox.Show(msg, "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Выполняет операции при запуске мастера шаблонов.
		/// </summary>
		/// <param name="automationObject"></param>
		/// <param name="replacementsDictionary"></param>
		/// <param name="runKind"></param>
		/// <param name="customParams"></param>
		protected virtual void RunStart(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			EnvDTE80.DTE2 dte = (EnvDTE80.DTE2)automationObject;

			new ReplacementDictionaryHelper(dte, replacementsDictionary).FillDictionary();
		}

		///// <summary>
		///// Добавляет конфигурацию Compile, если такая отсутствует.
		///// </summary>
		///// <param name="proj"></param>
		//private void EnsureCompileConfiguration(Project proj)
		//{
		//	ConfigurationManager confManager = proj.ConfigurationManager;

		//	const string compileConfigName = "Compile";

		//	if (((object[])confManager.ConfigurationRowNames).Contains(compileConfigName))
		//		return;

		//	//propagate - true, для добавления конфигурации в решение. Добавляем только если в решении ещё нет такой конфигурации
		//	bool propagate = !((object[])proj.DTE.Solution.Item(1).ConfigurationManager.ConfigurationRowNames).Contains(compileConfigName);

		//	//добавляем конфигурацию Compile, копируя из конфигурации Debug
		//	Configuration compileConfig = confManager.AddConfigurationRow(compileConfigName, "Debug", propagate).Item(1);

		//	//корректируем OutputPath в bin
		//	compileConfig.Properties.Item("OutputPath").Value = "bin\\";
		//}

		/// <summary>
		/// Indicates whether the specified project item should be added to the project.
		/// </summary>
		/// <returns>
		/// true if the project item should be added to the project; otherwise, false.
		/// </returns>
		/// <param name="filePath">The path to the project item.</param>
		public bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}

		/// <summary>
		/// Runs custom wizard logic when the wizard has completed all tasks.
		/// </summary>
		public void RunFinished()
		{

		}

		/// <summary>
		/// Runs custom wizard logic before opening an item in the template.
		/// </summary>
		/// <param name="projectItem">The project item that will be opened.</param>
		public void BeforeOpeningFile(ProjectItem projectItem)
		{
		}

		/// <summary>
		/// Runs custom wizard logic when a project item has finished generating.
		/// </summary>
		/// <param name="projectItem">The project item that finished generating.</param>
		public void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
		}

		/// <summary>
		/// Runs custom wizard logic when a project has finished generating.
		/// </summary>
		/// <param name="project">The project that finished generating.</param>
		public void ProjectFinishedGenerating(Project project)
		{
			//this.EnsureCompileConfiguration(project);
		}
	}
}
