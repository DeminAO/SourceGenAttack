using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator;

class SyntaxReceiver : ISyntaxReceiver
	{
		public ClassDeclarationSyntax Class { get; private set; }
		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			if (syntaxNode is ClassDeclarationSyntax cds)
			{
				Class = cds;
			}
		}
	}