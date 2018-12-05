grammar CSScript;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}

/*
 * Parser Rules
 */

code: line*;
line: (expression+ EOL) | block;

block: CURLY_BRACE_START line* CURLY_BRACE_END;

expression:
	'(' expression ')'										# parenthesisExp
	| NEW vartypes (parameters | array_index) initializer?	# newExp
	| (vartypes '.')? NAME generic_parameters? parameters	# funcExp
	| expression array_index								# arrayIndexExp
	| expression OP_ASSIGN expression						# assignmentExp
	| (VAR | vartypes) NAME									# varDeclExp
	| USING namespace										# usingNamespaceExp
	| NAME													# idAtomExp
	| LONG													# longAtomExp
	| ULONG													# ulongAtomExp
	| INT													# intAtomExp
	| UINT													# uintAtomExp
	| DECIMAL												# doubleAtomExp
	| DOUBLE												# doubleAtomExp
	| FLOAT													# floatAtomExp
	| STRING												# stringAtomExp;


parameters: '(' ')' | '(' expression (',' expression)* ')';

vartypes: vartype ('.' vartype)*;
vartype: NAME generic_parameters?;

generic_parameters: '<' vartypes (',' vartypes)* '>';
namespace: NAME (. NAME)*;
array_index: '[' expression? ']';

initializer:
	class_initializer
	| array_initializer
	| dictionary_initializer;

class_initializer:
	'{' class_initializer_element (',' class_initializer_element)* ','? '}';
class_initializer_element: NAME '=' expression;

array_initializer: '{' expression (',' expression)* '}';

dictionary_initializer:
	'{' dictionary_initializer_element (
		',' dictionary_initializer_element
	)* ','? '}';

dictionary_initializer_element:
	'{' expression ',' expression '}';

/*
 * Lexer Rules
 */
fragment LOWERCASE: [a-z];
fragment UPPERCASE: [A-Z];
fragment WORD: (LOWERCASE | UPPERCASE | '_')+;
fragment ESCAPED_QUOTE: '\\"';
fragment FNUMBER: [0-9]+ ('.' [0-9]+)?;

OP_MUL: '*';
VAR: 'var';
NEW: 'new';
USING: 'using';
OP_ASSIGN: '=';
LESS_THAN: '<';
GREATER_THAN: '>';
PARENTHESIS_START: '(';
PARENTHESIS_END: ')';
RECTANGLE_BRACE_START: '[';
RECTANGLE_BRACE_END: ']';
CURLY_BRACE_START: '{';
CURLY_BRACE_END: '}';
COMMA: ',';
DOT: '.';

NAME: WORD [0-9]* WORD*;
INT: [0-9]+;
UINT: INT ('u' | 'U');
LONG: INT ('l' | 'L');
ULONG: INT ('ul' | 'UL' | 'uL' | 'Ul');

FLOAT: FNUMBER 'f';
DOUBLE: FNUMBER 'd'?;
DECIMAL: FNUMBER 'm';
STRING: '"' ( ESCAPED_QUOTE | ~('\n' | '\r'))*? '"';

EOL: ';';

WHITESPACE: (' ' | '\t')+ -> skip;
NEWLINE: ('\r'? '\n' | '\r')+ -> skip;
