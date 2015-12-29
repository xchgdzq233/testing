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
	| BOOLEAN ;

/*
 * Lexer Rules
 */
STRING : 'STRING' ;
OBJECT : 'OBJECT' ;
GUID : 'GUID' ;
DATE : 'DATE' ;
BINARY : 'BINARY' ;
BOOLEAN : 'BOOLEAN' ;

Names : [A-Za-z_]+ ;

WS : [ \r\t\n]+ -> skip ;
