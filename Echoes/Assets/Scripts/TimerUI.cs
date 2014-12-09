using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimerUI : MonoBehaviour {

    private Text timerText;
    private float time;
    private bool countTime = true;

	// Use this for initialization
	void Start () {
        timerText = GetComponent<Text>();
        StartCoroutine(StartCountTime());
	}

    IEnumerator StartCountTime()
    {
        time = 0;
        while (countTime)
        {
            //Debug.Log("TIMER countdown  time = " + Time.time + " and instruction = " + timerText.text);
            yield return new WaitForEndOfFrame();

            timerText.text = TimeToString();
            time += Time.deltaTime;
        }

    }

    String TimeToString()
    {
        int minutes = (int)Mathf.Floor(time / 60);
        int seconds = (int)(time % 60);
        int fraction = (int)((time * 100) % 100);

        return String.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
    }
}
