using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : CachedBase {

    public float rotationSpeed = 450;
    public float walkSpeed = 8;
    public float runSpeed = 24;

    private CharacterController controller;
    private Quaternion targetRotation;
    

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        bool isRunning = Input.GetButton("Run");

        if (input != Vector3.zero)
        {
            // Smooth rotation
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        Vector3 motion = input.normalized;

        // Stick player to ground
        motion += Vector3.up * -5;

        Vector3 normalizedMotion = motion.normalized;

        if (isRunning)
            motion *= runSpeed;
        else
            motion *= walkSpeed;

        controller.Move(motion * Time.deltaTime);
	}
}
