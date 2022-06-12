/*
 * Scene Management
 *
 * Description: Provides simple calls for scene management. 
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-12
*/

namespace PushForward.Base
{
	#region using
	using ExtensionMethods;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	#endregion // using

	public class SceneManagement : SingletonBehaviour<SceneManagement>
	{
		#region object activation
		[Tooltip("Set the objects to deactivate on load.")]
		[SerializeField] private GameObject[] objectsToDeactivateOnLoad;
		[Tooltip("Set the objects to activate on load.")]
		[SerializeField] private GameObject[] objectsToActivateOnLoad;
		[Tooltip("Set the objects to deactivate on unload.")]
		[SerializeField] private GameObject[] objectsToDeactivateOnUnload;
		[Tooltip("Set the objects to activate on unload.")]
		[SerializeField] private GameObject[] objectsToActivateOnUnload;

		public void SetActiveObjectsOnLoad()
		{
			this.objectsToDeactivateOnLoad.DoForEach(go => go.Deactivate());
			this.objectsToActivateOnLoad.DoForEach(go => go.Activate());
		}

		public void SetActiveObjectsOnUnload()
		{
			this.objectsToActivateOnUnload.DoForEach(go => go.Activate());
			this.objectsToDeactivateOnUnload.DoForEach(go => go.Deactivate());
		}
		#endregion // object activation

		#region scene loading
		private bool unloading;

		public void ReloadScene()
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

			this.unloading = true;

			this.SetActiveObjectsOnUnload();

			//SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

			SceneManager.LoadScene(currentSceneIndex);
		}

		private void SwipeScenes(bool forwards)
		{
			int totalScenes = SceneManager.sceneCountInBuildSettings;
			int sceneIndex = SceneManager.GetActiveScene().buildIndex;

			this.unloading = true;

			this.SetActiveObjectsOnUnload();

			sceneIndex = forwards ? sceneIndex.CircleAdd(1, 0, totalScenes - 1) : sceneIndex.CircleSubtract(1, 0, totalScenes - 1);

			SceneManager.LoadScene(sceneIndex);
		}

		public void MouseDragToSceneSwipe(Vector2 drag)
		{
			this.SwipeScenes(Vector2.Dot(drag, Vector2.right).Positive());
		}
		#endregion

		#region engine
		private void Awake()
		{
			this.SetInstance(this);
			this.SetActiveObjectsOnLoad();
		}

		private void OnDestroy()
		{
			if (!this.unloading)
			{ this.SetActiveObjectsOnUnload(); }
		}
		#endregion
	}
}
