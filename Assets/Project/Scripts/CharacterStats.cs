﻿
using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        //TODO: some stuff with the hit variable
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Suicide")] //for debuging, to kill yourself quickly -> just right cklick on script and choose this option
    protected void Die()
    {
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
