using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System.Collections.Generic;

namespace ILInjection
{
    using static Instruction;
    internal static class WellknowMethods
    {
        public const string CLASS = "SuperComicLib.Runtime.XUnsafe";

        private static readonly ILGenAction LDA0 = new ILGenAction(ACTION_LDA0_RET);
        private static readonly ILGenAction ADD = new ILGenAction(ACTION_ADD);

        public static readonly Dictionary<string, ILGenAction> Items = new Dictionary<string, ILGenAction>
        {
            { $"T& {CLASS}::AsRef(T&)", LDA0 },
            { $"T {CLASS}::As(System.Object)", LDA0 },
            { $"TTo& {CLASS}::As(TFrom&)", LDA0 },
            { $"T& {CLASS}::Add(T&,System.Int32)", ADD },
        };

        private static void ACTION_LDA0_RET(MethodReference method, MethodBody body, Collection<Instruction> il)
        {
            body.MaxStackSize = 1;

            il.Add(Create(OpCodes.Ldarg_0));
            il.Add(Create(OpCodes.Ret));
        }

        private static void ACTION_ADD(MethodReference method, MethodBody body, Collection<Instruction> il)
        {
            body.MaxStackSize = 3;

            il.Add(Create(OpCodes.Ldarg_0));
            il.Add(Create(OpCodes.Ldarg_1));
            il.Add(Create(OpCodes.Sizeof, method.GenericParameters[0]));
            il.Add(Create(OpCodes.Conv_I));
            il.Add(Create(OpCodes.Mul));
            il.Add(Create(OpCodes.Add));
            il.Add(Create(OpCodes.Ret));
        }
    }
}
