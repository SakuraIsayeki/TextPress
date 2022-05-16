using Xunit;

namespace TextPress.Tests;

public class StringTemplateFactoryTests
{
	[Fact]
	public void TestFactoryReturnsNewTemplate()
	{
		StringTemplateFactory factory = new();
		StringTemplate template = factory.GetTemplate("");

		Assert.IsType<StringTemplate>(template);
	}
	
	[Fact]
	public void TestFactoryReturnsSameNamedTemplate()
	{
		StringTemplateFactory factory = new();
		StringTemplate template = factory.GetTemplate("something");
		StringTemplate template2 = factory.GetTemplate("something");

		Assert.Same(template, template2);
	}
	
	[Fact]
	public void TestFactoryReturnsDifferentNamedTemplates()
	{
		StringTemplateFactory factory = new();
		StringTemplate template = factory.GetTemplate("something");
		StringTemplate template2 = factory.GetTemplate("something else");

		Assert.NotSame(template, template2);
	}
}