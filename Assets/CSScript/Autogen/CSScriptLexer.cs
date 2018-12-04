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
using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5")]
[System.CLSCompliant(false)]
public partial class CSScriptLexer : Lexer {
	public const int
		OP_MUL=1, VAR=2, NEW=3, USING=4, OP_ASSIGN=5, LESS_THAN=6, GREATER_THAN=7, 
		PARENTHESIS_START=8, PARENTHESIS_END=9, RECTANGLE_BRACE_START=10, RECTANGLE_BRACE_END=11, 
		CURLY_BRACE_START=12, CURLY_BRACE_END=13, COMMA=14, DOT=15, NAME=16, INT=17, 
		UINT=18, LONG=19, ULONG=20, FLOAT=21, DOUBLE=22, DECIMAL=23, STRING=24, 
		EOL=25, WHITESPACE=26, NEWLINE=27;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"LOWERCASE", "UPPERCASE", "WORD", "ESCAPED_QUOTE", "FNUMBER", "OP_MUL", 
		"VAR", "NEW", "USING", "OP_ASSIGN", "LESS_THAN", "GREATER_THAN", "PARENTHESIS_START", 
		"PARENTHESIS_END", "RECTANGLE_BRACE_START", "RECTANGLE_BRACE_END", "CURLY_BRACE_START", 
		"CURLY_BRACE_END", "COMMA", "DOT", "NAME", "INT", "UINT", "LONG", "ULONG", 
		"FLOAT", "DOUBLE", "DECIMAL", "STRING", "EOL", "WHITESPACE", "NEWLINE"
	};


	public CSScriptLexer(ICharStream input)
		: base(input)
	{
		Interpreter = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'*'", "'var'", "'new'", "'using'", "'='", "'<'", "'>'", "'('", 
		"')'", "'['", "']'", "'{'", "'}'", "','", "'.'", null, null, null, null, 
		null, null, null, null, null, "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "OP_MUL", "VAR", "NEW", "USING", "OP_ASSIGN", "LESS_THAN", "GREATER_THAN", 
		"PARENTHESIS_START", "PARENTHESIS_END", "RECTANGLE_BRACE_START", "RECTANGLE_BRACE_END", 
		"CURLY_BRACE_START", "CURLY_BRACE_END", "COMMA", "DOT", "NAME", "INT", 
		"UINT", "LONG", "ULONG", "FLOAT", "DOUBLE", "DECIMAL", "STRING", "EOL", 
		"WHITESPACE", "NEWLINE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "CSScript.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2\x1D\xCF\b\x1\x4"+
		"\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b"+
		"\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4"+
		"\x10\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15"+
		"\t\x15\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A"+
		"\x4\x1B\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 "+
		"\t \x4!\t!\x3\x2\x3\x2\x3\x3\x3\x3\x3\x4\x3\x4\x3\x4\x6\x4K\n\x4\r\x4"+
		"\xE\x4L\x3\x5\x3\x5\x3\x5\x3\x6\x6\x6S\n\x6\r\x6\xE\x6T\x3\x6\x3\x6\x6"+
		"\x6Y\n\x6\r\x6\xE\x6Z\x5\x6]\n\x6\x3\a\x3\a\x3\b\x3\b\x3\b\x3\b\x3\t\x3"+
		"\t\x3\t\x3\t\x3\n\x3\n\x3\n\x3\n\x3\n\x3\n\x3\v\x3\v\x3\f\x3\f\x3\r\x3"+
		"\r\x3\xE\x3\xE\x3\xF\x3\xF\x3\x10\x3\x10\x3\x11\x3\x11\x3\x12\x3\x12\x3"+
		"\x13\x3\x13\x3\x14\x3\x14\x3\x15\x3\x15\x3\x16\x3\x16\a\x16\x87\n\x16"+
		"\f\x16\xE\x16\x8A\v\x16\x3\x16\a\x16\x8D\n\x16\f\x16\xE\x16\x90\v\x16"+
		"\x3\x17\x6\x17\x93\n\x17\r\x17\xE\x17\x94\x3\x18\x3\x18\x3\x18\x3\x19"+
		"\x3\x19\x3\x19\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A"+
		"\x3\x1A\x5\x1A\xA6\n\x1A\x3\x1B\x3\x1B\x3\x1B\x3\x1C\x3\x1C\x5\x1C\xAD"+
		"\n\x1C\x3\x1D\x3\x1D\x3\x1D\x3\x1E\x3\x1E\x3\x1E\a\x1E\xB5\n\x1E\f\x1E"+
		"\xE\x1E\xB8\v\x1E\x3\x1E\x3\x1E\x3\x1F\x3\x1F\x3 \x6 \xBF\n \r \xE \xC0"+
		"\x3 \x3 \x3!\x5!\xC6\n!\x3!\x3!\x6!\xCA\n!\r!\xE!\xCB\x3!\x3!\x3\xB6\x2"+
		"\"\x3\x2\x5\x2\a\x2\t\x2\v\x2\r\x3\xF\x4\x11\x5\x13\x6\x15\a\x17\b\x19"+
		"\t\x1B\n\x1D\v\x1F\f!\r#\xE%\xF\'\x10)\x11+\x12-\x13/\x14\x31\x15\x33"+
		"\x16\x35\x17\x37\x18\x39\x19;\x1A=\x1B?\x1C\x41\x1D\x3\x2\t\x3\x2\x63"+
		"|\x3\x2\x43\\\x3\x2\x32;\x4\x2WWww\x4\x2NNnn\x4\x2\f\f\xF\xF\x4\x2\v\v"+
		"\"\"\xDC\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x13"+
		"\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2\x2\x19\x3\x2\x2\x2"+
		"\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2\x2\x2\x1F\x3\x2\x2\x2\x2!\x3\x2\x2"+
		"\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2"+
		"+\x3\x2\x2\x2\x2-\x3\x2\x2\x2\x2/\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x2\x33"+
		"\x3\x2\x2\x2\x2\x35\x3\x2\x2\x2\x2\x37\x3\x2\x2\x2\x2\x39\x3\x2\x2\x2"+
		"\x2;\x3\x2\x2\x2\x2=\x3\x2\x2\x2\x2?\x3\x2\x2\x2\x2\x41\x3\x2\x2\x2\x3"+
		"\x43\x3\x2\x2\x2\x5\x45\x3\x2\x2\x2\aJ\x3\x2\x2\x2\tN\x3\x2\x2\x2\vR\x3"+
		"\x2\x2\x2\r^\x3\x2\x2\x2\xF`\x3\x2\x2\x2\x11\x64\x3\x2\x2\x2\x13h\x3\x2"+
		"\x2\x2\x15n\x3\x2\x2\x2\x17p\x3\x2\x2\x2\x19r\x3\x2\x2\x2\x1Bt\x3\x2\x2"+
		"\x2\x1Dv\x3\x2\x2\x2\x1Fx\x3\x2\x2\x2!z\x3\x2\x2\x2#|\x3\x2\x2\x2%~\x3"+
		"\x2\x2\x2\'\x80\x3\x2\x2\x2)\x82\x3\x2\x2\x2+\x84\x3\x2\x2\x2-\x92\x3"+
		"\x2\x2\x2/\x96\x3\x2\x2\x2\x31\x99\x3\x2\x2\x2\x33\x9C\x3\x2\x2\x2\x35"+
		"\xA7\x3\x2\x2\x2\x37\xAA\x3\x2\x2\x2\x39\xAE\x3\x2\x2\x2;\xB1\x3\x2\x2"+
		"\x2=\xBB\x3\x2\x2\x2?\xBE\x3\x2\x2\x2\x41\xC9\x3\x2\x2\x2\x43\x44\t\x2"+
		"\x2\x2\x44\x4\x3\x2\x2\x2\x45\x46\t\x3\x2\x2\x46\x6\x3\x2\x2\x2GK\x5\x3"+
		"\x2\x2HK\x5\x5\x3\x2IK\a\x61\x2\x2JG\x3\x2\x2\x2JH\x3\x2\x2\x2JI\x3\x2"+
		"\x2\x2KL\x3\x2\x2\x2LJ\x3\x2\x2\x2LM\x3\x2\x2\x2M\b\x3\x2\x2\x2NO\a^\x2"+
		"\x2OP\a$\x2\x2P\n\x3\x2\x2\x2QS\t\x4\x2\x2RQ\x3\x2\x2\x2ST\x3\x2\x2\x2"+
		"TR\x3\x2\x2\x2TU\x3\x2\x2\x2U\\\x3\x2\x2\x2VX\a\x30\x2\x2WY\t\x4\x2\x2"+
		"XW\x3\x2\x2\x2YZ\x3\x2\x2\x2ZX\x3\x2\x2\x2Z[\x3\x2\x2\x2[]\x3\x2\x2\x2"+
		"\\V\x3\x2\x2\x2\\]\x3\x2\x2\x2]\f\x3\x2\x2\x2^_\a,\x2\x2_\xE\x3\x2\x2"+
		"\x2`\x61\ax\x2\x2\x61\x62\a\x63\x2\x2\x62\x63\at\x2\x2\x63\x10\x3\x2\x2"+
		"\x2\x64\x65\ap\x2\x2\x65\x66\ag\x2\x2\x66g\ay\x2\x2g\x12\x3\x2\x2\x2h"+
		"i\aw\x2\x2ij\au\x2\x2jk\ak\x2\x2kl\ap\x2\x2lm\ai\x2\x2m\x14\x3\x2\x2\x2"+
		"no\a?\x2\x2o\x16\x3\x2\x2\x2pq\a>\x2\x2q\x18\x3\x2\x2\x2rs\a@\x2\x2s\x1A"+
		"\x3\x2\x2\x2tu\a*\x2\x2u\x1C\x3\x2\x2\x2vw\a+\x2\x2w\x1E\x3\x2\x2\x2x"+
		"y\a]\x2\x2y \x3\x2\x2\x2z{\a_\x2\x2{\"\x3\x2\x2\x2|}\a}\x2\x2}$\x3\x2"+
		"\x2\x2~\x7F\a\x7F\x2\x2\x7F&\x3\x2\x2\x2\x80\x81\a.\x2\x2\x81(\x3\x2\x2"+
		"\x2\x82\x83\a\x30\x2\x2\x83*\x3\x2\x2\x2\x84\x88\x5\a\x4\x2\x85\x87\t"+
		"\x4\x2\x2\x86\x85\x3\x2\x2\x2\x87\x8A\x3\x2\x2\x2\x88\x86\x3\x2\x2\x2"+
		"\x88\x89\x3\x2\x2\x2\x89\x8E\x3\x2\x2\x2\x8A\x88\x3\x2\x2\x2\x8B\x8D\x5"+
		"\a\x4\x2\x8C\x8B\x3\x2\x2\x2\x8D\x90\x3\x2\x2\x2\x8E\x8C\x3\x2\x2\x2\x8E"+
		"\x8F\x3\x2\x2\x2\x8F,\x3\x2\x2\x2\x90\x8E\x3\x2\x2\x2\x91\x93\t\x4\x2"+
		"\x2\x92\x91\x3\x2\x2\x2\x93\x94\x3\x2\x2\x2\x94\x92\x3\x2\x2\x2\x94\x95"+
		"\x3\x2\x2\x2\x95.\x3\x2\x2\x2\x96\x97\x5-\x17\x2\x97\x98\t\x5\x2\x2\x98"+
		"\x30\x3\x2\x2\x2\x99\x9A\x5-\x17\x2\x9A\x9B\t\x6\x2\x2\x9B\x32\x3\x2\x2"+
		"\x2\x9C\xA5\x5-\x17\x2\x9D\x9E\aw\x2\x2\x9E\xA6\an\x2\x2\x9F\xA0\aW\x2"+
		"\x2\xA0\xA6\aN\x2\x2\xA1\xA2\aw\x2\x2\xA2\xA6\aN\x2\x2\xA3\xA4\aW\x2\x2"+
		"\xA4\xA6\an\x2\x2\xA5\x9D\x3\x2\x2\x2\xA5\x9F\x3\x2\x2\x2\xA5\xA1\x3\x2"+
		"\x2\x2\xA5\xA3\x3\x2\x2\x2\xA6\x34\x3\x2\x2\x2\xA7\xA8\x5\v\x6\x2\xA8"+
		"\xA9\ah\x2\x2\xA9\x36\x3\x2\x2\x2\xAA\xAC\x5\v\x6\x2\xAB\xAD\a\x66\x2"+
		"\x2\xAC\xAB\x3\x2\x2\x2\xAC\xAD\x3\x2\x2\x2\xAD\x38\x3\x2\x2\x2\xAE\xAF"+
		"\x5\v\x6\x2\xAF\xB0\ao\x2\x2\xB0:\x3\x2\x2\x2\xB1\xB6\a$\x2\x2\xB2\xB5"+
		"\x5\t\x5\x2\xB3\xB5\n\a\x2\x2\xB4\xB2\x3\x2\x2\x2\xB4\xB3\x3\x2\x2\x2"+
		"\xB5\xB8\x3\x2\x2\x2\xB6\xB7\x3\x2\x2\x2\xB6\xB4\x3\x2\x2\x2\xB7\xB9\x3"+
		"\x2\x2\x2\xB8\xB6\x3\x2\x2\x2\xB9\xBA\a$\x2\x2\xBA<\x3\x2\x2\x2\xBB\xBC"+
		"\a=\x2\x2\xBC>\x3\x2\x2\x2\xBD\xBF\t\b\x2\x2\xBE\xBD\x3\x2\x2\x2\xBF\xC0"+
		"\x3\x2\x2\x2\xC0\xBE\x3\x2\x2\x2\xC0\xC1\x3\x2\x2\x2\xC1\xC2\x3\x2\x2"+
		"\x2\xC2\xC3\b \x2\x2\xC3@\x3\x2\x2\x2\xC4\xC6\a\xF\x2\x2\xC5\xC4\x3\x2"+
		"\x2\x2\xC5\xC6\x3\x2\x2\x2\xC6\xC7\x3\x2\x2\x2\xC7\xCA\a\f\x2\x2\xC8\xCA"+
		"\a\xF\x2\x2\xC9\xC5\x3\x2\x2\x2\xC9\xC8\x3\x2\x2\x2\xCA\xCB\x3\x2\x2\x2"+
		"\xCB\xC9\x3\x2\x2\x2\xCB\xCC\x3\x2\x2\x2\xCC\xCD\x3\x2\x2\x2\xCD\xCE\b"+
		"!\x2\x2\xCE\x42\x3\x2\x2\x2\x13\x2JLTZ\\\x88\x8E\x94\xA5\xAC\xB4\xB6\xC0"+
		"\xC5\xC9\xCB\x3\b\x2\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
