using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintLanguage
{
    [Language("Mint")]
    public class MintLanguage : Grammar
    {
        public MintLanguage()
        {
            #region Declare Terminals Here
            var blockComment = new CommentTerminal("block-comment", "/*", "*/");
            var lineComment = new CommentTerminal("line-comment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            NonGrammarTerminals.Add(blockComment);
            NonGrammarTerminals.Add(lineComment);

            var number = new NumberLiteral("number");
            var identifier = new IdentifierTerminal("identifier");
            var backString = new StringLiteral("back-string", "`");
            var doubleString = new StringLiteral("double-string", "\"");
            var refMark = ToTerm("&");
            var lPar = ToTerm("(");
            var rPar = ToTerm(")");
            var lBrace = ToTerm("{");
            var rBrace = ToTerm("}");
            var lSqBracket = ToTerm("[");
            var rSqBracket = ToTerm("]");
            var semi = ToTerm(";");
            var comma = ToTerm(",");
            var dot = ToTerm(".");
            var colon = ToTerm(":");
            var @break = ToTerm("break");
            var @continue = ToTerm("continue");
            var @return = ToTerm("return");
            #endregion

            #region Declare NonTerminals Here
            var literal = new NonTerminal("literal", number | backString | doubleString | "true" | "false");
            var breakStatement = new NonTerminal("break", @break + semi);
            var continueStatement = new NonTerminal("continue", @continue + semi);
            var returnStatement = new NonTerminal("return");
            MarkTransient(literal, breakStatement, continueStatement);

            var typeName = new NonTerminal("typeName", ToTerm("void") | "bool" | "character" | "item" | "int" | "string");
            var typeRefOpt = new NonTerminal("typeRefOpt", Empty | refMark);
            var type = new NonTerminal("type", typeName + typeRefOpt);
            MarkTransient(typeName, typeRefOpt);

            var block = new NonTerminal("block");
            var expressionStatement = new NonTerminal("expression-statement");
            var nullStatement = new NonTerminal("null-statement");
            var declarationStatement = new NonTerminal("declaration-statement");
            var selectionStatement = new NonTerminal("selection-statement");
            var iterationStatement = new NonTerminal("iteration-statement");
            var jumpStatement = new NonTerminal("jump-statement");
            var statement = new NonTerminal("statement");
            var statementListOpt = new NonTerminal("statement-list-opt");
            var embeddedStatement = new NonTerminal("embedded-statement");

            var ifStatement = new NonTerminal("if-statement");
            var elseStatement = new NonTerminal("else-statement");
            var elseStatementOpt = new NonTerminal("else-statement-opt");

            var switchStatement = new NonTerminal("switch-statement");
            var switchCases = new NonTerminal("switch-cases");
            var caseLabel = new NonTerminal("case-label");
            var caseConstantLabel = new NonTerminal("case-label-constant");
            var caseDefaultLabel = new NonTerminal("case-label-default");
            var caseStatement = new NonTerminal("case-statement");
            var caseBodyStatement = new NonTerminal("case-body-statement");
            var caseBodyStatements = new NonTerminal("case-body-statements");

            var whileStatement = new NonTerminal("while-statement");
            var forStatement = new NonTerminal("for-statement");
            var forInitExp = new NonTerminal("for-init-exp");
            var forCondExp = new NonTerminal("for-cond-exp");
            var forLoopExp = new NonTerminal("for-loop-exp");

            var variableDeclaration = new NonTerminal("variable-declaration");
            var variableDeclarator = new NonTerminal("variable-declarator");
            var variableDeclarators = new NonTerminal("variable-declarators");
            var variableInit = new NonTerminal("variable-initializer");
            var variableInitOpt = new NonTerminal("variable-initializer-opt");
            var initializer = new NonTerminal("initializer");

            var unaryOperator = new NonTerminal("unary-operator", ToTerm("-") | "!");

            var multiplicativeOperator = new NonTerminal("multiplicative-operator", ToTerm("*") | "/" | "%");
            var additiveOperator = new NonTerminal("additive-operator", ToTerm("+") | "-");
            var relationalEqOperator = new NonTerminal("relational-equality-operator", ToTerm("<") | ">" | "<=" | ">=" | "==" | "!=");
            var assignmentOperator = new NonTerminal("assignment-operator", ToTerm("=") | "+=" | "-=" | "*=" | "/=" | "%=");
            var incOrDec = new NonTerminal("inc-or-dec", ToTerm("++") | "--");
            MarkTransient(multiplicativeOperator, additiveOperator, relationalEqOperator, assignmentOperator, incOrDec);

            var expression = new NonTerminal("expression");
            var expressionOpt = new NonTerminal("expression-opt", Empty | expression);
            var typecast = new NonTerminal("typecast");
            var castExp = new NonTerminal("cast-expression");
            var primaryExp = new NonTerminal("primary-expression");
            var postfix = new NonTerminal("postfix");
            var postfixExp = new NonTerminal("postfix-expression");
            var postfixedExp = new NonTerminal("postfixed-expression");
            var preIncDecExp = new NonTerminal("pre-inc/dec");
            var binExp = new NonTerminal("binary-expression");
            var multiExp = new NonTerminal("multi-expression");
            var additiveExp = new NonTerminal("additive-expression");
            var relationalExp = new NonTerminal("relational-expression");
            var assignmentExp = new NonTerminal("assignment-expression");
            var unaryExp = new NonTerminal("unary-expression");
            var unaryOperatorExp = new NonTerminal("unary-operator-expression");
            var parenthExp = new NonTerminal("parenthesized-expression");
            var localizeExp = new NonTerminal("localize");
            MarkTransient(expressionOpt);

            var memberAccess = new NonTerminal("member-access");
            var memberAccessSegment = new NonTerminal("member-access-segment");
            var memberAccessSegments = new NonTerminal("member-access-segments");

            var methodInvocation = new NonTerminal("method-invocation");
            var argList = new NonTerminal("arg-list");
            var arg = new NonTerminal("arg");


            #endregion

            #region operators, punctuation and delimiters
            RegisterOperators(1, "||");
            RegisterOperators(2, "&&");
            RegisterOperators(3, "|");
            RegisterOperators(4, "^");
            RegisterOperators(5, "&");
            RegisterOperators(6, "==", "!=");
            RegisterOperators(7, "<", ">", "<=", ">=");
            RegisterOperators(8, "<<", ">>");
            RegisterOperators(9, "+", "-");
            RegisterOperators(10, "*", "/", "%");
            //RegisterOperators(11, ".");
            // RegisterOperators(12, "++", "--");
            RegisterOperators(-3, "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>=");

            MarkPunctuation(lPar, rPar, lBrace, rBrace, lSqBracket, rSqBracket, comma, semi, dot, colon);
           // this.MarkTransient(declaration, statement, embeddedStatement, expression, literal, binaryOperator, primaryExp, parenParameters,
           //     declarationStatement, incOrDec, parenthExp);
            #endregion

            #region Place Rules Here
            this.Root = new NonTerminal("Program", statementListOpt);

            // Statements and blocks
            block.Rule = lBrace + statementListOpt + rBrace;
            statement.Rule = declarationStatement | embeddedStatement;
            statementListOpt.Rule = MakeStarRule(statementListOpt, null, statement);
            MarkTransient(statement);

            declarationStatement.Rule = variableDeclaration + semi;
            variableDeclaration.Rule = type + variableDeclarators;
            variableDeclarator.Rule = identifier + variableInitOpt;
            variableInit.Rule = "=" + initializer;
            variableInitOpt.Rule = Empty | variableInit;
            variableDeclarators.Rule = MakePlusRule(variableDeclarators, comma, variableDeclarator);
            initializer.Rule = expression;
            MarkTransient(declarationStatement, variableInitOpt);

            embeddedStatement.Rule = block | nullStatement | expressionStatement | selectionStatement | iterationStatement | jumpStatement;
            nullStatement.Rule = semi;
            expressionStatement.Rule = expression + semi;
            selectionStatement.Rule = ifStatement | switchStatement;
            iterationStatement.Rule = whileStatement | forStatement;
            jumpStatement.Rule = breakStatement | continueStatement | returnStatement;
            returnStatement.Rule = @return + expressionOpt + semi;
            MarkTransient(embeddedStatement, jumpStatement);

            ifStatement.Rule = ToTerm("if") + lPar + expression + rPar + embeddedStatement + elseStatementOpt;
            elseStatementOpt.Rule = Empty | elseStatement;
            elseStatement.Rule = PreferShiftHere() + ToTerm("else") + embeddedStatement;
            MarkTransient(elseStatementOpt);

            switchStatement.Rule = ToTerm("switch") + lPar + expression + rPar + lBrace + switchCases + rBrace;
            switchCases.Rule = MakeStarRule(switchCases, caseStatement);
            caseStatement.Rule = caseLabel + colon + caseBodyStatements;
            caseLabel.Rule = caseConstantLabel | caseDefaultLabel;
            caseConstantLabel.Rule = ToTerm("case") + literal;
            caseDefaultLabel.Rule = ToTerm("default");
            caseBodyStatements.Rule = MakeStarRule(caseBodyStatements, statement);

            whileStatement.Rule = "while" + lPar + expression + rPar + embeddedStatement;

            forStatement.Rule = "for" + lPar + forInitExp + semi + forCondExp + semi + forLoopExp + rPar + embeddedStatement;
            forInitExp.Rule = expressionOpt | variableDeclaration;
            forCondExp.Rule = expressionOpt;
            forLoopExp.Rule = expressionOpt;

            // expression
            expression.Rule =  binExp | unaryExp | typecast;

            binExp.Rule = multiExp | additiveExp | relationalExp | assignmentExp;
            multiExp.Rule = expression + multiplicativeOperator + expression;
            additiveExp.Rule = expression + additiveOperator + expression;
            relationalExp.Rule = expression + relationalEqOperator + expression;
            assignmentExp.Rule = expression + assignmentOperator + expression;
            MarkTransient(binExp);

            castExp.Rule = unaryExp | typecast;
            typecast.Rule = lPar + type + rPar + castExp;
            MarkTransient(castExp);

            unaryExp.Rule = postfixExp | preIncDecExp | unaryOperatorExp;
            preIncDecExp.Rule = incOrDec + unaryExp;
            unaryOperatorExp.Rule = unaryOperator + castExp;
            MarkTransient(unaryExp);

            postfixExp.Rule = primaryExp | postfixedExp;
            postfixedExp.Rule = postfixExp + postfix;
            postfix.Rule = methodInvocation | memberAccess | incOrDec;
            MarkTransient(postfixExp);

            primaryExp.Rule = literal | identifier | parenthExp | localizeExp;
            parenthExp.Rule = lPar + expression + rPar;
            localizeExp.Rule = lSqBracket + doubleString + rSqBracket;
            MarkTransient(expression, primaryExp);

            memberAccess.Rule = dot + identifier;

            methodInvocation.Rule = lPar + argList + rPar;
            argList.Rule = MakeStarRule(argList, comma, arg);
            arg.Rule = expression;
            MarkTransient(argList);
            #endregion
        }

        new void MarkTransient(params NonTerminal[] args)
        {
            base.MarkTransient(args);
        }
    }
}
