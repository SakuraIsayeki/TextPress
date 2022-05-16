using System.Text.RegularExpressions;

namespace TextPress;

/// <summary>
/// Provides configuration for a <see cref="StringTemplate"/> instance.
/// </summary>
public record StringTemplateOptions
{
	public char StartDelimiter { get; init; } = '{';
	public char EndDelimiter { get; init; } = '}';
	public char? EscapeCharacter { get; init; }
	
	public VariableEscapingStyle EscapingStyle { get; init; } = VariableEscapingStyle.None;
	public RegexOptions RegexOptions { get; init; }
}

/// <summary>
/// Defines escaping strategies used to properly parse Variables in a <see cref="StringTemplate"/> instance.
/// </summary>
public enum VariableEscapingStyle
{
	/// <summary>
	/// No escaping is performed.
	/// </summary>
	None,
	
	/// <summary>
	/// Escaping is performed by doubling variable delimiters.
	/// (e.g: <c>{{variableName}}</c>)
	/// </summary>
	DoubleDelimiters,
	
	/// <summary>
	/// Escaping is performed by appending an escape character to the start of a variable.
	/// (e.g: <c>!{variableName}</c>)
	/// </summary>
	StartingCharacter,
	
	/// <summary>
	/// Escaping is performed by appending an escape character to the end of a variable.
	/// (e.g: <c>{variableName}!</c>)
	/// </summary>
	EndingCharacter
}