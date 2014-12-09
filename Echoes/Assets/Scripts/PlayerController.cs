using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : CachedBase {

    public float rotationSpeed = 450;
    public float walkSpeed = 15;

    private CharacterController controller;
    private Quaternion targetRotation;
    private Camera attachedCamera;
    private CameraController attachedCameraControl;
    public bool isGamepadConnected = false;

    private Player player;

    private Image sightImage;

    // Time until teleport
    private float teleportSecondCount;

    public bool enablePlayerControl = true;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        attachedCamera = Camera.main;
        attachedCameraControl = attachedCamera.GetComponent<CameraController>();
        player = GetComponent<Player>();

        sightImage = GameObject.FindGameObjectWithTag("SightWeapon").GetComponent<Image>();
        Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {

        isGamepadConnected = Input.GetJoystickNames().Length > 0;

        if (enablePlayerControl)
        {
            ControlMovements();
            ControlAim();

            ControlGun();
            ControlZoom();
            ControlTeleport();

            // Change sight position
            //Vector3 gunScreenPosition = attachedCamera.WorldToScreenPoint(player.currentGun.shootOrigin.position + (player.currentGun.shootRange * transform.forward));
            //sightImage.rectTransform.anchoredPosition = new Vector2(gunScreenPosition.x, gunScreenPosition.y);
        }
    }



    void ControlMovements()
    {
        Vector3 inputLeftStick = Vector3.zero;

        if (isGamepadConnected)
            inputLeftStick = new Vector3(Input.GetAxisRaw("L_XAxis_1"), 0, Input.GetAxisRaw("L_YAxis_1"));
        else
            inputLeftStick = new Vector3(Input.GetAxisRaw("HorizontalKeyBoard"), 0, Input.GetAxisRaw("VerticalKeyBoard"));
        


        Vector3 motion = inputLeftStick.normalized;

        // Stick player to ground
        motion += Vector3.up * -1;
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



        // CHANGE SIGHT POSITION
        if (Vector3.Distance(inputRightStick, player.currentGun.shootOrigin.position) < player.currentGun.shootRange)
        {
            sightImage.rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else
        {
            Vector3 gunScreenPosition = attachedCamera.WorldToScreenPoint(player.currentGun.shootOrigin.position + (player.currentGun.shootRange * transform.forward));
            sightImage.rectTransform.anchoredPosition = new Vector2(gunScreenPosition.x, gunScreenPosition.y);

            if (gunScreenPosition.y > Screen.height)
                sightImage.rectTransform.anchoredPosition = new Vector2(gunScreenPosition.x, Screen.height - 2);
            if (gunScreenPosition.y < 0)
                sightImage.rectTransform.anchoredPosition = new Vector2(gunScreenPosition.x, 2);
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

    void ControlZoom()
    {
        bool isZoomButtonOn = false;
        if (isGamepadConnected)
            isZoomButtonOn = Input.GetAxisRaw("TriggersL_1") > 0 ? true : false;
        else
            isZoomButtonOn = Input.GetButton("RunButton");

        if (isZoomButtonOn)
            attachedCameraControl.forwardZoom = transform.forward * 18;
        else
            attachedCameraControl.forwardZoom = Vector3.zero;
    }

    void ControlTeleport()
    {
        bool isTeleportButtonOn = false;

        if (isGamepadConnected)
            isTeleportButtonOn = Input.GetButton("LB_1");
        else
            isTeleportButtonOn = Input.GetButton("TeleportButton");

        if (isTeleportButtonOn)
        {
            // Check if on for 3 seconds
            if (teleportSecondCount < 2)
                teleportSecondCount += Time.deltaTime;
            else
            {
                // Do teleportation if button has been hold
                Debug.Log("POWER RANGERS TELEPORTATION!");
                GameObject teleportBallObj = GameObject.FindGameObjectWithTag("TeleportBall");

                // If a teleportation ball is in the map
                if (teleportBallObj)
                {
                    TeleportBall tpBall = teleportBallObj.GetComponent<TeleportBall>();
                    this.transform.position = tpBall.teleportPosition;
                }
                
                teleportSecondCount = 0;
            }

        }
        else
            teleportSecondCount = 0;

    }

    public bool IsSkipButtonPressed()
    {
        if (isGamepadConnected)
            return (Input.GetButtonDown("A_1"));
        else
            return Input.GetButtonDown("KeyBoardSkipButton");
    }
}
