/*
 * TextExtensionMethods
 *
 * Description: A collection of useful methods for C# System text classes to enhance functionality.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-12 
*/

// ReSharper disable once CheckNamespace - needs to be part of the text namespace
namespace System.Text
{
	public static class TextExtensionMethods
	{
		/// <summary>Checks if the string is null or empty.</summary>
		/// <param name="str">The string to check.</param>
		/// <returns>True if the string is null or empty, false otherwise.</returns>
		public static bool IsNullOrEmpty(this string str) => String.IsNullOrEmpty(str);

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
