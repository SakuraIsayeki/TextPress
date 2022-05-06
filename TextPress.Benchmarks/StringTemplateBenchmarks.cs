using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TextPress;

// Disable CA1822 as benchmark methods cannot be static.
#pragma warning disable CA1822 


namespace TextPress.Benchmarks;

[MemoryDiagnoser, GcServer(true)]
public class StringTemplateBenchmarks
{
	[Benchmark]
	public string FillTemplateCompiledRegex() => StringTemplate.FillTemplate("Hello, {name}!", new Dictionary<string, string> { { "name", "World" } });
	
}