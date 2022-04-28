using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Creep : MonoBehaviour
{
    [Header("Stat")]
    public float health, maxHealth;
    public float speed;
    public float armour;
    public float money;
    public float lives;
    public Vector3 objective;

    [Header("UI")]
    public GameObject canvas;
    public Image healthBar;

    public NavMeshAgent agent;      //The thing that handles pathfinding
    Camera cam;                     //Camera reference

    public GameObject deathParticle;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.fillAmount = 1;
        agent = GetComponent<NavMeshAgent>();   //Get agent component
        agent.SetDestination(objective);        //Tell agent where to go
        agent.speed = speed;                    //Tell agent how fast it can turn
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Set the Canvas rotation to match the cameras so it looks correct
        canvas.transform.rotation = cam.transform.rotation;

        //How far from the target are we right now?
        float dist = Vector3.Distance(transform.position, objective);

        //If we are close to the target
        if(dist < 1f)
        {
            //Deal damage to the manager
            FindObjectOfType<Manager>().CreepDied(0);
            FindObjectOfType<Manager>().ChangeLives(-1);

            //and disappear
            Destroy(gameObject);
        }
    }

    /// <summary>
    ///  Called whenever something damages the creep
    /// </summary>
    /// <param name="value"></param>
    public void TakeDamage(float value, Tower damageSource)
    {
        //Subtract health
        health = health - (value);

        //Set the health bar to proportion
        healthBar.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            FindObjectOfType<Manager>().CreepDied(money);

            //Spawn particles
            GameObject particle = Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(particle, 5);

            particle.transform.LookAt(damageSource.transform);

            //Die
            Destroy(gameObject);
        }
    }
}
