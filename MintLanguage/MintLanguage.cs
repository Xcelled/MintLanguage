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
            KeyTerm refMark = ToTerm("&", "refMark");
            #endregion

            #region Declare NonTerminals Here
            NonTerminal program = new NonTerminal("program");
            NonTerminal declarations = new NonTerminal("declarations");
            NonTerminal declaration = new NonTerminal("declaration");
            NonTerminal modifier = new NonTerminal("modifier");
            NonTerminal typeRef = new NonTerminal("type-ref");
            NonTerminal refMarkOpt = new NonTerminal("refmarkOpt", Empty | refMark);
            NonTerminal parenParameters = new NonTerminal("paren-parameters");
            NonTerminal parameters = new NonTerminal("parameters");
            NonTerminal parameter = new NonTerminal("parameter");

            #endregion

            #region Place Rules Here
            this.Root = program;

            program.Rule = declarations;

            declarations.Rule = MakeStarRule(declarations, declaration);

            declaration.Rule = modifier + typeRef + identifier + parenParameters;

            modifier.Rule = ToTerm("server") | "client";
            typeRef.Rule = identifier + refMarkOpt;

            parameter.Rule = typeRef + identifier;
            parameters.Rule = MakeStarRule(parameters, ToTerm(","), parameter);
            parenParameters.Rule = ToTerm("(") + parameters + ")";
            #endregion

            MarkPunctuation("(", ")", ",");
        }
    }
}
