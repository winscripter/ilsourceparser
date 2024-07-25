using ILSourceParser.Syntax.Instructions;
using ILSourceParser.Syntax.Instructions.OpCodes;

namespace ILSourceParser.Utilities;

/// <summary>
/// Utilities for <see cref="InstructionSyntax"/>.
/// </summary>
public static class InstructionUtilities
{
    /// <summary>
    /// Returns the name of the instruction based on the syntax node. For example,
    /// if the syntax node is an instance of <see cref="ParameterlessOpCodeSyntax"/>,
    /// the name of the parameterless instruction is returned, or, for example, if
    /// the syntax node is an instance of <see cref="UnboxAnyOpCodeSyntax"/>, "unbox.any"
    /// is returned.
    /// </summary>
    /// <param name="instruction">The input instruction to get name of.</param>
    /// <returns>The name of the instruction as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when the input instruction is not valid.</exception>
    public static string GetInstructionName(InstructionSyntax instruction)
    {
        return instruction switch
        {
            ParameterlessOpCodeSyntax parameterlessOpCode => parameterlessOpCode.Name,
            ArgumentLoadOpCodeSyntax argumentLoad => argumentLoad.Name,
            ArgumentStoreOpCodeSyntax argumentStore => argumentStore.Name,
            BranchOpCodeSyntax branchOpCode => branchOpCode.Name,
            ComparisonOpCodeSyntax comparisonOpCode => comparisonOpCode.Name,
            ConversionOpCodeSyntax conversionOpCode => conversionOpCode.Name,
            LoadElementOpCodeSyntax loadElement => loadElement.Name,
            LoadIndirectOpCodeSyntax loadIndirect => loadIndirect.Name,
            LoadLocalOpCodeSyntax loadLocal => loadLocal.Name,
            PushNumberToStackOpCodeSyntax pushNumberToStack => pushNumberToStack.Name,
            StoreElementOpCodeSyntax storeElement => storeElement.Name,
            StoreIndirectOpCodeSyntax storeIndirect => storeIndirect.Name,
            StoreLocalOpCodeSyntax storeLocal => storeLocal.Name,
            BoxOpCodeSyntax => "box",
            CalliOpCodeSyntax => "calli",
            CallOpCodeSyntax => "call",
            CallvirtOpCodeSyntax => "callvirt",
            CastclassOpCodeSyntax => "castclass",
            CpobjOpCodeSyntax => "cpobj",
            InitobjOpCodeSyntax => "initobj",
            IsinstOpCodeSyntax => "isinst",
            JmpOpCodeSyntax => "jmp",
            LdfldaOpCodeSyntax => "ldflda",
            LdfldOpCodeSyntax => "ldfld",
            LdftnOpCodeSyntax => "ldftn",
            LdsfldaOpCodeSyntax => "ldsflda",
            LdsfldOpCodeSyntax => "ldsfld",
            LdstrOpCodeSyntax => "ldstr",
            LdvirtftnOpCodeSyntax => "ldvirtftn",
            LeaveOpCodeSyntax => "leave",
            MkrefanyOpCodeSyntax => "mkrefany",
            NewarrOpCodeSyntax => "newarr",
            NewobjOpCodeSyntax => "newobj",
            RefanyvalOpCodeSyntax => "refanyval",
            SizeofOpCodeSyntax => "sizeof",
            SwitchOpCodeSyntax => "switch",
            UnboxAnyOpCodeSyntax => "unbox.any",
            UnboxOpCodeSyntax => "unbox",
            _ => throw new ArgumentException("Invalid opcode syntax node", nameof(instruction))
        };
    }
}
