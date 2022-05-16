using System.Text.RegularExpressions;

namespace TextPress;

/// <summary>
/// Provides configuration for a <see cref="StringTemplate"/> instance.
/// </summary>
public record struct StringTemplateOptions
{
	public StringTemplateOptions() { }

	/// <summary>
	/// Defines the start delimiter for a template variable.
	/// </summary>
	/// <example>
	///	For a template variable like <c>{something}</c>, the start delimiter is <c>{</c>.
	/// </example>
	public string StartDelimiter { get; init; } = "{";
	
	/// <summary>
	/// Defines the end delimiter for a template variable.
	/// </summary>
	/// <example>
	///	For a template variable like <c>{something}</c>, the end delimiter is <c>}</c>.
	/// </example>
	public string EndDelimiter { get; init; } = "}";

	/// <summary>
	/// Defines the escape character for a template variable.
	/// </summary>
	/// <remarks>
	///	The use and position of the escape character is dependent whether the <see cref="EscapingStyle"/> is set to 
	/// <see cref="VariableEscapingStyle.StartingCharacter"/> or <see cref="VariableEscapingStyle.EndingCharacter"/>.
	/// </remarks>
	/// <example>
	///		<para>
	///		- If <see cref="EscapingStyle"/> is set to <see cref="VariableEscapingStyle.StartingCharacter"/>,
	///		  the escape the escape character is <c>$</c>, for an escaped template variable like <c>${something}</c>.
	///		</para>
	///
	///		<para>
	///		- If <see cref="EscapingStyle"/> is set to <see cref="VariableEscapingStyle.EndingCharacter"/>,
	///		  the escape the escape character is <c>$</c>, for an escaped template variable like <c>{something}$</c>.
	///		</para>
	/// </example>
	public char? EscapeCharacter { get; init; } = null;
	
	public VariableEscapingStyle EscapingStyle { get; init; } = VariableEscapingStyle.None;
	public RegexOptions RegexOptions { get; init; } = RegexOptions.None;
}

/// <summary>
/// Defines escaping strategies used to properly parse Variables in a <see cref="StringTemplate"/> instance.
/// </summary>
public enum VariableEscapingStyle : byte
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
	/// (e.g: <c>${variableName}</c>)
	/// </summary>
	StartingCharacter,
	
	/// <summary>
	/// Escaping is performed by appending an escape character to the end of a variable.
	/// (e.g: <c>{variableName}$</c>)
	/// </summary>
	EndingCharacter
}