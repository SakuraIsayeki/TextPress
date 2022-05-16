using System.Collections.Immutable;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TextPress;

// Disable CA1822 as benchmark methods cannot be static.
#pragma warning disable CA1822 


namespace TextPress.Benchmarks;

[MemoryDiagnoser, GcServer(true)]
public class StringTemplateBenchmarks
{
	private static readonly Dictionary<string, string> _values = new() { { "name", "World" } };

	private static readonly StringTemplateFactory _factory = new();
	
	[GlobalSetup]
	public void Setup()
	{
		// Pre-initialize all templates.
		_factory.GetTemplate("");
		_factory.GetTemplate("");
		_factory.GetTemplate("custom", new()
		{
			StartDelimiter = "${",
			EndDelimiter = "}*",
			EscapeCharacter = '!',
			EscapingStyle = VariableEscapingStyle.StartingCharacter,
			RegexOptions = RegexOptions.Compiled
		});
	}
	
	[Benchmark] public string FillSimpleTemplate() => StringTemplate.Default.Fill("Hello, {name}!", _values);
	[Benchmark] public string FillSimpleFactoryTemplate() => _factory.GetTemplate("").Fill("Hello, {name}!", _values);
	[Benchmark] public string FillCustomTemplate() => _factory.GetTemplate("custom").Fill("Hello, ${name}*!", _values);

}