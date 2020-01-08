using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //for future use
    //TODO: public int ammoRate = 0;
    //public int damage = 10;
    public int fireRange = 100;

    public Transform muzzle;
    public Projectile projectile;
    //public GameObject hitEffect;
    //public GameObject bloodHitEffectPrefab;
    //public GameObject muzzleFlashPrefab;
    //public GameObject weaponSmokePrefab;
    //public GameObject muzzleSmokePrefab;

    //public float timeBetweenShots = 100;
    //float nextShotTime;
    public float muzzleVelocity = 35;
    bool isReloaded = true;

    public void GunFire()
    {
        if (isReloaded == true)
        {
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
            //isReloaded = false;
        }

        else
        {
            Debug.Log("Reload needed!");
        }

        //StartCoroutine(Shoot()); //Starts shhoting with raycasts
    }

    public void Reload()
    {
        //TODO: Reload animation
        Debug.Log("Reloading");
        isReloaded = true;
    }

    /*
    IEnumerator Shoot()
    {
        //muzzlePos = new Vector3(muzzle.transform.position.x, muzzle.transform.position.y, muzzle.transform.position.z);

        //Shoot
        if (Input.GetButtonDown("Fire1"))
        {
            player = GetComponentInParent<PlayerController>();

            //playerStats = transform.GetComponent<PlayerStats>();
            //enemy = FindObjectOfType<EnemyController>();

            RaycastHit hitWhatWithMouse;
            Ray shootRay = player.mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(shootRay, out hitWhatWithMouse))
            {
                //shoot in this direction
                Debug.Log("Player shot at " + hitWhatWithMouse.collider.name + " " + hitWhatWithMouse.point);
                Debug.DrawLine(muzzle.transform.position, hitWhatWithMouse.point, Color.yellow);

                playerStats = hitWhatWithMouse.transform.GetComponent<PlayerStats>();
                enemy = hitWhatWithMouse.transform.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    //GameObject cloneBloodHitEffect = Instantiate(bloodHitEffectPrefab, hitInfo.point, Quaternion.identity);
                    //Destroy(cloneBloodHitEffect.gameObject, 1.5f);
                }

                else if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                    //GameObject cloneBloodHitEffect = Instantiate(bloodHitEffectPrefab, hitMouse.point, Quaternion.identity);
                    //Destroy(cloneBloodHitEffect.gameObject, 1.5f);
                }

                else
                {
                    //GameObject cloneHitEffect = Instantiate(hitEffect, hitMouse.point, Quaternion.identity);
                    //Destroy(cloneHitEffect.gameObject, 1.5f);
                }

                lineRenderer.SetPosition(0, muzzle.transform.position);
                lineRenderer.SetPosition(1, hitWhatWithMouse.point);
            }
        }

        lastShootTime = Time.time;
        //isReloaded = false;


        lineRenderer.enabled = true;

        //wait for one frame
        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }

    /*
    public void WeaponFXEffects()
    {
        Vector2 muzzleRot = muzzle.rotation.eulerAngles;
        muzzleRot = new Vector2(muzzle.rotation.x, muzzle.rotation.y);
        GameObject cloneMuzleFlash = Instantiate(muzzleFlashPrefab, muzzle.position, muzzle.rotation);  //Quaternion.Euler(muzzleRot));

        float size = Random.Range(1.6f, 1.9f);
        cloneMuzleFlash.transform.localScale = new Vector3(size, size / 2, size);
        Destroy(cloneMuzleFlash.gameObject, 0.02f);

        GameObject cloneWeaponSmokePrefab = Instantiate(weaponSmokePrefab, transform.position, transform.rotation);
        Destroy(cloneWeaponSmokePrefab.gameObject, 5f);

        GameObject cloneMuzzleSmokePrefab = Instantiate(muzzleSmokePrefab, muzzle.position, muzzle.rotation);
        Destroy(cloneMuzzleSmokePrefab.gameObject, 5f);
    }

    public bool CanShoot()
    {
        if (Time.time - lastShootTime >= timeBetweenShots)
        {
            if (isReloaded == true)
                return true;
        }

        return false;
    }
    */
}