grammar CSScript;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}
 
/*
 * Parser Rules
 */

/* 
chat                : line line EOF ;
line                : name SAYS opinion NEWLINE;
name                : WORD ;
opinion             : TEXT ;
*/

code				: line* ;
line				: expression+ EOL;
expression			: '(' expression ')' 					#parenthesisExp
					| variable								#varExp
					| NAME									#idAtomExp
					| INT									#intAtomExp
					| variable OP_ASSIGN expression			#assignmentExp
					;

variable			: VAR NAME | NAME;

/*
 * Lexer Rules
 */
 
/*
fragment A          : ('A'|'a') ;
fragment S          : ('S'|'s') ;
fragment Y          : ('Y'|'y') ;

 
SAYS                : S A Y S ;
WORD                : (LOWERCASE | UPPERCASE)+ ;
TEXT                : '"' .*? '"' ;


*/

 
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment WORD       : (LOWERCASE | UPPERCASE | '_' )+ ;

OP_MUL				: '*' ;
VAR					: 'v' 'a' 'r' ;
OP_ASSIGN			: '=' ;


NAME 				: WORD [0-9]* ;
INT					: [0-9]+ ;
LONG				: INT 'L' ;




WHITESPACE          : (' '|'\t')+ -> skip ;


EOL					: ';' ('\r'? '\n' | '\r')* ;

