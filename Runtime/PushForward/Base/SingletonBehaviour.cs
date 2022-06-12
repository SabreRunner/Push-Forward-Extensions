/*
 * Singleton Behaviour
 *
 * Description: Facilitates singleton behavior for an inherited commponent.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 
*/
using UnityEngine;

/// <summary>Singleton per component.</summary>
/// <remarks>Inheritor needs to call for SetInstance with specific instead of generic.</remarks>
/// <typeparam name="T">The type of component you want to Singleton</typeparam>
// ReSharper disable once CheckNamespace
public class SingletonBehaviour<T> : MonoBehaviour where T : Component
{
	/// <summary>Return the component instance.</summary>
	public static T Instance { get; private set; }

	/// <summary>Save the instance of the component.</summary>
	/// <param name="component">The instance you want to singleton.</param>
	protected void SetInstance(T component)
	{
		if (SingletonBehaviour<T>.Instance != null && SingletonBehaviour<T>.Instance != component)
		{
			this.DestroyThisBehaviour();
			return;
		}

		// Save the component instance
		SingletonBehaviour<T>.Instance = component;
	}
}
