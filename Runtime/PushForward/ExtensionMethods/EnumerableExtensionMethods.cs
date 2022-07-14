/*
 * EnumerableExtensionMethods
 *
 * Description: A collection of useful methods for C# System collection classes to enhance functionality.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-12
*/

#region using
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>Helper methods for generic collections</summary>
// ReSharper disable once CheckNamespace - Needs to be available everywhere
public static class EnumerableExtensionMethods
{
	#region do for each
	/// <summary>Uses a given action on every item in the enumerable.</summary>
	/// <typeparam name="T">Any object or primitive.</typeparam>
	/// <param name="enumerable">The enumerable to work on.</param>
	/// <param name="action">The action to perform.</param>
	/// <returns>The given reference to the enumerable for extended processing.</returns>
	public static IEnumerable<T> DoForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		if (action == null)
		{ throw new ArgumentException("Action can't be null", nameof(action)); }

		if (enumerable == null)
		{ throw new ArgumentException("Array can't be null", nameof(enumerable)); }

		IEnumerable<T> doForEach = enumerable as T[] ?? enumerable.ToArray();
		foreach (T enumerate in doForEach)
		{ action(enumerate); }
		return doForEach;
	}

	/// <summary>Uses a given action on every item in the array.</summary>
	/// <typeparam name="T">Any object or primitive.</typeparam>
	/// <param name="array">The array to work on.</param>
	/// <param name="action">The action to perform.</param>
	/// <param name="backwards">To iterate on the array backwards</param>
	/// <returns>The given reference to the array for extended processing.</returns>
	public static T[] DoForEach<T>(this T[] array, Action<T, int> action, bool backwards = false)
	{
		if (action == null)
		{ throw new ArgumentException("Action can't be null", nameof(action)); }

		if (array == null)
		{ throw new ArgumentException("Array can't be null", nameof(array)); }

		int start = backwards ? array.Length - 1 : 0;
		int end = backwards ? 0 : array.Length;
		for (int index = start; index < end; index++)
		{ action(array[index], index); }

		return array;
	}

	/// <summary>Uses a given action on every item in the list.</summary>
	/// <typeparam name="T">Any object or primitive.</typeparam>
	/// <param name="list">The list to work on.</param>
	/// <param name="action">The action to perform.</param>
	/// <param name="backwards">To iterate on the array backwards</param>
	/// <returns>The given reference to the list for extended processing.</returns>
	public static List<T> DoForEach<T>(this List<T> list, Action<T, int> action, bool backwards = false)
	{
		if (action == null)
		{ throw new ArgumentException("Action can't be null", nameof(action)); }

		if (list == null)
		{ throw new ArgumentException("List can't be null", nameof(list)); }

		int start = backwards ? list.Count - 1 : 0;
		int end = backwards ? 0 : list.Count;
		for (int index = start; index < end; index++)
		{ action(list[index], index); }

		return list;
	}
	public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> func)
		=> enumerable.Select(func);
	#endregion // do for each

	#region find
	/// <summary>Searches the enumerable for an item that fits the predicate and returns it. If cannot find it, returns the given default item.</summary>
	/// <typeparam name="T">Any object or primitive.</typeparam>
	/// <param name="enumerable">The enumerable to work on.</param>
	/// <param name="predicate">The predicate to test on an item.</param>
	/// <param name="defaultItem">The item that will return if enumerable contains no item that fits predicate.</param>
	/// <returns>The first item that fits the predicate or defaultItem if none found.</returns>
	public static T FindFirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, T defaultItem = default(T))
	{
		foreach (T enumerate in enumerable)
		{
			if (predicate(enumerate))
			{ return enumerate; }
		}

		return defaultItem;
	}

	/// <summary>Searches the enumerable for an item that fits the predicate and returns it. If cannot find it, returns the default for T.</summary>
	/// <typeparam name="T">Any object or primitive.</typeparam>
	/// <param name="enumerable">The enumerable to work on.</param>
	/// <param name="predicate">The predicate to test on an item.</param>
	/// <returns>The first item that fits the predicate or the default for T if none found.</returns>
	public static T FindFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
	{
		return enumerable.FindFirstOrDefault(predicate);
	}
	#endregion // find

	#region general
	public static Dictionary<TKey, TValue> ChainClear<TKey, TValue>(this Dictionary<TKey, TValue> enumerable)
	{
		enumerable.Clear();
		return enumerable;
	}

	/// <summary>Returns the last element of an array.</summary>
	/// <typeparam name="T">The array element type.</typeparam>
	/// <param name="array">The array in question</param>
	/// <returns>The last element of the array.</returns>
	public static T Last<T>(this T[] array) => array[^1];

	/// <summary>Clones an existing dictionary, producing a new copy of it.</summary>
	/// <typeparam name="T1">First Type.</typeparam>
	/// <typeparam name="T2">Second Type.</typeparam>
	/// <returns>A copy of the calling dictionary.</returns>
	public static Dictionary<T1, T2> Clone<T1, T2>(this Dictionary<T1, T2> dict)
	{
		Dictionary<T1, T2> newDictionary = new Dictionary<T1, T2>();
		dict.DoForEach(kvp => newDictionary.Add(kvp.Key, kvp.Value));
		return newDictionary;
	}

	/// <summary>Add item to list with chaining.</summary>
	/// <typeparam name="T">The type of items in this list.</typeparam>
	/// <param name="list">The list to add to.</param>
	/// <param name="newItem">The new item to add</param>
	/// <param name="distinct">Whether to make sure the item added is unique in the list.</param>
	/// <returns>The original list with the new item.</returns>
	public static List<T> ChainAdd<T>(this List<T> list, T newItem, bool distinct = false)
	{
		if (distinct && list.Contains(newItem))
		{ return list; }

		list.Add(newItem);
		return list;
	}

	/// <summary>Adds an item to the list and makes sure it's distinct.</summary>
	/// <param name="list">The list to check.</param>
	/// <param name="newItem">The new item to add.</param>
	/// <returns>False if the list already contains the item, true otherwise.</returns>
	public static bool AddDistinct<T>(this List<T> list, T newItem)
	{
		if (list.Contains(newItem))
		{ return false; }
		list.Add(newItem);
		return true;
	}
	#endregion // general

	#region string representatioon
	/// <summary>A more convenient string representation for enumerables.</summary>
	/// <typeparam name="T">The enumerable type.</typeparam>
	/// <param name="enumerable">The enumerable to stringify.</param>
	/// <returns>A string describing the enumerable.</returns>
	public static string StringRepresentation<T>(this IEnumerable<T> enumerable)
	{
		if (enumerable == null)
		{ return "NULL"; }
		// ReSharper disable once PossibleMultipleEnumeration -- irrelevant as it's only checking for empty
		if (!enumerable.Any())
		{ return "[]"; }

		StringBuilder stringRepresentationBuilder = new StringBuilder("[ ");

		// ReSharper disable once PossibleMultipleEnumeration -- also order doesn't really matter.
		foreach (T item in enumerable)
		{ stringRepresentationBuilder.Append(item).Append(", "); }

		stringRepresentationBuilder.RemoveCharactersFromEnd(2).Append(" ]");

		return stringRepresentationBuilder.ToString();
	}

	/// <summary>A more convenient string representation for Vector3s.</summary>
	/// <param name="vec3">The Vector3 to stringify.</param>
	/// <param name="decimalAccuracy">How much decimal accuracy to use. (default = 4)</param>
	/// <returns>A string describing the Vector3</returns>
	public static string StringRepresentation(this Vector3 vec3, int decimalAccuracy = 4)
	{
		string n = $"N{decimalAccuracy}";
		return $"({vec3.x.ToString(n)}, {vec3.y.ToString(n)}, {vec3.z.ToString(n)})";
	}

	public static string StringRepresentation(this Vector2 vec2, int decimalAccuracy = 4)
	{
		string n = $"N{decimalAccuracy}";
		return $"({vec2.x.ToString(n)}, {vec2.y.ToString(n)})";
	}

	public static string StringRepresentation(this Quaternion quaternion, int decimalAccuracy = 4)
	{
		return quaternion.eulerAngles.StringRepresentation(decimalAccuracy);
	}
	#endregion // string representation
}
