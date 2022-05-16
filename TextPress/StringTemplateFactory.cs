using System.Text.RegularExpressions;

namespace TextPress;

/// <summary>
/// Provides a factory to create and retrieve <see cref="StringTemplate"/> instances.
/// </summary>
/// <remarks>
///	This class should be instantiated once per application (as a singleton),
/// as it's intended to bootstrap creation and retrieval of <see cref="StringTemplate"/> instances.
/// </remarks>
public class StringTemplateFactory
{
	/// <summary>
	/// Provides a registry for instantiated <see cref="StringTemplate"/> instances.
	/// </summary>
	protected IDictionary<string, StringTemplate?> Templates { get; } = new Dictionary<string, StringTemplate?>();

	/// <summary>
	/// Default options applied to all <see cref="StringTemplate"/> instances created by this factory,
	/// when no options are explicitly provided.
	/// </summary>
	public static StringTemplateOptions DefaultOptions { get; } = new()
	{
		EscapingStyle = VariableEscapingStyle.None,
		RegexOptions = RegexOptions.Compiled
	};
	
	/// <summary>
	/// Default constructor
	/// </summary>
	public StringTemplateFactory() { }

	/// <summary>
	/// Gets an existing <see cref="StringTemplate"/> instance from the factory, or creates a new one.
	/// </summary>
	/// <param name="name">Name of the template</param>
	/// <param name="options">Options to create the template, if it doesn't exist.</param>
	public StringTemplate GetTemplate(string? name, StringTemplateOptions? options = null)
	{
		name ??= "";
		
		if (!Templates.TryGetValue(name, out StringTemplate? template))
		{
			// Initialize a new StringTemplate instance and add it to the known templates.
			// If no options are provided, use the default options.
			
			template = new(options ?? DefaultOptions);
			Templates.Add(name, template);
		}

		return template!;
	}
}