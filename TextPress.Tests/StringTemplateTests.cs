using System;
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
	public void TestUnusedVariables()
	{
		const string template = "Hello, {name}!";
		const string expected = "Hello, World!";

		StringTemplate templateEngine = new(new());
		
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "name", "World" }, { "age", "42" } }));
	}
	
	[Fact]
	public void TestWrongVariables()
	{
		const string template = "Hello, {name}!";
		const string expected = "Hello, {name}!";

		StringTemplate templateEngine = new(new() { EscapingStyle = VariableEscapingStyle.DoubleDelimiters });
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "age", "42" } }));
	}
	
	[Fact]
	public void TestRepeatedVariables()
	{
		const string template = "Hello, {name}! Hello, {name}!";
		const string expected = "Hello, World! Hello, World!";

		StringTemplate templateEngine = new(new() { EscapingStyle = VariableEscapingStyle.DoubleDelimiters });
		Assert.Equal(expected, templateEngine.Fill(template, new Dictionary<string, string> { { "name", "World" } }));
	}
	
	[Fact]
	public void TestInvalidTemplateOptions()
	{
		// Invalid delimiters
		Assert.Throws<ArgumentOutOfRangeException>(() => new StringTemplate(new() { StartDelimiter = "" }));
		Assert.Throws<ArgumentOutOfRangeException>(() => new StringTemplate(new() { EndDelimiter = "" }));
		
		// Invalid escape character
		Assert.Throws<ArgumentOutOfRangeException>(() => new StringTemplate(new() { EscapingStyle = VariableEscapingStyle.StartingCharacter, EscapeCharacter = '\0' }));
		
		// No escape character specified for escaping styles that require it
		Assert.Throws<ArgumentOutOfRangeException>(() => new StringTemplate(new() { EscapingStyle = VariableEscapingStyle.StartingCharacter }));
		Assert.Throws<ArgumentOutOfRangeException>(() => new StringTemplate(new() { EscapingStyle = VariableEscapingStyle.EndingCharacter }));
	}
}