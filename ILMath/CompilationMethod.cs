namespace ILMath;

/// <summary>
/// The compilation method.
/// </summary>
public enum CompilationMethod
{
    /// <summary>
    /// Compiles the expression by generating IL code.<br/>
    /// <b>Note:</b> Does not work in AOT environments.
    /// </summary>
    IntermediateLanguage,
    
    /// <summary>
    /// Compiles the expression using expression trees.
    /// </summary>
    ExpressionTree,
    
    /// <summary>
    /// Compiles the expression using functional methods.
    /// </summary>
    Functional,
}