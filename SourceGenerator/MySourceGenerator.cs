using System;
using Microsoft.CodeAnalysis;

namespace SourceGenerator;

[Generator]
public class MySourceGenerator : ISourceGenerator
{
	public void Execute(GeneratorExecutionContext context)
	{
		context.AddSource("empty.g.cs", "namespace EmptyNamespace { public class Empty { } }");

		context.Log("start");

		if (context.SyntaxReceiver is SyntaxReceiver receiver)
		{
			var cds = receiver.Class;

			context.Log(cds);
		}

	}
	public void Initialize(GeneratorInitializationContext context)
	{
		context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
	}
}