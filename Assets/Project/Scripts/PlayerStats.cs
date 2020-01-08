/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;
        public int currentHealth;

        //sets our current health (or whatever stats are used) to the max health value at the start of the game
        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public PlayerStats playerStats = new PlayerStats();

    private void Awake()
    {
        playerStats.Init();
    }

    public void TakeDamage(int damage)
    {
        playerStats.currentHealth -= damage;

        if(playerStats.currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //TODO: this is a temporary solution
        Destroy(gameObject);
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [System.Serializable]
    public class Player_Stats
    {
        public int health = 10;
        //public int damage = 10;
    }

    public Player_Stats playerStats = new Player_Stats();

    public void TakeDamage(int damage)
    {
        playerStats.health -= damage;

        if (playerStats.health <= 0)
        {
            //temporary solution
            //TODO:
            Die();
        }
    }

    void Die()
    {
        //TODO: this is a temporary solution
        Destroy(gameObject);
    }

    //GameMaster.KillEnemy(this);
}
