using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class GunEntity : MonoBehaviour {

    public enum GunType
    {
        SemiAuto,
        Burst,
        Auto
    }

    // Cast Shoot Ray Part
    public Transform shootOrigin;
    public Transform gunStockPosition;
    private LineRenderer rayTracer;

    // Handle differents type of gun
    public GunType typeOfGun;
    public float roundPerMinute;
    public bool hasTriggerBeenRelease = true;
    private float secondsBetweenShoots;
    private float nextPossibleShoot;

    // Munition system
    public int ammoInStock;
    public int ammoByStock;
    public int ammoAmount;
    public float reloadingTime;

    // Handle audio
    private AudioSource audioEntity;

    // Handling Shell Ejection
    public Transform shellOrigin;
    public Rigidbody shellBody;

    // Handling damage
    public LayerMask collisionMask;
    public int weaponDamage;

    // Use this for initialization
    void Start()
    {
        secondsBetweenShoots = 60 / roundPerMinute;
        nextPossibleShoot = 0;
        audioEntity = GetComponent<AudioSource>();
        rayTracer = GetComponent<LineRenderer>();
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            Ray ray = new Ray(shootOrigin.position, shootOrigin.forward);
            RaycastHit hit;

            float shotDistance = 20;
            audioEntity.Play();
            if (Physics.Raycast(ray, out hit, shotDistance, collisionMask))
            {
                shotDistance = hit.distance;

                LivingEntity livingEntityTouched = hit.collider.GetComponent<LivingEntity>();
                if (livingEntityTouched)
                    livingEntityTouched.TakeDamage(weaponDamage);
            }

            // Compute time for enabling next shot (Mode Auto Only)
            nextPossibleShoot = Time.time + secondsBetweenShoots;

            // Draw Ray in Game
            if (rayTracer)
                StartCoroutine("RenderTracer", ray.direction * shotDistance);

            // Draw Ray in Debug
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 2);

            Rigidbody newShell = Instantiate(shellBody, shellOrigin.position, Quaternion.identity) as Rigidbody;
            newShell.AddForce(shellOrigin.right * Random.Range(150f, 200f) + shootOrigin.forward * Random.Range(-30f, 30f));
        }
    }

    private bool CanShoot()
    {
        if (typeOfGun == GunType.SemiAuto)
            return hasTriggerBeenRelease && Time.time > nextPossibleShoot;
        else if (typeOfGun == GunType.Auto)
            return Time.time > nextPossibleShoot;

        return true;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        rayTracer.enabled = true;

        rayTracer.SetPosition(0, shootOrigin.position);
        rayTracer.SetPosition(1, shootOrigin.position + hitPoint);

        yield return new WaitForFixedUpdate();
        rayTracer.enabled = false;
    }
}
