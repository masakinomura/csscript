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
	| variable							# varExp
	| NAME								# idAtomExp
	| INT								# intAtomExp
	| LONG								# longAtomExp
	| expression OP_ASSIGN expression	# assignmentExp
	| NEW vartypes parameters			# newExp;

variable: VAR NAME | NAME;

parameters: '(' ')' | '(' expression (',' expression)* ')';

vartypes: vartype ('.' vartype)*;
vartype: NAME template_type*;

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
LONG: INT 'L';

WHITESPACE: (' ' | '\t')+ -> skip;

EOL: ';' ('\r'? '\n' | '\r')*;

// handle characters which failed to match any other token ErrorCharacter : . ;