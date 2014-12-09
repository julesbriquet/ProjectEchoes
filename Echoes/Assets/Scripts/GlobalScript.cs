using UnityEngine;
using System.Collections;

public class GlobalScript : MonoBehaviour {

    public GameObject[] gameObjToActiveWhenGameFinished;
    public GameObject[] gameObjToDisableWhenGameFinished;

    public void FinishGame()
    {
        foreach (GameObject gameObjToActive in gameObjToActiveWhenGameFinished)
        {
            gameObjToActive.SetActive(true);
        }

        foreach (GameObject gameObjToDisable in gameObjToDisableWhenGameFinished)
        {
            gameObjToDisable.SetActive(false);
        }
    }
}
