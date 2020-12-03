/*
	TextExtensionMethos
	
	Description: A collection of useful methods for C# System text classes to enhance functionality.
	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2017-03-31
*/

namespace System.Text
{
	public static class TextExtensionMethods
	{
		/// <summary>Remove characters from the end of a StringBuilder.</summary>
		/// <param name="stringBuilder">The StringBuilder to remove from.</param>
		/// <param name="numberOfCharacters">The number of characters to remove.</param>
		/// <returns>The same StringBuilder with the characters removed.</returns>
		public static StringBuilder RemoveCharactersFromEnd(this StringBuilder stringBuilder, int numberOfCharacters)
		{
			stringBuilder.Remove(stringBuilder.Length - numberOfCharacters, numberOfCharacters);
			return stringBuilder;
		}
	}
}
