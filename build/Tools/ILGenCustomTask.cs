using Microsoft.Build.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Linq;
using Task = Microsoft.Build.Utilities.Task;

namespace ILInjection
{
    public class ILGenCustomTask : Task
    {
        public string AssemblyName { get; set; } = string.Empty;

        public override bool Execute()
        {
            if (string.IsNullOrWhiteSpace(AssemblyName))
            {
                Log.LogError($"'{nameof(AssemblyName)}' is NullOrWhitespace!");
                return false;
            }

            var dllPath = AssemblyName;
            if (!File.Exists(dllPath))
            {
                Log.LogError($"Dll Not Found -> Path: '{dllPath}'");
                return false;
            }

            try
            {
                RunTask(dllPath);
            }
            catch (Exception ex)
            {
                Log.LogMessage(MessageImportance.High, "STACK TRACE: ");
                Log.LogMessage(MessageImportance.High, ex.StackTrace.ToString());

                Log.LogErrorFromException(ex);
                return false;
            }

            return true;
        }

        private void RunTask(string dllPath)
        {
            Log.LogMessage(MessageImportance.High, "Start...");

            var assembly = AssemblyDefinition.ReadAssembly(dllPath, new ReaderParameters(ReadingMode.Deferred)
            {
                ReadWrite = true,
                InMemory = false
            });

            var item = assembly.MainModule.Types.FirstOrDefault(x => x.IsAbstract && x.IsSealed && x.FullName == WellknowMethods.CLASS);
            if (item == null) return;

            foreach (var method in item.Methods.Where(x => x.IsStatic && x.IsRuntime))
            {
                if (!WellknowMethods.Items.TryGetValue(method.FullName, out var items))
                {
                    Log.LogError($"  - '{method.FullName}' is not a well-known method.");
                    continue;
                }

                Log.LogMessage(MessageImportance.High, $"  - Find Method: '{method.FullName}'");

                method.IsRuntime = false;
                method.IsManaged = method.IsIL = true;

                var body = new MethodBody(method);

                items.Invoke(method, body, body.Instructions);

                method.Body = body;
            }
            
            Log.LogMessage(MessageImportance.High, "Postprocess...");

            // NOTE:
            //  any Non-IL method find --> no
            var isMixed = assembly.MainModule.Types.Any(x => x.Methods.Any(y => !y.IsIL));
            if (isMixed)
            {
                Log.LogError("Non-IL Method List: ");

                foreach (var type in assembly.MainModule.Types)
                    foreach (var method in item.Methods.Where(x => !x.IsIL))
                        Log.LogError($"  - {method.FullName}");

                throw new InvalidOperationException("ILGenCustomTask:: Mixed-image not supported");
            }
            else
                assembly.MainModule.Attributes |= ModuleAttributes.ILOnly;

            Log.LogMessage(MessageImportance.High, "Write assembly...");

            assembly.Write();
        }
    }
}