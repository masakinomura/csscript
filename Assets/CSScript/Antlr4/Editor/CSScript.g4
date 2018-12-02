grammar CSScript;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}

/*
 * Parser Rules
 */

/* 
 chat : line line EOF ;
 line : name SAYS opinion NEWLINE;
 name : WORD ;
 opinion : TEXT ;
 */

code: line*;
line: expression+ EOL;

expression:
	'(' expression ')'					# parenthesisExp
	| NEW vartypes parameters			# newExp
	| expression OP_ASSIGN expression	# assignmentExp
	| variable							# varExp
	| NAME								# idAtomExp
	| LONG								# longAtomExp
	| ULONG								# ulongAtomExp
	| INT								# intAtomExp
	| UINT								# uintAtomExp
	| DECIMAL							# doubleAtomExp
	| DOUBLE							# doubleAtomExp
	| FLOAT								# floatAtomExp
	| STRING							# stringAtomExp;

variable: VAR NAME | NAME;

parameters: '(' ')' | '(' expression (',' expression)* ')';

vartypes: vartype ('.' vartype)*;
vartype: NAME template_type?;

template_type: '<' vartypes (',' vartypes)* '>';

/*
 * Lexer Rules
 */

/*
 fragment A : ('A'|'a') ;
 fragment S : ('S'|'s') ;
 fragment Y : ('Y'|'y') ;
 
 
 SAYS : S A Y
 S
 ;
 WORD : (LOWERCASE | UPPERCASE)+ ;
 TEXT : '"' .*? '"' ;
 
 
 */
fragment LOWERCASE: [a-z];
fragment UPPERCASE: [A-Z];
fragment WORD: (LOWERCASE | UPPERCASE | '_')+;
fragment ESCAPED_QUOTE: '\\"';
fragment FNUMBER: [0-9]+ ('.' [0-9]+)?;

OP_MUL: '*';
VAR: 'v' 'a' 'r';
NEW: 'n' 'e' 'w';
OP_ASSIGN: '=';
LESS_THAN: '<';
GREATER_THAN: '>';
PARENTHESIS_START: '(';
PARENTHESIS_END: ')';
COMMA: ',';
DOT: '.';

NAME: WORD [0-9]*;
INT: [0-9]+;
UINT: INT ('u' | 'U');
LONG: INT ('l' | 'L');
ULONG: INT ('ul' | 'UL' | 'uL' | 'Ul');

FLOAT: FNUMBER 'f';
DOUBLE: FNUMBER 'd'?;
DECIMAL: FNUMBER 'm';
STRING: '"' ( ESCAPED_QUOTE | ~('\n' | '\r'))*? '"';

EOL: ';' ('\r'? '\n' | '\r')*;

WHITESPACE: (' ' | '\t')+ -> skip;