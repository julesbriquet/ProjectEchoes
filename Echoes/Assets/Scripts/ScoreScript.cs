using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public Text scoreToGet;

	// Use this for initialization
	void Start () {
        Text scoreDisplay = GetComponent<Text>();
        scoreDisplay.text = scoreToGet.text;
	}
	
}
