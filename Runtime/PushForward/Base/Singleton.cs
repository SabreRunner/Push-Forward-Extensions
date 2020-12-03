using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour : BaseMonoBehaviour
{
	public static SingletonBehaviour Instance { get; private set; }

	protected void SetInstance()
	{
		if (SingletonBehaviour.Instance != null && SingletonBehaviour.Instance != this)
		{
			this.DestroyThisBehaviour();
			return;
		}

		SingletonBehaviour.Instance = this;
	}
}
