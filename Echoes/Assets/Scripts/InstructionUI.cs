using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InstructionUI : MonoBehaviour {

    public Sprite[] spriteList;
    public int indexSpriteList;

    public Sprite nextButtonSpriteKeyBoard;
    public Sprite nextButtonSpritePad;
    private Image currentInstructionImage;
    private Image nextButtonImage;
    public PlayerController playerControl;

    public GameObject[] objectListToActiveAfterInstruction;

	// Use this for initialization
	void Start () {

        if (spriteList.Length > 0)
        {
            currentInstructionImage = this.GetComponent<Image>();
            nextButtonImage = this.gameObject.GetComponentsInChildren<Image>()[1];
            playerControl.enablePlayerControl = false;

            indexSpriteList = 0;
            currentInstructionImage.sprite = spriteList[indexSpriteList];
            
            
            if (playerControl.isGamepadConnected)
                nextButtonImage.sprite = nextButtonSpritePad;
            else
                nextButtonImage.sprite = nextButtonSpriteKeyBoard;
         }
	}
	
	// Update is called once per frame
	void Update () {
        if (playerControl.isGamepadConnected)
            nextButtonImage.sprite = nextButtonSpritePad;
        else
            nextButtonImage.sprite = nextButtonSpriteKeyBoard;
    
        if (playerControl.IsSkipButtonPressed()) {
            indexSpriteList++;
            // If end of instruction
            if (indexSpriteList >= spriteList.Length)
            {
                this.gameObject.SetActive(false);
                playerControl.enablePlayerControl = true;
                foreach (GameObject objectToActive in objectListToActiveAfterInstruction)
                {
                    objectToActive.SetActive(true);
                }
            }
            else
                currentInstructionImage.sprite = spriteList[indexSpriteList];
        }

    }
}
