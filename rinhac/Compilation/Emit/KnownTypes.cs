using Mono.Cecil;
using Rinha.Core;

namespace Rinha.Compilation.Emit;

public enum KnownType
{
    SystemObject,
    RinhaObject,
    RinhaObjectArr,
    RinhaInnerClosure,

    SystemVoid,
    SystemConsole,

    RinhaInt,
    RinhaBool,
    RinhaStr,

    RinhaTuple,
    RinhaClosure,
    RinhaClosureParams,

    RinhaBuiltIns
}

public class KnownTypes
{
    private Dictionary<KnownType, TypeReference> _types;

    public KnownTypes(ModuleDefinition module)
    {
        _types = new Dictionary<KnownType, TypeReference>()
        {
            { KnownType.SystemObject, module.ImportReference(typeof(object)) },
            { KnownType.RinhaObject, module.ImportReference(typeof(RinhaObject)) },
            { KnownType.RinhaObjectArr, module.ImportReference(typeof(RinhaObject[])) },
            { KnownType.RinhaInnerClosure, module.ImportReference(typeof(RinhaRunnableClosure)) },

            { KnownType.SystemVoid, module.ImportReference(typeof(void)) },
            { KnownType.SystemConsole, module.ImportReference(typeof(Console)) },

            { KnownType.RinhaInt, module.ImportReference(typeof(RinhaInt)) },
            { KnownType.RinhaBool, module.ImportReference(typeof(RinhaBool)) },
            { KnownType.RinhaStr, module.ImportReference(typeof(RinhaStr)) },

            { KnownType.RinhaTuple, module.ImportReference(typeof(RinhaTuple)) },
            { KnownType.RinhaClosure, module.ImportReference(typeof(RinhaClosure)) },
            { KnownType.RinhaClosureParams, module.ImportReference(typeof(RinhaClosureParams)) },

            { KnownType.RinhaBuiltIns, module.ImportReference(typeof(BuiltInMethods)) }
        };

    }

    public TypeReference GetRef(KnownType known)
    {
        return _types[known];
    }
}
