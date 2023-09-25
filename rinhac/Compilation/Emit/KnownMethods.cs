using Mono.Cecil;
using Rinha.Core;

namespace Rinha.Compilation.Emit;

public enum KnownMethod
{
    SystemConsoleWriteLine,

    RinhaIntCtor,
    RinhaBoolCtor,
    RinhaStrCtor,

    RinhaPrint,
}

public class KnownMethods
{
    private Dictionary<KnownMethod, MethodReference?> _methods;

    public KnownMethods(ModuleDefinition module)
    {
        _methods = new Dictionary<KnownMethod, MethodReference?>()
        {
            { 
                KnownMethod.SystemConsoleWriteLine,
                module.ImportReference(
                    typeof(Console).GetMethod("WriteLine", new [] { typeof(object) })
                )
            },
            {
                KnownMethod.RinhaIntCtor,
                module.ImportReference(
                    typeof(RinhaInt).GetConstructors()[0]
                )
            },
            {
                KnownMethod.RinhaBoolCtor,
                module.ImportReference(
                    typeof(RinhaBool).GetConstructors()[0]
                )
            },
            {
                KnownMethod.RinhaStrCtor,
                module.ImportReference(
                    typeof(RinhaStr).GetConstructors()[0]
                )
            },
            {
                KnownMethod.RinhaPrint,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod("Print", new [] { typeof(RinhaObject) })
                )
            },
        };

    }

    public MethodReference? GetRef(KnownMethod known)
    {
        return _methods[known];
    }
}
