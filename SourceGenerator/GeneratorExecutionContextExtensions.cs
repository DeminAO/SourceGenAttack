using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace SourceGenerator;

static class GeneratorExecutionContextExtensions
{
	public static void Log(this GeneratorExecutionContext context, object data, [CallerLineNumber] int line = 0)
	{
		string mes = $"report line {line}, data: {data?.ToString()}";
		context.ReportDiagnostic(Diagnostic.Create("sg1", "cat1", mes, DiagnosticSeverity.Warning, DiagnosticSeverity.Warning, true, 1));
	}
}