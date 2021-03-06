grammar CSScript;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}

/*
 * Parser Rules
 */

code: line*;
line: (expression+ ';'); // | block;

block: CURLY_BRACE_START line* CURLY_BRACE_END;

expression:
	NEW type_elements (parameters | array_index) initializer?	# newExp
	| (selector '.')* NAME generic_parameters? parameters		# funcExp
	| (VAR | type) NAME											# varDeclExp
	| USING namespace											# usingNamespaceExp
	| expression DOT expression									# dotExp
	| expression array_index									# arrayIndexExp
	| '(' expression ')'										# parenthesisaExp
	| expression OP_ASSIGN expression							# assignmentExp
	| expression OP_PLUS expression								# plusExp
	| selector ('.' selector)*									# selectorExp
	| LONG														# longAtomExp
	| ULONG														# ulongAtomExp
	| INT														# intAtomExp
	| UINT														# uintAtomExp
	| DECIMAL													# doubleAtomExp
	| DOUBLE													# doubleAtomExp
	| FLOAT														# floatAtomExp
	| STRING													# stringAtomExp;

parameters: '(' ')' | '(' expression (',' expression)* ')';

type: type_elements arraytype?;
type_elements: type_element ('.' type_element)*;
type_element: NAME generic_parameters?;
arraytype: '[' ']';

selector: NAME (generic_parameters)?;

generic_parameters: '<' type (',' type)* '>';

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
fragment LETTER: (LOWERCASE | UPPERCASE | '_');
fragment WORD: (LOWERCASE | UPPERCASE | '_')+;
fragment ESCAPED_QUOTE: '\\"';
fragment NUMBER: [0-9];
fragment FNUMBER: [0-9]+ ('.' [0-9]+)?;
fragment AT: '@';

EOL: ';';
VAR: 'var';
NEW: 'new';
USING: 'using';
OP_ASSIGN: '=';
OP_MUL: '*';
OP_PLUS: '+';
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

NAME: AT? LETTER (LETTER | NUMBER)*;
INT: NUMBER+;
UINT: INT ('u' | 'U');
LONG: INT ('l' | 'L');
ULONG: INT ('ul' | 'UL' | 'uL' | 'Ul');

FLOAT: FNUMBER 'f';
DOUBLE: FNUMBER 'd'?;
DECIMAL: FNUMBER 'm';
STRING: '"' ( ESCAPED_QUOTE | ~('\n' | '\r'))*? '"';

WHITESPACE: (' ' | '\t')+ -> skip;
NEWLINE: ('\r'? '\n' | '\r')+ -> skip;
