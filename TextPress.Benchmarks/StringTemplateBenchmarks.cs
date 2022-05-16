using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TextPress;

// Disable CA1822 as benchmark methods cannot be static.
#pragma warning disable CA1822 


namespace TextPress.Benchmarks;

[MemoryDiagnoser, GcServer(true)]
public class StringTemplateBenchmarks
{
	private static readonly Dictionary<string, string> values = new() { { "name", "World" } };

	[Benchmark]
	public string FillTemplateCompiledRegex() => StringTemplate.Default.Fill("Hello, {name}!", values);
	
}