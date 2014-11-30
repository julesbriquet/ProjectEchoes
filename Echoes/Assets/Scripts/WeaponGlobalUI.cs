using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponGlobalUI : MonoBehaviour {

    private Text ammoNumber;
    private Image currentGunIcon;

    // Display gray bullet
    public Image bulletEmptyLine1;
    // Display white bullet
    public Image bulletInMagLine1;

    // Display gray bullet
    public Image bulletEmptyLine2;
    // Display white bullet
    public Image bulletInMagLine2;


    private List<GameObject> weaponBulletUI;

    void Start()
    {
        ammoNumber = gameObject.GetComponentInChildren<Text>();
        
        Image[] gunImageUIList = gameObject.GetComponentsInChildren<Image>();
        currentGunIcon = gunImageUIList[1];
        bulletEmptyLine1 = gunImageUIList[2];
        bulletInMagLine1 = gunImageUIList[3];

        bulletEmptyLine2 = gunImageUIList[4];
        bulletInMagLine2 = gunImageUIList[5];


    }

    public void reloadWeaponUI(int newAmmoNumber, int numberOfLine, int magCapacity, int bulletInMag)
    {
        updateAmmoInMagUI(numberOfLine, magCapacity, bulletInMag);
        updateTotalAmmo(newAmmoNumber);
    }

    public void updateTotalAmmo(int newAmmoNumber)
    {
        ammoNumber.text = newAmmoNumber.ToString();
    }

    public void updateAmmoInMagUI(int numberOfLine, int magCapacity, int bulletInMag)
    {
        int bulletPerLine = magCapacity / numberOfLine;

        Rect currentSpriteRect = bulletInMagLine1.sprite.rect;

        if (bulletInMag > bulletPerLine)
            bulletInMagLine1.rectTransform.sizeDelta = new Vector2(bulletPerLine * currentSpriteRect.width, currentSpriteRect.height);
        else
            bulletInMagLine1.rectTransform.sizeDelta = new Vector2(bulletInMag * currentSpriteRect.width, currentSpriteRect.height);

        bulletInMagLine2.rectTransform.sizeDelta = new Vector2((bulletInMag - bulletPerLine) * currentSpriteRect.width, currentSpriteRect.height);
    }

    public void changeWeaponUI(int magCapacity, int bulletInMag, int numberOfLine, Vector2 startingPoint, Sprite bulletSprite, Sprite gunIconSprite, float spriteScale)
    {
        currentGunIcon.sprite = gunIconSprite;


        int bulletPerLine = magCapacity / numberOfLine;
        Vector3 startingPosition = new Vector3(startingPoint.x, startingPoint.y, 0);

        // Set position
        bulletEmptyLine1.rectTransform.anchoredPosition = startingPosition;
        bulletInMagLine1.rectTransform.anchoredPosition = startingPosition;

        // Set scale
        bulletEmptyLine1.rectTransform.localScale = new Vector3(spriteScale, spriteScale, 1);
        bulletInMagLine1.rectTransform.localScale = new Vector3(spriteScale, spriteScale, 1);

        // Set sprite
        bulletEmptyLine1.sprite = bulletSprite;
        bulletInMagLine1.sprite = bulletSprite;

        // Set image rectangle size depending on number of munition (sprite will be repeated)
        bulletEmptyLine1.rectTransform.sizeDelta = new Vector2(bulletPerLine * bulletSprite.rect.width, bulletSprite.rect.height);

        if (bulletInMag > bulletPerLine)
            bulletInMagLine1.rectTransform.sizeDelta = new Vector2(bulletPerLine * bulletSprite.rect.width, bulletSprite.rect.height);
        else    
            bulletInMagLine1.rectTransform.sizeDelta = new Vector2(bulletInMag * bulletSprite.rect.width, bulletSprite.rect.height);

        if (numberOfLine == 2)
        {
            // Set scale
            bulletEmptyLine2.rectTransform.localScale = new Vector3(spriteScale, spriteScale, 1);
            bulletInMagLine2.rectTransform.localScale = new Vector3(spriteScale, spriteScale, 1);

            startingPosition.y -= (bulletSprite.rect.height * bulletInMagLine2.rectTransform.localScale.y);

            // Set position
            bulletEmptyLine2.rectTransform.anchoredPosition = startingPosition;
            bulletInMagLine2.rectTransform.anchoredPosition = startingPosition;

            // Set sprite
            bulletEmptyLine2.sprite = bulletSprite;
            bulletInMagLine2.sprite = bulletSprite;

            // Set image rectangle size depending on number of munition (sprite will be repeated)
            bulletEmptyLine2.rectTransform.sizeDelta = new Vector2(bulletPerLine * bulletSprite.rect.width, bulletSprite.rect.height);

            bulletInMagLine2.rectTransform.sizeDelta = new Vector2((bulletInMag - bulletPerLine) * bulletSprite.rect.width, bulletSprite.rect.height);
        }
        else
        {
            bulletEmptyLine2.rectTransform.sizeDelta = new Vector2(-1, -1);
            bulletInMagLine2.rectTransform.sizeDelta = new Vector2(-1, -1);
        }
    }

}
