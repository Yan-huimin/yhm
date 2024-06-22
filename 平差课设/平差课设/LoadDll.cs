using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Adjustment_course_design
{
    internal class LoadDll
    {
            public static void LoadResourceDll()
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }

            private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                string assName = new AssemblyName(args.Name).Name;
                string resName = "Adjustment_course_design.My_dll." + assName + ".dll";


                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName))
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            }

    }
}
