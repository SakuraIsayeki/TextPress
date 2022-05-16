// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using TextPress.Benchmarks;


Summary? summary = BenchmarkRunner.Run<StringTemplateBenchmarks>(DefaultConfig.Instance
#if !LATEST_RUNTIME_ONLY
	.AddJob(Job.Default.WithRuntime(CoreRuntime.Core60))
#endif
#if NET7_0_OR_GREATER
	.AddJob(Job.Default.WithRuntime(CoreRuntime.CreateForNewVersion("net7.0", ".NET 7.0 (Workstation GC)")).AsBaseline())
	.AddJob(Job.Default.WithRuntime(CoreRuntime.CreateForNewVersion("net7.0", ".NET 7.0 (Server GC)")).WithGcServer(true))
#endif
);