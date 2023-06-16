using System.Diagnostics;
using Utilities;

Console.WriteLine(StringUtilities.ToWords("Marcin Jagiela"));
Console.WriteLine(StringUtilities.ToWords("The lord of the rings"));

Console.WriteLine(StringUtilities.ToSentence("marcin jagiela"));
Console.WriteLine(StringUtilities.ToSentence("the LorD OF thE Rings"));

Console.WriteLine(StringUtilities.ToPascalCase("marcin jagiela"));
Console.WriteLine(StringUtilities.ToPascalCase("the LorD OF thE Rings"));

var joined = new List<string> { "First", "Second", "Third" }.JoinWith(", ");
Console.WriteLine(joined);

var name = new Optional<string>("Marcin");
Console.WriteLine(name.HasValue); //Should display true
Console.WriteLine(name); //Should display Marcin
Console.WriteLine(name.Value); //Should display Marcin
var name2 = new Optional<string>(null);
Console.WriteLine(name2.HasValue); //Should display false
Console.WriteLine(name2); //Should display nothing
Console.WriteLine(name2.Value); //Should throw an exception





