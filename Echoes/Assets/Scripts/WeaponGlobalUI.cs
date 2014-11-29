using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponGlobalUI : MonoBehaviour {

    private Text ammoNumber;
    private Image currentGunIcon;
    private List<Image> ammoImageList;
    public Image bulletSpriteObject;
    private List<GameObject> weaponBulletUI;

    void Start()
    {
        ammoNumber = gameObject.GetComponentInChildren<Text>();
        currentGunIcon = gameObject.GetComponentsInChildren<Image>()[1];
        ammoImageList = new List<Image>();
    }

    private void changeCurrentGunIcon(Sprite gunIconSprite)
    {
        currentGunIcon.sprite = gunIconSprite;
    }

    public void updateNumberOfAmmo(int newAmmoNumber)
    {
        ammoNumber.text = newAmmoNumber.ToString();
    }

    public void createWeaponUI(int magCapacity, int bulletInMag, int numberOfLine, float distanceBetweenAmmo, Vector2 startingPoint, Sprite bulletSprite, Sprite gunIconSprite)
    {
         
        
        // Loading bullets
        int bulletPerLine = magCapacity /numberOfLine;
        Vector3 instantiationPosition = new Vector3(startingPoint.x, startingPoint.y, 0);

        // Creating an empty gameObj
        //GameObject emptyGameObject = Instantiate(, instantiationPosition, gameObject.transform.rotation) as GameObject;

        for (int i = 1; i <= magCapacity; ++i)
        {
            Image curBullet = Instantiate(bulletSpriteObject) as Image;
            curBullet.rectTransform.position = instantiationPosition;
            curBullet.sprite = bulletSprite;
            curBullet.transform.parent = this.transform;

            if (i <= bulletInMag)
                curBullet.color = Color.white;
            

            if (i % bulletPerLine == 0)
            {
                instantiationPosition.y += 10;
                instantiationPosition.x = startingPoint.x;
            }
            else
                instantiationPosition.x += distanceBetweenAmmo;

            ammoImageList.Add(curBullet);
        }
    }

}
