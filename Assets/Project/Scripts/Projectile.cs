using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;

    public GameObject bloodHitEffectPrefab;
    //public GameObject projectileTrailPrefab;

    float speed = 10;
    float damage = 1f;

    float lifetime = 1.1f;
    float fineCollisionFit = .1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);

        Collider[] initialCollision = Physics.OverlapSphere(transform.position, .1f, collisionMask);

        if(initialCollision.Length > 0)
        {
            OnHitObject(initialCollision[0], transform.position);
        }

        //GameObject cloneProjectileTrail = Instantiate(projectileTrailPrefab, transform.position, Quaternion.identity);
        //Destroy(cloneProjectileTrail.gameObject, lifetime);
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, moveDistance + fineCollisionFit, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
            GameObject cloneBloodHitEffect = Instantiate(bloodHitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(cloneBloodHitEffect.gameObject, 1.5f);
        }
    }

    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
        GameObject.Destroy(gameObject);
    }

    public void ProjectileFXEffects()
    {

    }
}

