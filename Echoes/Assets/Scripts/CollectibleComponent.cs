using UnityEngine;
using System.Collections;

public class CollectibleComponent : CachedBase {

    public enum CollectibleType
    {
        MEDKIT,
        PISTOL_AMMO,
        SHOTGUN_AMMO,
        RIFLE_AMMO
    };

    public float rotationSpeed = 4f;
    public int ammoQuantity;
    private Vector3 startPosition;
    public CollectibleType typeOfCollectible;

    // Use this for initialization
	void Start () {
        startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(this.transform.position.x, startPosition.y + (Mathf.Sin(Time.time) * 0.3f), this.transform.position.z);
        this.transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, transform.eulerAngles.y + rotationSpeed, rotationSpeed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player") {
            Player playerEntity = other.GetComponent<Player>();
            
            if (this.typeOfCollectible == CollectibleType.MEDKIT)
                playerEntity.GetHeal(10);
            else
                playerEntity.GetAmmo((int)typeOfCollectible - 1, ammoQuantity);

            Destroy(gameObject);
        }
    }
}
