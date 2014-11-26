using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

    private float lifeTime = 5;
    private Material mat;
    private Color originalColor;
    private float fadePercent;

    private float deathTime;
	// Use this for initialization
	void Start () {
        mat = renderer.material;
        originalColor = mat.color;

        StartCoroutine("Fade", -lifeTime);
        Destroy(gameObject, lifeTime);
	}

    IEnumerator Fade(float fadingDuration)
    {
        bool fadingOut = (fadingDuration < 0.0f);
        float fadingOutSpeed = 1.0f / fadingDuration;

        // grab all child objects
        Renderer[] rendererObjects = this.gameObject.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rendererObjects.Length; i++)
        {
            rendererObjects[i].enabled = true;
        }

        float currentAlphaValue = 0f;

        if (fadingOut)
            currentAlphaValue = 1f;

        // iterate to change alpha value 
        while ((currentAlphaValue >= 0.0f && fadingOut) || (currentAlphaValue <= 1.0f && !fadingOut))
        {
            currentAlphaValue += Time.deltaTime * fadingOutSpeed;
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                Color newColor = rendererObjects[i].material.color;

                newColor.a = currentAlphaValue;
                newColor.a = Mathf.Clamp(newColor.a, 0.0f, 1.0f);
                rendererObjects[i].material.color = newColor;
            }

            yield return null;
        }

        // turn objects off after fading out
        if (fadingOut)
        {
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                rendererObjects[i].enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Ground")
            rigidbody.Sleep();
    }
}
