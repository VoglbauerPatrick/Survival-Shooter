﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variablendefintion
    int health = 100;
    LootDropManager manager = null;

    int randomLootNum;
    int randomSoundNum;

    AudioManager audioManager;

    //Partikel Systeme
    public ParticleSystem killParticles;

    //Enemy Animator
    public Animator animator;

    //Start
    void Start()
    {
        GetComponent<EnemyFollow>().enabled = false;

        //Dem manager wird der LootDropManager zugewiesen
        manager = GameObject.FindObjectOfType<LootDropManager>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    //Methode für Schaden
    public void takeDamage(int damage)
    {
        //Schaden wird erlitten
        health = health - damage;

        //Schadens-Animation wird abgespielt
        animator.SetTrigger("Hit");

        //Wenn die Lebenspunkte kleiner als oder gleich 0 sind wird das Objekt zerstört;
        if (health <= 0)
        {

            //großer Burst wird gespawnt
            Instantiate(killParticles, gameObject.transform.position, gameObject.transform.rotation);

            Destroy(gameObject);

            GameObject.FindGameObjectWithTag("Player").GetComponent<KillCounterSystem>().IncreaseKillCounter();
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawnSystem>().IncreaseKillCounter();

            //LootSpawnSystem
            //Es wird zufallsgeneriert ob Loot gedroppt wird
            if (Random.Range(1, GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawnSystem>().maxRandomValue) == 1)
            {
                //Es wird zufallsgeneriert welcher Loot gedroppt wirds
                //Max + 1, weil der maximale Wert exklusiv ist
                randomLootNum = Random.Range(1, 101);

                Debug.Log(randomLootNum);

                if (randomLootNum > 0 && randomLootNum <= 30)       
                {
                    manager.Spawn(gameObject.transform, "Shotgun");     //Spawnwarscheinlichkeit: 30%
                }
                else if (randomLootNum > 30 && randomLootNum <= 60)
                {
                    manager.Spawn(gameObject.transform, "Golden Gun");  //Spawnwarscheinlichkeit: 30%
                }
                else if (randomLootNum > 60 && randomLootNum <= 90)
                {
                    manager.Spawn(gameObject.transform, "VAPR-XKG");    //Spawnwarscheinlichkeit: 30%
                }
                else if (randomLootNum > 90 && randomLootNum <= 100)
                {
                    manager.Spawn(gameObject.transform, "Minigun");     //Spawnwarscheinlichkeit: 10%
                }

                //LootDropSound abspielen
                audioManager.Play("LootDropSound");
            }
            //Wenn keine Waffe spawnt ist die Chance 1 zu 50, dass ein Herz spawnt
            else if (Random.Range(0, 50) == 0) {

                manager.Spawn(gameObject.transform, "Full Heart");

                //LootDropSound abspielen
                audioManager.Play("LootDropSound");
            }

   
            //Einer von 3 Zombie-Sounds wird abgespielt
            randomSoundNum = Random.Range(1, 4);

            if(randomSoundNum == 1) {
                audioManager.Play("Zombie 1");
            }
            else if (randomSoundNum == 2)
            {
                audioManager.Play("Zombie 2");
            }
            else if (randomSoundNum == 3)
            {
                audioManager.Play("Zombie 3");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ground")
        {
            GetComponent<EnemyFollow>().enabled = true;
        }
    }
}
