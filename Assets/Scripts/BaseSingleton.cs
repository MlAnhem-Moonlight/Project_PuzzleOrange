using UnityEngine;
using System;

namespace Hyb.Utils
{
	// public abstract class Singleton<T> where T : new()
	// {
	// 	private static T _instance;
	//
	// 	public static T Instance
	// 	{
	// 		get
	// 		{
	// 			if (_instance == null)
	// 			{
	// 				_instance = new T();
	// 			}
	// 			return _instance;
	// 		}
	// 	}
	// }

	/// <summary>
	/// Singleton for mono behavior object, only return exsited object, don't create new
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ManualSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
	{
		[SerializeField] bool dontDestroyOnload;
		private static T _instance;

		private static bool _applicationIsQuitting;

		public static T Instance
		{
			get
			{
				if (_applicationIsQuitting)
					return null;

				if (_instance == null)
				{
					//Debug.LogError("Cannot find Object with type " + typeof(T));
				}

				return _instance;
			}
		}

		public static bool IsInstanceValid()
		{
			return (_instance != null);
		}

		//MUST OVERRIDE AWAKE AT CHILD CLASS
		public virtual void Awake()
		{
			if (_instance != null)
			{
				Debug.LogWarning("Already has instance of " + typeof(T));
				GameObject.Destroy(this.gameObject);
				return;
			}

			if (_instance == null)
				_instance = (T)(MonoBehaviour)this;

			if (_instance == null)
			{
				Debug.LogError("Awake xong van NULL " + typeof(T));
			}
			//Debug.LogError("Awake of " + typeof(T));
			if (dontDestroyOnload)
				DontDestroyOnLoad(this);
		}

		protected virtual void OnDestroy()
		{
			//self destroy?
			if (_instance == this)
			{
				_instance = null;
				//Debug.LogError ("OnDestroy " + typeof(T));
			}
		}


		private void OnApplicationQuit()
		{
			_applicationIsQuitting = true;
		}
	}
}