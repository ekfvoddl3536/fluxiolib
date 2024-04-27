using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace ILInjection
{
    public delegate void ILGenAction(MethodReference method, MethodBody body, Collection<Instruction> il);
}
