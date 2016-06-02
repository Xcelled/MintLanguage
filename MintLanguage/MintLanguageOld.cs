using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MintLanguage
{
    public class MintLanguageOld : Grammar
    {
        public MintLanguageOld()
        {
            #region Declare Terminals Here
            CommentTerminal blockComment = new CommentTerminal("block-comment", "/*", "*/");
            CommentTerminal lineComment = new CommentTerminal("line-comment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            NonGrammarTerminals.Add(blockComment);
            NonGrammarTerminals.Add(lineComment);

            NumberLiteral number = new NumberLiteral("number");
            IdentifierTerminal identifier = new IdentifierTerminal("identifier");
            StringLiteral backString = new StringLiteral("back-string", "`");
            StringLiteral doubleString = new StringLiteral("double-string", "\"");
            KeyTerm refMark = ToTerm("&");
            KeyTerm lPar = ToTerm("(");
            KeyTerm rPar = ToTerm(")");
            KeyTerm lBrace = ToTerm("{");
            KeyTerm rBrace = ToTerm("}");
            var lSqBracket = ToTerm("[");
            var rSqBracket = ToTerm("]");
            KeyTerm semi = ToTerm(";");
            KeyTerm comma = ToTerm(",");
            KeyTerm dot = ToTerm(".");
            #endregion

            #region Declare NonTerminals Here
            NonTerminal literal = new NonTerminal("literal", number | backString | doubleString | "true" | "false");

            NonTerminal program = new NonTerminal("program");
            NonTerminal declarations = new NonTerminal("declarations");
            NonTerminal declaration = new NonTerminal("declaration");
            NonTerminal method = new NonTerminal("method");
            NonTerminal modifier = new NonTerminal("modifier");
            NonTerminal typeRef = new NonTerminal("type-ref");
            NonTerminal refMarkOpt = new NonTerminal("ref-mark-opt", Empty | refMark);
            NonTerminal parenParameters = new NonTerminal("paren-parameters");
            NonTerminal parameters = new NonTerminal("parameters");
            NonTerminal parameter = new NonTerminal("parameter");

            NonTerminal block = new NonTerminal("block");
            NonTerminal statementExp = new NonTerminal("statement-exp");
            NonTerminal statement = new NonTerminal("statement");
            NonTerminal statementListOpt = new NonTerminal("statement-list-opt");
            NonTerminal declarationStatement = new NonTerminal("declaration-statement");
            NonTerminal embeddedStatement = new NonTerminal("embedded-statement");

            NonTerminal variableDeclaration = new NonTerminal("variable-declaration");
            NonTerminal variableDeclarator = new NonTerminal("variable-declarator");
            NonTerminal variableDeclarators = new NonTerminal("variable-declarators");
            NonTerminal initializer = new NonTerminal("initializer");

            NonTerminal unaryOperator = new NonTerminal("unary-operator", ToTerm("-") | "!");
            NonTerminal binaryOperator = new NonTerminal("binary-operator", ToTerm("<") | "||" | "&&" | "|" | "^" | "&" | "==" | "!=" | ">" | "<=" | ">=" | "<<" | ">>" | "+" | "-" | "*" | "/" | "%");
            NonTerminal assignmentOperator = new NonTerminal("assignment-operator", ToTerm("=") | "+=" | "-=" | "*=" | "/=" | "%=" | "&=" | "|=" | "^=" | "<<=" | ">>=");
            NonTerminal incOrDec = new NonTerminal("inc-or-dec", ToTerm("++") | "--");

            NonTerminal expression = new NonTerminal("expression");
            NonTerminal typecast = new NonTerminal("typecast");
            NonTerminal primaryExp = new NonTerminal("primary-expression");
            NonTerminal binOpExp = new NonTerminal("binary-expression");
            NonTerminal unaryExp = new NonTerminal("unary-expression");
            NonTerminal parenthExp = new NonTerminal("parenthesized-expression");
            NonTerminal preIncDecExp = new NonTerminal("pre-inc/dec");
            NonTerminal postIncDecExp = new NonTerminal("post-inc/dec");
            var localizeExp = new NonTerminal("localize");

            NonTerminal memberAccess = new NonTerminal("member-access");
            NonTerminal memberAccessSegment = new NonTerminal("member-access-segment");
            NonTerminal memberAccessSegments = new NonTerminal("member-access-segments");

            NonTerminal methodInvocation = new NonTerminal("method-invocation");
            NonTerminal argList = new NonTerminal("arg-list");
            NonTerminal arg = new NonTerminal("arg");

            var selectionStatement = new NonTerminal("selection");
            var ifStatement = new NonTerminal("if");
            var switchStatement = new NonTerminal("switch");
            var elseClauseOpt = new NonTerminal("else-clause-opt");
            var switch_section = new NonTerminal("switch_section");
            var switch_sections_opt = new NonTerminal("switch_sections_opt");
            var switch_label = new NonTerminal("switch_label");
            var switch_labels = new NonTerminal("switch_labels");
            var while_statement = new NonTerminal("while_statement");
            var do_statement = new NonTerminal("do_statement");
            var for_statement = new NonTerminal("for_statement");
            var foreach_statement = new NonTerminal("foreach_statement");
            var for_initializer_opt = new NonTerminal("for_initializer_opt");
            var for_condition_opt = new NonTerminal("for_condition_opt");
            var for_iterator_opt = new NonTerminal("for_iterator_opt");
            var break_statement = new NonTerminal("break_statement");
            var continue_statement = new NonTerminal("continue_statement");
            var goto_statement = new NonTerminal("goto_statement");
            var return_statement = new NonTerminal("return_statement");

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

            MarkPunctuation(lPar, rPar, lBrace, rBrace, lSqBracket, rSqBracket, comma, semi, dot);
           // this.MarkTransient(declaration, statement, embeddedStatement, expression, literal, binaryOperator, primaryExp, parenParameters,
           //     declarationStatement, incOrDec, parenthExp);

            this.AddTermsReportGroup("assignment", "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>=");
            this.AddTermsReportGroup("statement", "if", "switch", "do", "while", "for", "foreach", "continue", "goto", "return", "try", "yield",
                                                  "break", "throw", "unchecked", "using");
            this.AddTermsReportGroup("constant", number, backString, doubleString);
            this.AddTermsReportGroup("constant", "true", "false");

            this.AddTermsReportGroup("unary operator", "+", "-", "!", "~");

            this.AddToNoReportGroup(comma, semi);
            this.AddToNoReportGroup("++", "--", "{", "}");
            //
            #endregion

            #region Place Rules Here
            this.Root = program;

            program.Rule = declarations;

            declarations.Rule = MakeStarRule(declarations, declaration);

            declaration.Rule = method | declarationStatement;

            method.Rule = modifier + typeRef + identifier + parenParameters + block;

            modifier.Rule = ToTerm("server") | "client";

            typeRef.Rule = identifier + refMarkOpt;

            // Parameters
            parameter.Rule = typeRef + identifier;
            parameters.Rule = MakeStarRule(parameters, comma, parameter);
            parenParameters.Rule = lPar + parameters + rPar;

            // Statements and blocks
            block.Rule = lBrace + statementListOpt + rBrace;
            statementListOpt.Rule = MakeStarRule(statementListOpt, null, statement);
            statement.Rule = declarationStatement | embeddedStatement;
            statementExp.Rule = memberAccess | memberAccess + assignmentOperator + expression | preIncDecExp | postIncDecExp;

            // Decalartion statement
            declarationStatement.Rule = variableDeclaration + semi;
            variableDeclaration.Rule = typeRef + variableDeclarators;
            variableDeclarator.Rule = identifier + "=" + initializer;
            variableDeclarators.Rule = MakePlusRule(variableDeclarators, comma, variableDeclarator);
            initializer.Rule = expression;

            // Embedded statement
            embeddedStatement.Rule = block | semi | statementExp | selectionStatement;

            // Selection
            selectionStatement.Rule = ifStatement; //| switchStatement;
            
            // If else
            ifStatement.Rule = ToTerm("if") + parenthExp + embeddedStatement + elseClauseOpt;
            elseClauseOpt.Rule = Empty | PreferShiftHere() + "else" + embeddedStatement;

            // switch
            switchStatement.Rule = "switch" + parenthExp + lBrace + switch_sections_opt + rBrace;
            //switch_section.Rule = switch_labels + statement_list;
            //switch_sections_opt.Rule = MakeStarRule(switch_sections_opt, null, switch_section);
            //switch_label.Rule = "case" + expression + colon | "default" + colon;
            //switch_labels.Rule = MakePlusRule(switch_labels, null, switch_label);

            // expression
            expression.Rule = typecast | binOpExp | primaryExp;
            typecast.Rule = parenthExp + primaryExp;
            binOpExp.Rule = expression + binaryOperator + expression;
            primaryExp.Rule = literal | identifier | memberAccess | unaryExp | parenthExp | preIncDecExp | postIncDecExp | localizeExp;
            unaryExp.Rule = unaryOperator + primaryExp;
            parenthExp.Rule = lPar + expression + rPar;
            preIncDecExp.Rule = incOrDec + memberAccess;
            postIncDecExp.Rule = memberAccess + incOrDec;
            localizeExp.Rule = lSqBracket + doubleString + rSqBracket;

            memberAccess.Rule = identifier + memberAccessSegments;
            memberAccessSegments.Rule = MakePlusRule(memberAccessSegments, null, memberAccessSegment);
            memberAccessSegment.Rule = dot + identifier | methodInvocation;

            methodInvocation.Rule = lPar + argList + rPar;
            argList.Rule = MakeStarRule(argList, comma, arg);
            arg.Rule = expression;
            #endregion

        }
    }
}
