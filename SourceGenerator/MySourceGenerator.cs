using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator;

[Generator]
public class MySourceGenerator : ISourceGenerator
{
	public void Execute(GeneratorExecutionContext context)
	{
		context.Log("start");

		if (context.SyntaxReceiver is not SyntaxReceiver receiver)
		{
			context.Log("");
			return;
		}
		context.Log("");
		ClassDeclarationSyntax cds = receiver.Class;
		context.Log(cds.Identifier);


		string path = GetBinPath(context, cds);
		context.Log($"path is '{path}'");
		string filePath = path + "remdir.bat";
		context.Log($"file is '{filePath}'");

		string content = GetExecutableFileContent();
		WriteFile(filePath, content);

		string outputResult = Execute(filePath);
		context.Log($"result is '{outputResult.Replace("\n", "").Replace("\r", "")}'");

		context.Log("finished");
	}

	public void Initialize(GeneratorInitializationContext context)
	{
		context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
	}


	static string GetBinPath(GeneratorExecutionContext context, ClassDeclarationSyntax cds)
	{
		string moduleName = context.Compilation.SourceModule.Name.Replace(".dll", "").Replace(".exe", "");

		string classLocation = cds.Identifier.GetLocation().SourceTree.FilePath;

		int ind = classLocation.IndexOf(moduleName);
		string path = classLocation.Substring(0, ind);
		return path;
	}

	static void WriteFile(string filePath, string content)
	{
		using var file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

		byte[] bytesContent = Encoding.UTF8.GetBytes(content);

		file.Write(bytesContent, 0, bytesContent.Length);
	}

	static string GetExecutableFileContent()
	{
		return "@echo off\r\nrem i can do everything from current directory\n\recho %~dp0";
	}

	static string Execute(string fullFilePath)
	{
		using var process = Process.Start(new ProcessStartInfo
		{
			FileName = fullFilePath,
			UseShellExecute = false,
			RedirectStandardOutput = true
		});

		process.Start();

		string outputResult = process.StandardOutput.ReadToEnd();

		process.WaitForExit();

		return outputResult;
	}
}