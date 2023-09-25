using Mono.Cecil;
using Rinha.Core;

namespace Rinha.Compilation.Emit;

public enum KnownMethod
{
    SystemConsoleWriteLine,

    RinhaIntCtor,
    RinhaBoolCtor,
    RinhaStrCtor,

    // Arithmetic
    RinhaAdd,
    RinhaSub,
    RinhaMul,
    RinhaDiv,
    RinhaRem,

    // Logical
    RinhaEq,
    RinhaNeq,
    RinhaLt,
    RinhaGt,
    RinhaLOrEq,
    RinhaGOrEq,
    RinhaAnd,
    RinhaOr,

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
            {
                KnownMethod.RinhaAdd,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "Add",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaSub,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "Sub",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaMul,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "Mul",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaDiv,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "Div",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaRem,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "Rem",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaEq,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "Eq",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaNeq,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "NotEq",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaLt,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "LessThan",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaGt,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "GreaterThan",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaLOrEq,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "LessOrEq",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaGOrEq,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "GreaterOrEq",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaAnd,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "And",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
            {
                KnownMethod.RinhaOr,
                module.ImportReference(
                    typeof(BuiltInMethods).GetMethod(
                        "Or",
                        new [] { typeof(RinhaObject), typeof(RinhaObject) })
                )
            },
        };

    }

    public MethodReference? GetRef(KnownMethod known)
    {
        return _methods[known];
    }
}
