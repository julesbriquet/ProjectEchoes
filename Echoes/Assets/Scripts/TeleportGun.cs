using UnityEngine;
using System.Collections;

public class TeleportGun : GunEntity {

    public TeleportBall ballBody;
    private TeleportBall lastSpawnedBall;

	
    public override void Shoot(WeaponGlobalUI weaponUI)
    {
        if (CanShoot())
        {
            Ray ray = new Ray(shootOrigin.position, shootOrigin.forward);
            RaycastHit hit;
            float shotDistance = shootRange;
            

            // Do nothing if not touch
            if (Physics.Raycast(ray, out hit, shootRange, collisionMask))
            {
                shootAudioEntity.Play();
                shotDistance = hit.distance;
                Transform livingEntityTouched = hit.collider.GetComponent<Transform>();
                if (livingEntityTouched)
                {
                    if (lastSpawnedBall)
                        Destroy(lastSpawnedBall.gameObject);

                    Quaternion teleportBallRotation = Quaternion.FromToRotation(Vector3.right, hit.normal);
                    lastSpawnedBall = Instantiate(ballBody, hit.point, teleportBallRotation) as TeleportBall;
                    Vector3 newTeleportPoint = hit.point + (hit.normal * 2);
                    lastSpawnedBall.teleportPosition = newTeleportPoint;
                    //Debug.Log("WALAK" + hit.normal);
                    //Debug.Log("ANGLE" + hit.transform.eulerAngles);
                    
                }

                // Compute time for enabling next shot (Mode Auto Only)
                nextPossibleShoot = Time.time + secondsBetweenShoots;

                // Countdown ammo in mag
                ammoInMag--;
                weaponUI.updateAmmoInMagUI(gunUIEntity.numberOfLine, ammoPerMag, ammoInMag);

                Rigidbody newShell = Instantiate(shellBody, shellOrigin.position, Quaternion.identity) as Rigidbody;
                newShell.AddForce(shellOrigin.right * Random.Range(150f, 200f) + shootOrigin.forward * Random.Range(-30f, 30f));
            }
            else
            {
                // Simply avoid to much calling
                nextPossibleShoot = Time.time + 1;
            }

            // Draw Ray in Game
            //if (rayTracer)
              //  StartCoroutine("RenderTracer", ray.direction * shotDistance);



            // Draw Ray in Debug
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 2);
            
            
        }
        else if (ammoInMag == 0 && CanReload())
            ReloadWeapon(weaponUI);

    }
}
