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
            CommentTerminal blockComment = new CommentTerminal("block-comment", "/*", "*/");
            CommentTerminal lineComment = new CommentTerminal("line-comment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            NonGrammarTerminals.Add(blockComment);
            NonGrammarTerminals.Add(lineComment);

            NumberLiteral number = new NumberLiteral("number");
            IdentifierTerminal identifier = new IdentifierTerminal("identifier");
            StringLiteral backString = new StringLiteral("back-string", "`");
            StringLiteral doubleString = new StringLiteral("double-string", "\"");
            KeyTerm refMark = ToTerm("&", "ref-mark");
            KeyTerm lPar = ToTerm("(");
            KeyTerm rPar = ToTerm(")");
            KeyTerm lBrace = ToTerm("{");
            KeyTerm rBrace = ToTerm("}");
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
            NonTerminal statement = new NonTerminal("statement");
            NonTerminal statementListOpt = new NonTerminal("statement-list-opt");
            NonTerminal declarationStatement = new NonTerminal("declaration-statement");
            NonTerminal embeddedStatement = new NonTerminal("embedded-statement");

            NonTerminal variableDeclaration = new NonTerminal("variable-declaration");
            NonTerminal variableDeclarator = new NonTerminal("variable-declarator");
            NonTerminal variableDeclarators = new NonTerminal("variable-declarators");
            NonTerminal initializer = new NonTerminal("initializer");



            NonTerminal expression = new NonTerminal("expression");
            NonTerminal typecast = new NonTerminal("typecast");
            NonTerminal primaryExpression = new NonTerminal("primary-expression");
            NonTerminal binOpExp = new NonTerminal("binary-operation");

            NonTerminal memberAccess = new NonTerminal("member-access");
            NonTerminal memberAccessSegment = new NonTerminal("member-access-segment");
            NonTerminal memberAccessSegments = new NonTerminal("member-access-segments");

            NonTerminal methodInvocation = new NonTerminal("method-invocation");
            NonTerminal argList = new NonTerminal("arg-list");
            NonTerminal arg = new NonTerminal("arg");
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

            // Decalartion statement
            declarationStatement.Rule = variableDeclaration + semi;
            variableDeclaration.Rule = typeRef + variableDeclarators;
            variableDeclarator.Rule = identifier + "=" + initializer;
            variableDeclarators.Rule = MakePlusRule(variableDeclarators, comma, variableDeclarator);
            initializer.Rule = expression;

            // Embedded statement
            embeddedStatement.Rule = block | semi;

            // expression
            expression.Rule = typecast | primaryExpression;
            typecast.Rule = lPar + typeRef + rPar + primaryExpression;
            primaryExpression.Rule = literal | identifier | memberAccess;

            memberAccess.Rule = identifier + memberAccessSegments;
            memberAccessSegments.Rule = MakePlusRule(memberAccessSegments, null, memberAccessSegment);
            memberAccessSegment.Rule = dot + identifier | methodInvocation;

            methodInvocation.Rule = lPar + argList + rPar;
            argList.Rule = MakeStarRule(argList, comma, arg);
            arg.Rule = expression;
            #endregion

            MarkPunctuation(lPar, rPar, lBrace, rBrace, comma, semi, dot);
        }
    }
}
