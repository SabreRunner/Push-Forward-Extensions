/*
 * MathExtensionMethods
 *
 * Description: A collection of useful methods for math classes to enhance functionality.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-27
*/

// ReSharper disable once CheckNamespace
namespace PushForward.ExtensionMethods
{
	#region using
	using System;
	using UnityEngine;
	#endregion // using

	public static class MathExtensionMethods
	{
		#region max
		/// <summary>Maximum function for TimeSpan.</summary>
		/// <param name="timeSpan">First TimeSpan</param>
		/// <param name="other">Second TimeSpan</param>
		/// <returns>Biggest of the two.</returns>
		public static TimeSpan Max(this TimeSpan timeSpan, TimeSpan other)
		{ return timeSpan.Ticks > other.Ticks ? timeSpan : other; }
		/// <summary>Maximum function for TimeSpan.</summary>
		/// <param name="timeSpan">First TimeSpan</param>
		/// <param name="milliseconds">Milliseconds to  compare</param>
		/// <returns>Biggest of the two.</returns>
		public static TimeSpan Max(this TimeSpan timeSpan, double milliseconds)
		{ return timeSpan.TotalMilliseconds > milliseconds ? timeSpan : TimeSpan.FromMilliseconds(milliseconds); }
		#endregion // max

		#region between
		/// <summary>Make sure the number is between the low and high values.</summary>
		/// <param name="number">The number to check.</param>
		/// <param name="low">The low limit.</param>
		/// <param name="high">The high limit.</param>
		/// <param name="inclusive">Whether to include limits or not.</param>
		/// <returns>True if the number is between the values, false otherwise.</returns>
		public static bool Between(this int number, int low, int high, bool inclusive = true)
			=> inclusive ? number >= low && number <= high : number > low && number < high;

		/// <summary>Make sure the number is between the low and high values.</summary>
		/// <param name="number">The number to check.</param>
		/// <param name="low">The low limit.</param>
		/// <param name="high">The high limit.</param>
		/// <param name="inclusive">Whether to include limits or not.</param>
		/// <returns>True if the number is between the values, false otherwise.</returns>
		public static bool Between(this float number, float low, float high, bool inclusive = true)
			=> inclusive ? number >= low && number <= high : number > low && number < high;

		/// <summary>Make sure the number is between the low and high values.</summary>
		/// <param name="number">The number to check.</param>
		/// <param name="low">The low limit.</param>
		/// <param name="high">The high limit.</param>
		/// <param name="inclusive">Whether to include limits or not.</param>
		/// <returns>True if the number is between the values, false otherwise.</returns>
		public static bool Between(this long number, long low, long high, bool inclusive = true)
			=> inclusive ? number >= low && number <= high : number > low && number < high;

		/// <summary>Make sure the number is between the low and high values.</summary>
		/// <param name="number">The number to check.</param>
		/// <param name="low">The low limit.</param>
		/// <param name="high">The high limit.</param>
		/// <param name="inclusive">Whether to include limits or not.</param>
		/// <returns>True if the number is between the values, false otherwise.</returns>
		public static bool Between(this double number, double low, double high, bool inclusive = true)
			=> inclusive ? number >= low && number <= high : number > low && number < high;

		/// <summary>Make sure the vector is between the low and high values.</summary>
		/// <remarks>This is an algebraic function comparing inner values against each other, not vectors in 3D space.</remarks>
		/// <param name="vector">the vector to check</param>
		/// <param name="low">The low limit.</param>
		/// <param name="high">The high limit.</param>
		/// <param name="inclusive">Whether to include limits or not.</param>
		/// <returns>True if the vector is between the values, false otherwise.</returns>
		public static bool Between(this Vector2 vector, Vector2 low, Vector2 high, bool inclusive = true)
			=> inclusive ? vector.x >= low.x && vector.x <= high.x && vector.y >= low.y && vector.y <= high.y
							: vector.x > low.x && vector.x < high.x && vector.y > low.y && vector.y < high.y;
		#endregion // between

		#region within
		/// <summary>Check if a tested number is within a certain distance of this number.</summary>
		/// <param name="tested">Which number to test.</param>
		/// <param name="distance">The distance to check within.</param>
		/// <param name="target">The target to check around.</param>
		/// <returns>True if <see cref="tested"/> is within <see cref="distance"/> of <see cref="target"/></returns>
		public static bool Within(this float tested, float distance, float target)
			=> tested.Between(target - distance, target + distance);
		#endregion // within

		#region round
		/// <summary>Rounds the double to the nearest integer.</summary>
		/// <param name="number">The number to round.</param>
		/// <returns>The rounded number.</returns>
		public static int RoundToInt(this double number) => (int)Math.Round(number, MidpointRounding.ToEven);

		/// <summary>Rounds the float to the nearest integer.</summary>
		/// <param name="number">The number to round.</param>
		/// <returns>The rounded number.</returns>
		public static int RoundToInt(this float number) => Mathf.RoundToInt(number);

		/// <summary>Floors the float to the nearest integer.</summary>
		/// <param name="number">The number to floor.</param>
		/// <returns>The floored number.</returns>
		public static int Floor(this float number) => Mathf.FloorToInt(number);
		#endregion

		#region clamp
		/// <summary>Clamp an int to min and max.</summary>
		/// <param name="value">The number to clamp.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value</param>
		/// <returns>The clamped number.</returns>
		public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);

		/// <summary>Clamp an long to min and max.</summary>
		/// <param name="value">The number to clamp.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value</param>
		/// <returns>The clamped number.</returns>
		public static long Clamp(this long value, long min, long max)
			=> value > max ? max : value < min ? min : value;

		/// <summary>Clamp a float to min and max.</summary>
		/// <param name="value">The number to clamp.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value</param>
		/// <returns>The clamped number.</returns>
		public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);

		/// <summary>Clamp a double to min and max.</summary>
		/// <param name="value">The number to clamp.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value</param>
		/// <returns>The clamped number.</returns>
		public static double Clamp(this double value, double min, double max)
			=> value > max ? max : value < min ? min : value;

		/// <summary>Clamp a float to 0 and 1.</summary>
		/// <param name="value">The number to clamp.</param>
		/// <returns>The clamped number.</returns>
		public static float Clamp01(this float value) => Mathf.Clamp01(value);

		/// <summary>Clamp a double to 0 and 1.</summary>
		/// <param name="value">The number to clamp.</param>
		/// <returns>The clamped number.</returns>
		public static double Clamp01(this double value) => value.Clamp(0, 1);

		/// <summary>Clamp a Vector2 to min and max</summary>
		/// <param name="value">The Vector2 to clamp.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value</param>
		/// <returns>The clamped Vector2.</returns>
		public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
			=> new(value.x.Clamp(min.x, max.x), value.y.Clamp(min.y, max.y));

		/// <summary>Clamp a Vector3 to min and max</summary>
		/// <param name="value">The Vector3 to clamp.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value</param>
		/// <returns>The clamped Vector3.</returns>
		public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
			=> new(value.x.Clamp(min.x, max.x), value.y.Clamp(min.y, max.y), value.z.Clamp(min.z, max.z));
		#endregion

		#region abs
		/// <summary>Returns the absolute value of given value.</summary>
		/// <param name="value">The value to make absolute.</param>
		public static int Abs(this int value) => Mathf.Abs(value);
		/// <summary>Returns the absolute value of given value.</summary>
		/// <param name="value">The value to make absolute.</param>
		public static float Abs(this float value) => Mathf.Abs(value);
		/// <summary>Compares two floats.</summary>
		/// <remarks>The comparison is Epsilon based. The check is whether the difference between the two numbers is below a certain threshold called Epsilon (accessible in this class)</remarks>
		/// <returns>True if they are sufficiently close, false otherwise</returns>
		public static bool FloatEqual(this float value, float other)
			=> (value - other).Abs() < float.Epsilon;
		#endregion

		#region sign
		/// <summary>Checks if a float is positive.</summary>
		/// <param name="value">The float to check.</param>
		/// <returns>false, if the value is negative and true the value is positive</returns>
		/// <remarks>float cannot be definitively pinned down as equal to exactly 0f.</remarks>
		public static bool IsPositive(this float value) => Mathf.Sign(value) > 0f;
		#endregion // sign

		#region circle add
		/// <summary>Adds in a circle from minimum to maximum range and back around.</summary>
		/// <remarks>Do not use this for subtraction!</remarks>
		/// <param name="value">The base value.</param>
		/// <param name="addition">How much to add.</param>
		/// <param name="minimum">The minimum of the range.</param>
		/// <param name="maximum">The maximum of the range.</param>
		/// <returns></returns>
		public static int CircleAdd(this int value, int addition, int minimum, int maximum)
		{
			if (addition < 0)
			{
				Debug.LogWarning("Addition is negative. CircleAdd is used for adding. Use CircleSubtract for Subtraction");
				return value;
			}

			// start by adding
			value += addition;
			// if it crosses the maximum, grab the remainder and add to the start
			if (value > maximum)
			{
				// this is so the first step after maximum will be minimum
				int remainder = value - (maximum + 1);
				value = minimum + remainder;
			}

			return value;
		}

		/// <summary>Subtracts in a circle from maximum to minimum range and back around.</summary>
		/// <remarks>The subtracting value should still be positive!</remarks>
		/// <param name="value">The base value.</param>
		/// <param name="subtraction">How much to subtract.</param>
		/// <param name="minimum">The minimum of the range.</param>
		/// <param name="maximum">The maximum of the range.</param>
		/// <returns></returns>
		public static int CircleSubtract(this int value, int subtraction, int minimum, int maximum)
		{
			if (subtraction < 0)
			{
				Debug.LogWarning("Subtraction should still be a positive integer.");
				return value;
			}

			// start by subtracting
			value -= subtraction;

			if (value < minimum)
			{
				int remainder = value - (minimum - 1);
				value = maximum - remainder.Abs();
			}

			return value;
		}
		#endregion // circle add

		#region transforms
		#region breakdown
		/// <summary>Convert a Quaternion to a Vector4 of values.</summary>
		public static Vector4 ToVector4(this Quaternion quaternion) => new(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
		/// <summary>Convert a Vector3 to a tuple of floats.</summary>
		public static (float, float, float) Breakdown(this Vector3 vec3) => (vec3.x, vec3.y, vec3.z);
		/// <summary>Convert a Quaternion to a tuple of floats.</summary>
		public static (float, float, float, float) Breakdown(this Quaternion quaternion) => (quaternion.x, quaternion.y, quaternion.z, quaternion.w);
		#endregion // breakdown
		/// <summary>Get the absolute distance between the 'from' vector to the 'to' vector.</summary>
		public static float DistanceTo(this Vector3 from, Vector3 to) => Vector3.Distance(from, to);
		/// <summary>Get the rotation between the 'from' rotation to the 'to' rotation.</summary>
		public static Quaternion RotationTo(this Quaternion from, Quaternion to)
		{
			Quaternion rotation = Quaternion.identity;
			rotation.SetFromToRotation(from.eulerAngles, to.eulerAngles);
			return rotation;
		}
		#endregion // transforms
	}
}
