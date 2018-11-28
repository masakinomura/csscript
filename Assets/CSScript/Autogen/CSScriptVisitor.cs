//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from CSScript.g4 by ANTLR 4.5

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

#pragma warning disable 3021
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="CSScriptParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5")]
[System.CLSCompliant(false)]
public interface ICSScriptVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="CSScriptParser.code"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCode([NotNull] CSScriptParser.CodeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CSScriptParser.line"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLine([NotNull] CSScriptParser.LineContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisExp</c>
	/// labeled alternative in <see cref="CSScriptParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesisExp([NotNull] CSScriptParser.ParenthesisExpContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>idAtomExp</c>
	/// labeled alternative in <see cref="CSScriptParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdAtomExp([NotNull] CSScriptParser.IdAtomExpContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>intAtomExp</c>
	/// labeled alternative in <see cref="CSScriptParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntAtomExp([NotNull] CSScriptParser.IntAtomExpContext context);
}
