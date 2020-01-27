using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Firemode {Bayonet, Single };
    public Firemode firemode;
    //for future use
    //TODO: public int ammoRate = 0;
    //public int damage = 10;
    public int fireRange = 100;
    public int projectilesCountInBag;
    public float reloadTime = 1f;

    [Header("Recoil")]
    public Vector2 pullbackMinMax = new Vector2(.1f, .35f);
    public Vector2 recoilAngleMinMax = new Vector2(3,5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

    [Header("GunFX")]
    public Transform muzzleEjection;
    public Transform weaponSmokeEjection;
    public Projectile projectile;
    public float flashLightTime;

    [Header("GunPrefabs")]
    public GameObject flashLightHolder;
    public GameObject muzzleFlashPrefab;
    public GameObject muzzleSmokePrefab;
    public Transform muzzleSmokeStoppingPrefab;
    public GameObject weaponSmokePrefab;


    //public float timeBetweenShots = 100;
    //float nextShotTime;
    public float muzzleVelocity = 35;

    bool triggerWeaponMode;
    bool isReloaded = true;
    bool isReloading = false;      //to prevent from reloading during a reload
    int projectilesRemainingInBag;
    int projectilesInMuzzle;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    private void Start()
    {
        DeactivateMuzzleLight();
        projectilesRemainingInBag = projectilesCountInBag;
    }

    private void LateUpdate()
    {
        //to animate the recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, .1f);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, .1f);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;
    }

    public void GunFire()
    {
        if (isReloaded == true && !isReloading)
        {
            Projectile newProjectile = Instantiate(projectile, muzzleEjection.position, muzzleEjection.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
            isReloaded = false;
            GunFireFXEffects();
        }

        else
        {
            Debug.Log("Reload needed!");
        }
    }

    public void Reload()
    {
        if(projectilesRemainingInBag > 0 && !isReloading)
        {
            projectilesRemainingInBag--;
            projectilesInMuzzle++;
            StartCoroutine(AnimateReload());
            Debug.Log("Reloading");
            isReloaded = true;
        }

        else
        {
            print("Amunition bag empty!");
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;

        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1 / reloadTime;
        float percent = 0; //to know, how far in the animation we are

        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 60;

        while(percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;
            yield return null;
        }

        isReloading = false;
    }

    public void Aim(Vector3 aimPoint)
    {
        transform.LookAt(aimPoint);
    }

    void GunFireFXEffects()
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

        //to animate the recoil
        transform.localPosition -= Vector3.forward * Random.Range(pullbackMinMax.x, pullbackMinMax.y);
        recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
        recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
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