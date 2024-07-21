grammar Expression;

// Parser rules

root       : expression EOF ;
expression : term ((PLUS | MINUS) term)? ;
term       : exponent ((MULTIPLICATION | DIVISION | MODULO) exponent)? ;
exponent   : factor (POWER exponent)? ;
factor     : NUMBER | variable | unary | function | OPEN_PARENTHESIS expression CLOSE_PARENTHESIS ;
function   : IDENTIFIER OPEN_PARENTHESIS (expression (COMMA expression)*)? CLOSE_PARENTHESIS ;
unary      : PLUS factor | MINUS factor ;
variable   : IDENTIFIER ;

// Lexer rules

PLUS              : '+' ;
MINUS             : '-' ; 
MULTIPLICATION    : '*' ;
DIVISION          : '/' ;
MODULO            : '%' ;
POWER             : '^' ;
OPEN_PARENTHESIS  : '(' ;
CLOSE_PARENTHESIS : ')' ;
COMMA             : ',' ;
IDENTIFIER        : [a-zA-Z_][a-zA-Z0-9_]* ;
NUMBER            : [0-9]+('.'[0-9]+)? ;
NEWLINE           : [\r\n]+ -> skip ;
WHITESPACE        : [ \t]+ -> skip ;