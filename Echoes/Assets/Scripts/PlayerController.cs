using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : CachedBase {

    public float rotationSpeed = 450;
    public float walkSpeed = 8;
    public float runSpeed = 24;

    private CharacterController controller;
    private Quaternion targetRotation;
    private Camera attachedCamera;
    private bool isGamepadConnected = false;
    private GunSystem gunEntity;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        gunEntity = gameObject.GetComponentInChildren<GunSystem>();
        attachedCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        isGamepadConnected = Input.GetJoystickNames().Length > 0;
        ControlMovements();
        ControlAim();

        ControlGun();

	}



    void ControlMovements()
    {
        Vector3 inputLeftStick = Vector3.zero;

        if (isGamepadConnected)
            inputLeftStick = new Vector3(Input.GetAxisRaw("L_XAxis_1"), 0, Input.GetAxisRaw("L_YAxis_1"));
        else
            inputLeftStick = new Vector3(Input.GetAxisRaw("HorizontalKeyBoard"), 0, Input.GetAxisRaw("VerticalKeyBoard"));

        bool isRunning = false;
        if (isGamepadConnected)
            isRunning = Input.GetAxisRaw("TriggersL_1") > 0 ? true : false;
        else
            isRunning = Input.GetButton("RunButton");




        Vector3 motion = inputLeftStick.normalized;

        // Stick player to ground
        motion += Vector3.up * -5;


        if (isRunning)
            motion *= runSpeed;
        else
            motion *= walkSpeed;

        controller.Move(motion * Time.deltaTime);
    }

    void ControlAim()
    {
        Vector3 inputRightStick = Vector3.zero;
        

        if (isGamepadConnected)
            inputRightStick = new Vector3(Input.GetAxisRaw("R_XAxis_1"), 0, Input.GetAxisRaw("R_YAxis_1"));
        else
            inputRightStick = attachedCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, attachedCamera.transform.position.y - transform.position.y));

        if (inputRightStick != Vector3.zero)
        {
            // Rotation
            if (isGamepadConnected)
                targetRotation = Quaternion.LookRotation(inputRightStick);
            else
                targetRotation = Quaternion.LookRotation(inputRightStick - new Vector3(transform.position.x, 0, transform.position.z));

            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }
    }

    void ControlGun()
    {
        bool triggerShoot = false;

        if (isGamepadConnected)
            triggerShoot = Input.GetAxisRaw("TriggersR_1") > 0 ? true : false;
        else
            triggerShoot = Input.GetButton("ShootButton");

        if (triggerShoot)
        {
            gunEntity.Shoot();
            gunEntity.hasTriggerBeenRelease = false;
        }
        else
            gunEntity.hasTriggerBeenRelease = true;
    }
}
