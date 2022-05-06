using System.Text.RegularExpressions;

namespace TextPress;

/// <summary>
/// Base class for a string template.
/// </summary>
public partial class StringTemplate
{
	private const string TemplatePattern = "{(?<variable>[^}]+)}";

#if NET7_0_OR_GREATER
	[RegexGenerator(TemplatePattern)]
	private static partial Regex TemplateRegex();
#else
	private static Regex TemplateRegex() => _templateRegex;
	private static readonly Regex _templateRegex = new(TemplatePattern, RegexOptions.Compiled);
#endif
	
	/// <summary>
	/// Substitutes variables notated as "{variable}" with values from the corresponding dictionary keys.
	/// </summary>
	/// <param name="model"></param>
	/// <param name="values"></param>
	/// <returns></returns>
	public static string FillTemplate(string model, IDictionary<string, string> values) 
		=> TemplateRegex().Replace(model, match =>
		{
			string variable = match.Groups["variable"].Value;
			return values.ContainsKey(variable) ? values[variable] : match.Value;
		});
}