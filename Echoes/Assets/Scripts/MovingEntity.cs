using UnityEngine;
using System.Collections;

public class MovingEntity : CachedBase {

    public float rotationSpeed = 4f;
    public float moveSpeed = 4f;
    private Vector3 startPosition;
    private Vector3 startAngle;

    public override void Awake()
    {
        base.Awake(); //does the caching.
        //Debug.Log ("Awake called!");
    }


    // Use this for initialization
    void Start()
    {
        startPosition = this.transform.position;
        startAngle = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveSpeed > 0)
            this.transform.position = new Vector3(startPosition.x + (Mathf.Sin(Time.time) * moveSpeed), this.transform.position.y, this.transform.position.z);

        if (rotationSpeed > 0)
            this.transform.eulerAngles = new Vector3(startAngle.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, transform.eulerAngles.y + rotationSpeed, rotationSpeed * Time.deltaTime), startAngle.z);
    }
}
