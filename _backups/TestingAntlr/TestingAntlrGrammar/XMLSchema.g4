grammar XMLSchema;

/*
 * Parser Rules
 */

 schemaHeader : '<?xml version="1.0" encoding="utf-8"?><xs:schema xmlns:ditaarch="http://dita.oasis-open.org/architecture/2005/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema">'node+'</xs:schema>' ;
 
 node : '<'EndNode?'xs:'NodeType nodeProperty* EndNode?'>' ;

 nodeProperty : PropertyName'="'propertyValue'"' ;

 propertyValue : SingleINT
	| 'unbounded'
	| StringID
	| 'xs:string' 
	| UseType ;

/*
 * Lexer Rules
 */

 NodeType : 'element'
	| 'complexType'
	| 'sequence'
	| 'attribute' ;
 PropertyName : 'name'
	| 'type'
	| 'minOccurs'
	| 'maxOccurs'
	| 'use'
	| 'mixed' ;
 SingleINT : [0-9] ;
 StringID : [A-Za-z]+ ;
 UseType : 'required'
	| 'optional' ;
 EndNode : '/' ;
 WS : [ \t\n\r]+ -> skip ;