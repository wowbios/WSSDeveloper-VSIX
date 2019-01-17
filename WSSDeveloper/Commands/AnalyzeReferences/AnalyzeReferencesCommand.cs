using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using VSLangProj;

using Consts = WSSConsulting.WSSDeveloper.Constants.Commands.AnalyzeReferences;

namespace WSSConsulting.WSSDeveloper.Commands
{
    /// <summary>
    /// Команда анализа ссылок в проекте.
    /// </summary>
    internal class AnalyzeReferencesCommand : WSSCommandBase
    {
        /// <summary>
        /// Срабатывает при выполнении команды.
        /// </summary>
        protected override void OnExecute()
        {
            DirectoryInfo gacDir = new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("windir"), "assembly"));
            if (!gacDir.Exists)
                throw new Exception("Не удалось определить расположение GAC");


            SelectedItem refItem = this.DTEInfo.DTE.SelectedItems.FirstOrDefaultOfType<SelectedItem>();
            if (refItem == null || refItem.Name != "References")
                throw new Exception("References node expected");

            Project currentProject = (this.DTE.ActiveSolutionProjects as Array).GetValue(0) as Project;
            VSProject vsLangProject = currentProject.Object as VSProject;
            if (vsLangProject == null) return;

            StringBuilder message = new StringBuilder(4096);
            message.AppendLine("----------------------- Анализ ссылок проекта -----------------------");
            message.AppendLine("Всего ссылок: " + vsLangProject.References.Count);

            List<string> gacReferences = new List<string>();
            List<string> projectReferences = new List<string>();
            Dictionary<string, List<string>> referencesByClientChanges = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> referencesByWSSRelease = new Dictionary<string, List<string>>();
            List<string> referencesInCurrentVersion = new List<string>();

            int otherCount = 0;
            foreach (Reference reference in vsLangProject.References)
            {
                string name = reference.Name;
                string path = reference.Path;
                //dynamic dynamicRef = reference;

                //GAC
                if (path.ToLower().StartsWith(gacDir.FullName.ToLower()))
                {
                    gacReferences.Add(name/* + "[SpecificVersion=" + dynamicRef.SpecificVersion + "]"*/);
                    continue;
                }

                //комплекты заказчиков
                int clientChangesIndex = path.IndexOf(Consts.ClientChangesPathPart);
                if (clientChangesIndex != -1)
                {
                    int afterClientChangesIndex = clientChangesIndex + Consts.ClientChangesPathPart.Length + 1;
                    int afterCodeSlash = path.IndexOf('\\', afterClientChangesIndex);
                    if (afterCodeSlash != -1)
                    {
                        int afterReleaseSlash = path.IndexOf('\\', afterCodeSlash + 1);
                        if (afterCodeSlash != -1)
                        {
                            string key = path.Substring(afterClientChangesIndex, afterReleaseSlash - afterClientChangesIndex);
	                        if (referencesByClientChanges.TryGetValue(key, out List<string> referencesByKey))
                                referencesByKey.Add(name);
                            else
                                referencesByClientChanges.Add(key, new List<string> { name });
                        }
                    }
                    continue;
                }

                //фиксированная версия релиза
                int fixedIndex = path.IndexOf(Consts.FixedRelease, StringComparison.Ordinal);
                if (fixedIndex != -1)
                {
                    int afterFixedReleaseIndex = fixedIndex + Consts.FixedRelease.Length + 1;
                    int afterFixedReleaseSlashIndex = path.IndexOf('\\', afterFixedReleaseIndex);
                    string key = path.Substring(afterFixedReleaseIndex, afterFixedReleaseSlashIndex - afterFixedReleaseIndex);
	                if (referencesByWSSRelease.TryGetValue(key, out List<string> referencesByKey))
                        referencesByKey.Add(name);
                    else
                        referencesByWSSRelease.Add(key, new List<string> { name });
                    continue;
                }

                //текущая версия
                int currentVersionSystemIndex = path.IndexOf(Consts.CurrentVersionSystem, StringComparison.Ordinal);
                int currentVersionEDMSIndex = path.IndexOf(Consts.CurrentVersionEDMS, StringComparison.Ordinal);
                if (currentVersionEDMSIndex != -1 || currentVersionSystemIndex != -1)
                {
                    referencesInCurrentVersion.Add(name);
                    continue;
                }

                //проект в решении
                if (reference.SourceProject != null)
                {
                    projectReferences.Add($"[{name}] {reference.SourceProject.UniqueName}");
                    continue;
                }

                //другое
                otherCount++;
            }

            message.AppendLine(this.CreateMessage("Ссылок в [GAC]", gacReferences));
            foreach (KeyValuePair<string, List<string>> kvp in referencesByClientChanges)
            {
                string customerCodeAndRelease = kvp.Key;
                message.AppendLine(this.CreateMessage("Ссылок в комплект " + customerCodeAndRelease, kvp.Value));
            }

            foreach (KeyValuePair<string, List<string>> kvp in referencesByWSSRelease)
            {
                string release = kvp.Key;
                message.AppendLine(this.CreateMessage("Ссылок на релиз " + release, kvp.Value));
            }

            message.AppendLine(this.CreateMessage("Ссылок на текущую версию", referencesInCurrentVersion));

            message.AppendLine(this.CreateMessage("Ссылок на другие проекты", projectReferences));
            message.AppendLine("Остальные ссылки: " + otherCount);
            this.WriteToOutput(message.ToString());
        }

        /// <summary>
        /// Формирует сообщение о группе ссылок.
        /// </summary>
        /// <param name="refName"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        private string CreateMessage(string refName, List<string> references)
        {
            if (String.IsNullOrEmpty(refName))
                throw new ArgumentNullException(nameof(refName));
            if (references == null)
                throw new ArgumentNullException(nameof(references));

            StringBuilder messageBuilder = new StringBuilder(4096);
            messageBuilder.AppendFormat("{0}: {1}", refName, references.Count);
            if (references.Count > 0)
            {
                messageBuilder.Append("\n\t" + String.Join("\n\t", references.ToArray()));
                messageBuilder.AppendLine();
            }
            return messageBuilder.ToString();
        }
    }
}
