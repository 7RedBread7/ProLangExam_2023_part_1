namespace Utilities;

public static class StringUtilities
{
	public static int ToWords(string sentence)
	{
		string[] words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		HashSet<string> uniqueWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		foreach (string word in words)
		{
			uniqueWords.Add(word);
		}

		return uniqueWords.Count;
	}

	public static string ToSentence(string sentence)
	{
		sentence = sentence.ToLower();
		return (char.ToUpper(sentence[0]) + sentence.Substring(1) + '.');
	}
	public static string ToPascalCase(string sentence)
	{
		string[] words = sentence.ToLower().Split(' ');
		for (int i = 0; i < words.Length; i++)
		{
			words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
		}
		return (string.Join("", words));
	}
	public static string JoinWith(this List<string> strings, string joinWith)
	{
		return (string.Join(joinWith, strings));
	}
}