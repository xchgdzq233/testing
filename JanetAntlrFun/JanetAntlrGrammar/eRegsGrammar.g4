grammar eRegsGrammar;

/*
 * Parser Rules
 */

r : classStructure+ ;

classStructure : Names parentClassName? '{' propertyNames* '}' ;

parentClassName : ':' Names ;

propertyNames : '[' Names ']' '(' dataType ')';

dataType : STRING 
	| OBJECT
	| GUID
	| DATE
	| BINARY
	| LONG
	| DOUBLE
	| BOOLEAN ;

/*
 * Lexer Rules
 */
STRING : 'STRING' ;
OBJECT : 'OBJECT' ;
GUID : 'GUID' ;
DATE : 'DATE' ;
BINARY : 'BINARY' ;
LONG : 'LONG' ;
DOUBLE : 'DOUBLE' ;
BOOLEAN : 'BOOLEAN' ;

Names : [A-Za-z0-9_]+ ;

WS : [ \r\t\n]+ -> skip ;
