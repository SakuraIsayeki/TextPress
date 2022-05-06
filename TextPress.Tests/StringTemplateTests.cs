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

		string result = StringTemplate.FillTemplate(template, new Dictionary<string, string> { { "name", "World" } });
		Assert.Equal(expected, result);
	}
	
	[Fact]
	public void TestSimpleTemplateRegexWithMultipleValues()
	{
		const string template = "Hello, {name}! You are {age} years old.";
		const string expected = "Hello, World! You are 42 years old.";

		string result = StringTemplate.FillTemplate(template, new Dictionary<string, string> { { "name", "World" }, { "age", "42" } });
		Assert.Equal(expected, result);
	}
}