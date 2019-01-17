using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace WSSConsulting.WSSDeveloper.Commands
{
    /// <summary>
    /// Парсер файла Manifest.xml
    /// </summary>
    internal class ManifestParser
    {
        /// <summary>
        /// Элемент проекта.
        /// </summary>
        public ProjectItem Item { get; private set; }

        /// <summary>
        /// Полный путь к файлу.
        /// </summary>
        public readonly string ItemFullPath;

        /// <summary>
        /// Элемент проекта.
        /// </summary>
        /// <param name="item"></param>
        public ManifestParser(ProjectItem item)
        {
			this.Item = item ?? throw new ArgumentNullException(nameof(item));
            this.ItemFullPath = item.GetFullPath();
        }

        /// <summary>
        /// Добавляет файлы в Manifest.xml.
        /// </summary>
        /// <param name="relPaths"></param>
        public void AddFiles(List<string> relPaths)
        {
            if (relPaths == null)
                throw new ArgumentNullException(nameof(relPaths));

            if (relPaths.Count == 0) return;

            string templateToSearch = "</TemplateFiles>";
            string allText = File.ReadAllText(this.ItemFullPath);
            int templateFilesIndex = allText.IndexOf(templateToSearch, StringComparison.Ordinal);
            if (templateFilesIndex == -1)
                throw new NotificationException("Некорректный формат файла {0}. Не найден элемент \"{1}\"", Constants.ManifestFileName, templateToSearch);

            int insertIndex = templateFilesIndex;

            StringBuilder builder = new StringBuilder(allText);

            bool rowInserted = false;
            foreach (string relPath in relPaths)
            {
                string row = String.Format("\t\t<TemplateFile Location='{1}'/>{0}", Environment.NewLine, relPath);
                if (allText.IndexOf(row, StringComparison.Ordinal) == -1)
                {
                    rowInserted = true;
                    builder.Insert(insertIndex, row);
                    insertIndex += row.Length;
                }
            }
            if (rowInserted)
            {
                File.WriteAllText(this.ItemFullPath, builder.ToString(), Encoding.UTF8);
                Window win = this.Item.DTE.ItemOperations.OpenFile(this.ItemFullPath, EnvDTE.Constants.vsViewKindTextView);
            }
        }
    }
}
