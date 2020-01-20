using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //for future use
    //TODO: public int ammoRate = 0;
    //public int damage = 10;
    public int fireRange = 100;
    public float flashLightTime;

    public Transform muzzleEjection;
    public Transform weaponSmokeEjection;
    public Projectile projectile;

    public GameObject flashLightHolder;
    public GameObject muzzleFlashPrefab;
    public GameObject muzzleSmokePrefab;
    public Transform muzzleSmokeStoppingPrefab;
    public GameObject weaponSmokePrefab;


    //public float timeBetweenShots = 100;
    //float nextShotTime;
    public float muzzleVelocity = 35;
    bool isReloaded = true;

    private void Start()
    {
        DeactivateMuzzleLight();
    }

    public void GunFire()
    {
        if (isReloaded == true)
        {
            Projectile newProjectile = Instantiate(projectile, muzzleEjection.position, muzzleEjection.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
            isReloaded = false;

            WeaponFXEffects();
        }

        else
        {
            Debug.Log("Reload needed!");
        }
    }

    public void Reload()
    {
        //TODO: Reload animation
        Debug.Log("Reloading");
        isReloaded = true;
    }

    void WeaponFXEffects()
    {
        flashLightHolder.SetActive(true);

        //Vector2 muzzleRot = muzzle.rotation.eulerAngles;
        //muzzleRot = new Vector2(muzzle.rotation.x, muzzle.rotation.y);
        GameObject cloneMuzleFlash = Instantiate(muzzleFlashPrefab, muzzleEjection.position, muzzleEjection.rotation);  //Quaternion.Euler(muzzleRot));

        float size = Random.Range(1.6f, 1.9f);
        cloneMuzleFlash.transform.localScale = new Vector3(size*size*2, size*size*2, size*size*2);
        Destroy(cloneMuzleFlash.gameObject, 0.02f);

        GameObject cloneWeaponSmokePrefab = Instantiate(weaponSmokePrefab, weaponSmokeEjection.position, weaponSmokeEjection.rotation);
        Destroy(cloneWeaponSmokePrefab.gameObject, 3f);

        //GameObject cloneWeaponSmokePrefab = Instantiate(weaponSmokePrefab, transform.position, transform.rotation);
        //Destroy(cloneWeaponSmokePrefab.gameObject, 5f);

        Transform cloneMuzzleSmokeStoppingPrefab = Instantiate(muzzleSmokeStoppingPrefab, muzzleEjection.position, muzzleEjection.rotation) as Transform;
        Destroy(cloneMuzzleSmokeStoppingPrefab.gameObject, 3f);

        GameObject cloneMuzzleSmokePrefab = Instantiate(muzzleSmokePrefab, muzzleEjection.position, Quaternion.identity);
        Destroy(cloneMuzzleSmokePrefab.gameObject, 3f);

        Invoke("DeactivateMuzzleLight", flashLightTime);
    }

    void DeactivateMuzzleLight()
    {
        flashLightHolder.SetActive(false);
    }

    /*
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