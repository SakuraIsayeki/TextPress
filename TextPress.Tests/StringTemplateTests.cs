using System.Collections.Generic;
using Xunit;

namespace TextPress.Tests;

public class StringTemplateTests
{
	[Fact]
	public void TestSimpleTemplateRegex()
	{
		const string template = "Hello, {name}!";
		const string expected = "Hello, World!";

		string result = StringTemplate.Default.Fill(template, new Dictionary<string, string> { { "name", "World" } });
		Assert.Equal(expected, result);
	}
	
	[Fact]
	public void TestSimpleTemplateRegexWithMultipleValues()
	{
		const string template = "Hello, {name}! You are {age} years old.";
		const string expected = "Hello, World! You are 42 years old.";

		string result = StringTemplate.Default.Fill(template, new Dictionary<string, string> { { "name", "World" }, { "age", "42" } });
		Assert.Equal(expected, result);
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
		
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "name", "world" } }));
	}
}