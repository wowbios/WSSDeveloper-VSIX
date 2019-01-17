using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace WSS.Wizards
{
	/// <summary>
	/// Мастер шаблона кастомного проекта
	/// </summary>
	public class WSSCustomProjectTemplateWizard : WSSItemTemplateWizard
	{
		/// <summary>
		/// Выполняет операции при запуске мастера шаблонов.
		/// </summary>
		/// <param name="automationObject"></param>
		/// <param name="replacementsDictionary"></param>
		/// <param name="runKind"></param>
		/// <param name="customParams"></param>
		protected override void RunStart(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			base.RunStart(automationObject, replacementsDictionary, runKind, customParams);


			// TODO: [AV] CR: Раскомментировать, если надо отображать какое-нибудь окно перед созданием проекта (добавлением шаблона?)

			/*
            //показываем форму настройки кастомного проекта
            using (CustomProjectWizardForm customForm = new CustomProjectWizardForm())
            {
                customForm.ShowDialog();

                //код заказчика
                replacementsDictionary.Add("$customercode$", customForm.CustomerCode);

                //название кастомного проекта
                string customProjectName = "WSSC.V4.DMS." + customForm.CustomerCode;
                replacementsDictionary.Add("$customprojectname$", customProjectName);

                //подменяем папку с проектом, как у кастомного проекта
                string directory = replacementsDictionary["$destinationdirectory$"];
                directory = directory.Replace("\\" + replacementsDictionary["$safeprojectname$"],
                                              "\\" + customProjectName);
                replacementsDictionary["$destinationdirectory$"] = directory;

                //подменяем названия проекта и решения
                replacementsDictionary["$projectname$"] =
                replacementsDictionary["$specifiedsolutionname$"] =
                replacementsDictionary["$safeprojectname$"] = customProjectName;
            }*/
		}
	}
}
