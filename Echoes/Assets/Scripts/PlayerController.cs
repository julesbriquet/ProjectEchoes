using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : CachedBase {

    public float rotationSpeed = 450;
    public float walkSpeed = 8;
    public float runSpeed = 24;

    private CharacterController controller;
    private Quaternion targetRotation;
    private Camera attachedCamera;
    private bool isGamepadConnected = false;

    private Player player;
    

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        attachedCamera = Camera.main;
        player = GetComponent<Player>();

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
        motion += Vector3.up * -1;


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
        if (player.currentGun)
        {

            /*
             * SHOOTING PART 
             */
            bool triggerShoot = false;

            if (isGamepadConnected)
                triggerShoot = Input.GetAxisRaw("TriggersR_1") > 0 ? true : false;
            else
                triggerShoot = Input.GetButton("ShootButton");

            if (triggerShoot)
            {
                player.currentGun.Shoot(player.globalWeaponUI);
                player.currentGun.hasTriggerBeenRelease = false;
            }
            else
                player.currentGun.hasTriggerBeenRelease = true;

            bool changeGun = false;


            /*
             * CHANGE WEAPON PART 
             */
            if (isGamepadConnected)
                changeGun = Input.GetButtonDown("RB_1");
            else
                changeGun = Input.GetAxis("ChangeWeaponButton") > 0 ? true : false;

            if (changeGun)
                player.EquipGun(player.gunIndex + 1);


            /*
             * RELOADING PART
             */
            bool reloadGun = false;
            if (isGamepadConnected)
                reloadGun = Input.GetButtonDown("X_1") && player.currentGun.CanReload();
            else
                reloadGun = Input.GetButtonDown("ReloadWeaponButton") && player.currentGun.CanReload();

            if (reloadGun)
                player.currentGun.ReloadWeapon(player.globalWeaponUI);

        }
    }
    
}
