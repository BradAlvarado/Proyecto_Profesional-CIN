using System.Reflection;
using System.Runtime.Loader;

namespace Sistema_CIN.Extension
{
	public class CustomAssemblyLoadContext : AssemblyLoadContext
	{
		public IntPtr LoadUnmanagedLibrary(string absolutePath)
		{
			return LoadUnmanagedDll(absolutePath);
		}
		protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
		{
			return LoadUnmanagedDllFromPath(unmanagedDllName);
		}
		protected override Assembly Load(AssemblyName assemblyName)
		{
			throw new NotImplementedException();
		}
	}
}
