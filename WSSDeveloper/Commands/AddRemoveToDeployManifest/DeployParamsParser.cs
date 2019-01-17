using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace WSSConsulting.WSSDeveloper.Commands
{
    /// <summary>
    /// Парсер файла DeployParams.txt.
    /// </summary>
    internal class DeployParamsParser
    {
        /// <summary>
        /// Файл - элемент проекта.
        /// </summary>
        public ProjectItem Item { get; }

        /// <summary>
        /// Полный путь к файлу.
        /// </summary>
        public readonly string ItemFullPath;

        /// <summary>
        /// Парсер файла DeployParams.txt.
        /// </summary>
        /// <param name="item"></param>
        public DeployParamsParser(ProjectItem item)
        {
			this.Item = item ?? throw new ArgumentNullException(nameof(item));
            this.ItemFullPath = item.GetFullPath();
        }

        /// <summary>
        /// Добавляет файлы в DeployParams.txt.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<string> AddFiles(IEnumerable<ProjectItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            string allText = File.ReadAllText(this.ItemFullPath);

            bool changed = false;
            List<string> relativePaths = new List<string>();
            StringBuilder rows = new StringBuilder();
            foreach (ProjectItem projectItem in items)
            {
                string itemRelPath = projectItem.GetProjectRelativePath();
                string deployRelativePath = $"LAYOUTS\\WSS\\{this.Item.ContainingProject.Name}\\{itemRelPath}";
                string row = $"{Environment.NewLine}..\\{itemRelPath}\t\t\t\t\t{deployRelativePath}";

                relativePaths.Add(deployRelativePath);
                if (allText.IndexOf(row, StringComparison.Ordinal) == -1)
                {
                    rows.Append(row);
                    changed = true;
                }
            }

            if (changed)
            {
                File.AppendAllText(this.ItemFullPath, rows.ToString(), Encoding.UTF8);
                Window win = this.Item.DTE.ItemOperations.OpenFile(this.ItemFullPath, EnvDTE.Constants.vsViewKindTextView);
            }
            return relativePaths;
        }
    }
}
