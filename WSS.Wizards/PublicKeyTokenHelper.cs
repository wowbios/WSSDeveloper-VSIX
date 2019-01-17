using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace WSS.Wizards
{
	/// <summary>
	/// Класс для получения открытого ключа сборки.
	/// </summary>
	internal class PublicKeyTokenHelper
	{

		/// <summary>
		/// Возвращает открытый ключ сборки проекта. Подразумевается, что сборка требует подписания.
		/// </summary>
		/// <param name="project"></param>
		/// <returns></returns>
		public string GetPublicKeyToken(Project project)
		{
			//ключ по умолчанию
			string publicKeyToken = "9f4da00116c38ec5";

			if (project == null)
				return publicKeyToken;

			bool signAssembly = (bool)project.Properties.Item("SignAssembly").Value;
			if (signAssembly)
			{
				object keyFileValue = project.Properties.Item("AssemblyOriginatorKeyFile").Value;
				string keyFile;
				if (keyFileValue != null && !String.IsNullOrEmpty(keyFile = keyFileValue.ToString()))
				{
					var snk = File.ReadAllBytes(Path.Combine(
						project.Properties.Item("FullPath").Value.ToString(), keyFile));

					byte[] publicKeyTokenBytes = this.GetPublicKeyToken(snk);

					publicKeyToken = String.Join("",
												 BitConverter.ToString(publicKeyTokenBytes)
															 .Split(new[] { '-' },
																	StringSplitOptions.RemoveEmptyEntries))
										   .ToLower();
				}

			}
			return publicKeyToken;
		}

		/// <summary>
		/// Возвращает контент публичного ключа.
		/// </summary>
		/// <param name="snk"></param>
		/// <returns></returns>
		private byte[] GetPublicKeyToken(byte[] snk)
		{
			StrongNameKeyPair skp = new StrongNameKeyPair(snk);

			using (var csp = new SHA1CryptoServiceProvider())
			{
				byte[] hash = csp.ComputeHash(skp.PublicKey);

				byte[] token = new byte[8];

				for (int i = 0; i < 8; i++)
				{
					token[i] = hash[hash.Length - i - 1];
				}

				return token;
			}
		}
	}
}
