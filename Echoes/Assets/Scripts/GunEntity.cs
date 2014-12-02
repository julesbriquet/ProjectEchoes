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
    public int ammoInMag;
    public int ammoPerMag;
    public int totalAmmo;
    public float reloadingTime;

    // Handle audio
    private AudioSource shootAudioEntity;
    private AudioSource reloadAudioEntity;

    // Handling Shell Ejection
    public Transform shellOrigin;
    public Rigidbody shellBody;

    // Handling damage
    public LayerMask collisionMask;
    public int weaponDamage;

    public int shootRange;


    // GUI Handler that contains data for that specific weapon
    public GunUI gunUIEntity;


    // Use this for initialization
    void Start()
    {
        secondsBetweenShoots = 60 / roundPerMinute;
        nextPossibleShoot = 0;
        AudioSource[] tmpAudioTable = GetComponents<AudioSource>();
        shootAudioEntity = tmpAudioTable[0];
        reloadAudioEntity = tmpAudioTable[1];
        rayTracer = GetComponent<LineRenderer>();


    }

    void Awake()
    {
        gunUIEntity = GetComponent<GunUI>();
    }

    public void Shoot(WeaponGlobalUI weaponUI)
    {
        if (CanShoot())
        {
            Ray ray = new Ray(shootOrigin.position, shootOrigin.forward);
            RaycastHit hit;
            float shotDistance = shootRange;
            shootAudioEntity.Play();
            if (Physics.Raycast(ray, out hit, shootRange, collisionMask))
            {
                shotDistance = hit.distance;

                LivingEntity livingEntityTouched = hit.collider.GetComponent<LivingEntity>();
                if (livingEntityTouched)
                    livingEntityTouched.TakeDamage(weaponDamage);
            }

            // Compute time for enabling next shot (Mode Auto Only)
            nextPossibleShoot = Time.time + secondsBetweenShoots;

            // Countdown ammo in mag
            ammoInMag--;
            weaponUI.updateAmmoInMagUI(gunUIEntity.numberOfLine, ammoPerMag, ammoInMag);

            // Draw Ray in Game
            if (rayTracer)
                StartCoroutine("RenderTracer", ray.direction * shotDistance);

            

            // Draw Ray in Debug
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 2);
            Debug.Log("Direction " + ray.direction * shotDistance);

            Rigidbody newShell = Instantiate(shellBody, shellOrigin.position, Quaternion.identity) as Rigidbody;
            newShell.AddForce(shellOrigin.right * Random.Range(150f, 200f) + shootOrigin.forward * Random.Range(-30f, 30f));
        }
        else if (ammoInMag == 0 && CanReload())
            ReloadWeapon(weaponUI);
    }

    public bool CanShoot()
    {
        bool canShoot = Time.time > nextPossibleShoot && ammoInMag > 0;

        if (typeOfGun == GunType.SemiAuto)
            canShoot = canShoot && hasTriggerBeenRelease;

        return canShoot;
    }

    public bool CanReload()
    {
        bool canReload = totalAmmo > 0 && ammoInMag < ammoPerMag;

        if (typeOfGun == GunType.SemiAuto)
            canReload = canReload && hasTriggerBeenRelease;

        return canReload;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        rayTracer.enabled = true;

        rayTracer.SetPosition(0, shootOrigin.position);
        rayTracer.SetPosition(1, shootOrigin.position + hitPoint);

        yield return new WaitForFixedUpdate();
        rayTracer.enabled = false;
    }

    public void ReloadWeapon(WeaponGlobalUI weaponUI)
    {
        int currentAmmoInMag = ammoInMag;
        if (totalAmmo + currentAmmoInMag < ammoPerMag)
            ammoInMag = currentAmmoInMag + totalAmmo;
        else
            ammoInMag = ammoPerMag;

        reloadAudioEntity.Play();
        totalAmmo -= (ammoInMag - currentAmmoInMag);
        weaponUI.reloadWeaponUI(totalAmmo, gunUIEntity.numberOfLine, ammoPerMag, ammoInMag);
        nextPossibleShoot = Time.time + reloadingTime;
    }
}
