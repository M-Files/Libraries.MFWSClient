using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFaaP.MFWSClient.Tests
{
	[TestClass]
	public class SetupAssemblyInitializer
	{
		[AssemblyInitialize]
		public static void AssemblyInit(TestContext context)
		{
			var assemblies = new List<string>()
			{
				"System.Text.Json",
				"System.Runtime.CompilerServices.Unsafe"
			};
			var assemlbyResolveHelper = new AssemblyResolveHelper(assemblies);
			AppDomain.CurrentDomain.AssemblyResolve += assemlbyResolveHelper.OnAssemlbyResolve;
		}
	}
	public class AssemblyResolveHelper
	{
		public AssemblyResolveHelper(string assemblyName)
		{
			_assemlbyNames.Add(assemblyName);
		}

		public AssemblyResolveHelper(List<string> assemblyNames)
		{
			if (assemblyNames != null)
			{
				_assemlbyNames = assemblyNames;
			}
		}

		private List<string> _assemlbyNames = new List<string>();

		public System.Reflection.Assembly OnAssemlbyResolve(object sender, ResolveEventArgs args)
		{
			var assemblyName = _assemlbyNames.FirstOrDefault(i => i == args.Name.Split(',')[0]);
			if (assemblyName != null)
			{
				string assemlbyPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), assemblyName + ".dll");

				if (File.Exists(assemlbyPath))
				{
					return System.Reflection.Assembly.LoadFrom(assemlbyPath);
				}
			}
			return null;
		}
	}
}
