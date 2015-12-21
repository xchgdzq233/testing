grammar FileNetSQLGrammar;

/*
 * Parser Rules
 */

 getUnSingedTempCodifiedSubjectMattersDef : 'GetUnSingedTempCodifiedSubjectMatters' '(' CaseID ')' ';'

/*
 * Lexer Rules
 */
 CaseID : [A-Za-z0-9]+ ;


WS	:	[ \t\r\n]+ -> skip ;
