using UnityEngine;
using System.Collections;

public class CameraController : CachedBase {

    private Transform targetTransform;
    private Vector3 targetPosition;
    public Vector3 forwardZoom;

	// Use this for initialization
	void Start () {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        transform.position = targetPosition;

        forwardZoom = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        targetPosition = new Vector3(targetTransform.position.x + forwardZoom.x, transform.position.y, targetTransform.position.z + forwardZoom.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 8);
	}
}
