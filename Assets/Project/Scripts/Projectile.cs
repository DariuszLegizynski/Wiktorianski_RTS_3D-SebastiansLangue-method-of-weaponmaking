using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;

    public GameObject bloodHitEffectPrefab;

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
            OnHitObject(initialCollision[0]);
        }
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
            OnHitObject(hit);
            GameObject cloneBloodHitEffect = Instantiate(bloodHitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(cloneBloodHitEffect.gameObject, 1.5f);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();

        if(damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }

    void OnHitObject(Collider c)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        GameObject.Destroy(gameObject);
    }

    public void ProjectileFXEffects()
    {

    }
}

