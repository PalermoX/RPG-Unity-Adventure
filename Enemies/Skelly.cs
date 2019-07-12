﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skelly : MonoBehaviour, IEnemy
{
    public int       Experience { get; set; }
    public Spawner   Spawner    { get; set; }
    public DropTable DropTable  { get; set; }
    
    public float attack,
                 strength,
                 maxHealth,
                 currentHealth;

    private Collider[]     withinAggroColliders;

    public  PickupItem     pickupItem;
    public  LayerMask      aggroLayerMask;
    private NavMeshAgent   navAgent;
    private CharacterStats characterStats;
    private Player         player;
    

    void Start()
    {
        // Create Instance of droptable.
        DropTable = new DropTable();

        /* Example below, summing up the 25% chance of drops, we have a total of 75%.
           This means that 25% of the time, the NPC may drop nothing. */
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("Sword", 25), // 25% chance of sword being dropped.
            new LootDrop("Staff_Of_Pain", 25),
            new LootDrop("Heal_Potion", 25)
        };

        Experience = 200;
        navAgent = GetComponent<NavMeshAgent>(); // Reference from navagent on the enemy
        characterStats = new CharacterStats(5, 10, 15);
        currentHealth = maxHealth;
    }

    void FixedUpdate() // Maxes at 50fps
    {
        /* Send out imaginated sphere that exist aroudn every frame around the enemy to
           decide if theres a collider its looking for inside the spehre or not.. Sphere == radius.
           10 units for it to exist. */
        withinAggroColliders = Physics.OverlapSphere(transform.position, 10, aggroLayerMask);
        if (withinAggroColliders.Length > 0)
        {
            ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
            // Debug.Log("Player has been spotted. Get em!!");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Source: @17:20 https://www.youtube.com/watch?v=Bs0rJEkYBvc&list=PLivfKP2ufIK6ToVMtpc_KTHlJRZjuE1z0&index=16
    void ChasePlayer(Player player)
    {
        // Ensures the enemy can't attack unless close to you.
        navAgent.SetDestination(player.transform.position);
        this.player = player;
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            if (!IsInvoking("PerformAttack"))
            {
                // Invoke itself automatically, starting time attack is .5f.  when it gets close, then every 2 seconds hit.
                InvokeRepeating("PerformAttack", 0.1f, 1f);
            }
        }
        else
        {
            // Debug.Log("Not within distance");
            CancelInvoke("PerformAttack");
        }
    }

    public void Die()
    {
        DropLoot();
        CombatEvents.EnemyDied(this); // This instance of the object.
        this.Spawner.Respawn();
        Destroy(gameObject);
    }

    public void PerformAttack()
    {
        player.TakeDamage(5);
    }

    void DropLoot()
    {
        Item item = DropTable.GetDrop();
        if(item != null)
        {
            PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity); // Item dropped will appear right where NPC died.
            instance.ItemDrop = item;
        }
    }
}
