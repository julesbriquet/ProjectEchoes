using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : LivingEntity {

    public GunEntity currentGun;
    public Transform hands;
    public GunEntity[] startingGuns;
    public List<GameObject> gunsInInventory;
    public int gunIndex = -1;

    public WeaponGlobalUI globalWeaponUI;

    void Start()
    {
        gunsInInventory = new List<GameObject>();

        for (int i = 0; i < startingGuns.Length; i++)
        {
            GunEntity gunEntityScript = Instantiate(startingGuns[i], hands.position - startingGuns[i].gunStockPosition.localPosition, hands.rotation) as GunEntity;
            GameObject currentGunObj = gunEntityScript.gameObject;
            currentGunObj.transform.parent = hands;
            currentGunObj.SetActive(false);
            gunsInInventory.Add(currentGunObj);

            startingGuns[i] = gunEntityScript;
        }
        EquipGun(0);

        globalWeaponUI = GameObject.FindGameObjectWithTag("WeaponUI").GetComponent<WeaponGlobalUI>();
    }

    public void GetAmmo(int weaponIndex, int ammoQuantity)
    {
        Debug.Log("Get ammo for idx" + weaponIndex + " qte: " + ammoQuantity);

        // If not equiped weapon
        startingGuns[weaponIndex].totalAmmo += ammoQuantity;

        // If currentWeapon => need to update UI
        if (weaponIndex == gunIndex)
            globalWeaponUI.updateTotalAmmo(currentGun.totalAmmo);
    }



    public void EquipGun(int weaponIndex)
    {
        if (currentGun)
            gunsInInventory[gunIndex].SetActive(false);

        gunIndex = weaponIndex % gunsInInventory.Count;
        currentGun = startingGuns[gunIndex];
        gunsInInventory[gunIndex].SetActive(true);


        globalWeaponUI.changeWeaponUI(currentGun.ammoPerMag, currentGun.ammoInMag, currentGun.gunUIEntity.numberOfLine, currentGun.gunUIEntity.startPosition, currentGun.gunUIEntity.bulletIcon, currentGun.gunUIEntity.gunIcon, currentGun.gunUIEntity.scale);
        globalWeaponUI.reloadWeaponUI(currentGun.totalAmmo, currentGun.gunUIEntity.numberOfLine, currentGun.ammoPerMag, currentGun.ammoInMag);
    }
}
