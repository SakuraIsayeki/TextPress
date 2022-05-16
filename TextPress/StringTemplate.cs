using System.Text.RegularExpressions;

namespace TextPress;

/// <summary>
/// Base class for a string template.
/// </summary>
public class StringTemplate
{
	/// <summary>
	/// Represents the Regular Expression for which variables will be matched.
	/// </summary>
	protected Regex TemplateRegex { get; } = _defaultTemplateRegex;

	private const string DefaultTemplatePattern = "{(?<variable>[^}]+)}";
	protected static readonly Regex _defaultTemplateRegex = new(DefaultTemplatePattern, RegexOptions.Compiled);

	public static StringTemplate Default { get; } = new(_defaultTemplateRegex);

	/// <summary>
	/// Instantiates a new StringTemplate using the given options.
	/// </summary>
	public StringTemplate(StringTemplateOptions options)
	{
		TemplateRegex = BuildTemplateRegex(options);
	}

	protected internal StringTemplate(Regex regex)
	{
		TemplateRegex = regex;
	}

	/// <summary>
	/// Builds a Regex from the given options.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	protected internal static Regex BuildTemplateRegex(StringTemplateOptions options)
	{
		/*
		 * Regex is built with the following pattern:
		 *   - Starts with options.StartDelimiter, ends with options.EndDelimiter
		 *   - Contains a group named "variable"
		 *
		 *   - If EscapingStyle is not None, then the following is added to the pattern:
		 *     - StartDelimiter indicates that the variable is escaped if the EscapeCharacter is present in front of the variable
		 *	   - EndDelimiter indicates that the variable is escaped if the EscapeCharacter is present after the variable
		 *	   - DoubleDelimiters indicates that the variable is escaped if StartDelimiter and EndDelimiter are present twice in a row.
		 */

		// Escape all options characters
		string startDelimiter = Regex.Escape(options.StartDelimiter.ToString());
		string endDelimiter = Regex.Escape(options.EndDelimiter.ToString());
		string? escapeCharacter = null;
		
		if (options is { EscapingStyle: not (VariableEscapingStyle.None or VariableEscapingStyle.DoubleDelimiters), EscapeCharacter: not null })
		{
			escapeCharacter = Regex.Escape(options.EscapeCharacter.ToString()!);
		}

		string pattern = options.EscapingStyle switch
		{
			// No escaping
			VariableEscapingStyle.None => $"{startDelimiter}(?<variable>[^{endDelimiter}]+){endDelimiter}",

			not (VariableEscapingStyle.None or VariableEscapingStyle.DoubleDelimiters) when options.EscapeCharacter is null 
				=> throw new ArgumentOutOfRangeException(nameof(options), "EscapeCharacter must be specified when EscapingStyle is not None."),
			
			// Exclude any string with double delimiters (e.g. {{variable}} will be ignored)
			VariableEscapingStyle.DoubleDelimiters => $"{startDelimiter}(?<variable>[^{endDelimiter}]+(?<!{startDelimiter}{endDelimiter})){endDelimiter}",

			// Escape if the EscapeCharacter is present in front of the variable (e.g. !{variable} will be ignored)
			// Use negative lookbehind to ensure that the EscapeCharacter is not the first character in the string
			VariableEscapingStyle.StartingCharacter => $"(?<!{escapeCharacter}){startDelimiter}(?<variable>[^{endDelimiter}]+){endDelimiter}",
			

			// Escape if the EscapeCharacter is present after the variable (e.g. {variable}! will be ignored)
			// Use negative lookahead to ensure that the EscapeCharacter is not the last character in the string
			VariableEscapingStyle.EndingCharacter => $"{startDelimiter}(?<variable>[^{endDelimiter}]+){endDelimiter}(?!{escapeCharacter})",
				
			_ => throw new ArgumentOutOfRangeException(nameof(options.EscapingStyle), options.EscapingStyle, "Invalid escaping style.")
		};

		return new(pattern, options.RegexOptions);
	}

	/*
	 * FIXME: Method is allocating too much memory (552 bytes per call).
	 */
	/// <summary>
	/// Substitutes variables notated as "{variable}" in the model string, with values from the corresponding dictionary keys.
	/// </summary>
	public string Fill(string model, IReadOnlyDictionary<string, string> values) => TemplateRegex.Replace(model, match =>
		{
			string variable = match.Groups["variable"].Value;

			return values.ContainsKey(variable) ? values[variable] : match.Value;
		}
	);
}