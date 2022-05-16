using System.Collections.Generic;
using Xunit;

namespace TextPress.Tests;

public class StringTemplateOptionsTests
{
	[Fact]
	public void TestTemplateOptionsRetention()
	{
		StringTemplateOptions options = new() { StartDelimiter = "<<", EndDelimiter = ">>" };
		StringTemplate template = new(options);
		
		Assert.Equal(options, template.Options);
	}
	
	[Fact]
	public void TestDoubleEscapedTemplateRegex()
	{
		const string template = "Hello, {name}! Hello {{name}}!";
		const string expected = "Hello, world! Hello {{name}}!";

		StringTemplate templateEngine = new(new() { EscapingStyle = VariableEscapingStyle.DoubleDelimiters });
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "name", "world" } }));
	}
	
	[Fact]
	public void TestStartEscapedTemplateRegex()
	{
		const string template = "Hello, {name}! Hello ${name}!";
		const string expected = "Hello, world! Hello ${name}!";

		StringTemplate templateEngine = new(new()
		{
			EscapingStyle = VariableEscapingStyle.StartingCharacter, 
			EscapeCharacter = '$'
		});
		
		Assert.Equal(VariableEscapingStyle.StartingCharacter, templateEngine.Options.EscapingStyle);
		Assert.Equal('$', templateEngine.Options.EscapeCharacter);
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "name", "world" } }));
	}
	
	[Fact]
	public void TestEndEscapedTemplateRegex()
	{
		const string template = "Hello, {name}! Hello {name}$!";
		const string expected = "Hello, world! Hello {name}$!";

		StringTemplate templateEngine = new(new()
		{
			EscapingStyle = VariableEscapingStyle.EndingCharacter, 
			EscapeCharacter = '$'
		});
		
		Assert.Equal(VariableEscapingStyle.EndingCharacter, templateEngine.Options.EscapingStyle);
		Assert.Equal('$', templateEngine.Options.EscapeCharacter);
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "name", "world" } }));
	}
	
	[Fact]
	public void TestMulticharDelimitersTemplateRegex()
	{
		const string template = "Hello, *{name}*! Hello {name}!";
		const string expected = "Hello, world! Hello {name}!";

		StringTemplate templateEngine = new(new()
		{
			EscapingStyle = VariableEscapingStyle.DoubleDelimiters, 
			StartDelimiter = "*{",
			EndDelimiter = "}*"
		});
		
		Assert.Equal(VariableEscapingStyle.DoubleDelimiters, templateEngine.Options.EscapingStyle);
		Assert.Equal("*{", templateEngine.Options.StartDelimiter);
		Assert.Equal("}*", templateEngine.Options.EndDelimiter);
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "name", "world" } }));
	}
}