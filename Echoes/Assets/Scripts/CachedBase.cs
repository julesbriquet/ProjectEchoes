using UnityEngine;
using System.Collections;

public class CachedBase : MonoBehaviour
{

		[HideInInspector]
		public new Rigidbody
				rigidbody;
		[HideInInspector]
		public new Transform
				transform;
		[HideInInspector]
		public new Camera
				camera;

		public virtual void Awake ()
		{
				transform = gameObject.transform;
				if (gameObject.rigidbody)
						rigidbody = gameObject.rigidbody;
				if (gameObject.camera)
						camera = gameObject.camera;
				//Debug.Log ("Caching complete.");
		}
}
