using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpiderAiScript : MonoBehaviour
{
    private NavMeshAgent aiNavMesh;
    private Collider triggerCol;
    public GameObject player;
    public GameObject enemy;

    public float cooldownMax;

    float timer, wanderTime, cooldownHunt;
    
    public float movementRange;
    public float movementRatio;
    public Vector2 wanderTimerRange;
    public float minDistance;
    public float rotationSpeed;
    public float detectionRange;

    private bool hasTarget;

    private Quaternion rot;


    public enum State
    {
        FindCharacter,
        Wander,
        Idle,
        FindZombie,
        TouchCharacter,
        TouchZombie
    }

    private State currentState;
    void Start()
    {
        aiNavMesh = GetComponent<NavMeshAgent>();
        triggerCol = player.GetComponent<Collider>();

        currentState = State.Wander;
        wanderTime = wanderTimerRange.x;
        hasTarget = false;
        
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        enemy = FindClosestEnemy();

        if (cooldownHunt >= cooldownMax)
        {
            cooldownHunt = 0;
        }

        if (Vector3.Distance(player.transform.position, transform.position) < detectionRange && !hasTarget)
        {
            currentState = State.FindCharacter;
            hasTarget = true;
        }
        else if (enemy)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < detectionRange && !hasTarget)
            {
                currentState = State.FindZombie;
                hasTarget = true;
            }
        }
        
        switch (currentState)
        {
            case State.FindCharacter:
                timer += Time.deltaTime;
                MoveTowards(player);
                float distToPlayer = Vector3.Distance(player.transform.position, transform.position);
                if (distToPlayer < minDistance)
                {
                    currentState = State.TouchCharacter;
                }
                break;
            case State.Wander:
                timer += Time.deltaTime;
                Wandering();
                break;
            case State.Idle:
                
                MoveTowardsStraight(gameObject);
                
                cooldownHunt += Time.deltaTime;
                
                if (cooldownHunt >= cooldownMax)
                {
                    hasTarget = false;
                    currentState = State.Wander;
                }
                
                break;
            case State.FindZombie:
                timer += Time.deltaTime;
                MoveTowards(enemy);
                float dist = Vector3.Distance(enemy.transform.position, transform.position);
                if (dist < minDistance)
                {
                    currentState = State.TouchZombie;
                }
                break;
            case State.TouchCharacter:
                MoveTowardsStraight(player);
                break;
            case State.TouchZombie:
                MoveTowardsStraight(enemy);
                break;
            
        }
        
        Vector3 dir = aiNavMesh.destination - transform.position;
        dir.y = 0;//This allows the object to only rotate on its y axis
        rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    void OnTriggerStay(Collider triggerCol)
    {
        /*
         if(triggerCol.gameObject.tag == "Player" && !hasTarget)
        {
            
        }

        if (triggerCol.gameObject.tag == "Enemy" && !hasTarget)
        {
            currentState = State.FindZombie;
            hasTarget = true;
        }*/

        
    }
    
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && go.gameObject.GetComponent<EnemyAIController>())
            {
                closest = go;
                distance = curDistance;
            }
        }
        
        

        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && !other.isTrigger)
        {
            Destroy(other.gameObject);

            currentState = State.Wander;

            hasTarget = false;
        }
        else if (!other.isTrigger)
        {
            currentState = State.Idle;
        }
        
        Debug.Log("ah.");
    }

    void Wandering()
    {
        if (timer >= wanderTime)
        {
            Vector2 wanderTarget = Random.insideUnitCircle * movementRange;
            Vector3 wanderPos3D = new Vector3(transform.position.x + wanderTarget.x, transform.position.y, transform.position.z + wanderTarget.y);
            SetAITarget(wanderPos3D);
            timer = 0;
            wanderTime = Random.Range(wanderTimerRange.x, wanderTimerRange.y);
            
            SetAITarget(wanderPos3D);
        }
    }

    void MoveTowards(GameObject followObject)
    {
        if (timer >= wanderTime)
        {
            Vector2 wanderTarget = Random.insideUnitCircle * movementRange;
            Vector3 wanderPos3D = new Vector3(((1 - movementRatio)*transform.position.x + movementRatio*followObject.transform.position.x) + wanderTarget.x, transform.position.y, ((1 - movementRatio)*transform.position.z + movementRatio*followObject.transform.position.z) + wanderTarget.y);
            SetAITarget(wanderPos3D);
            timer = 0;
            wanderTime = Random.Range(wanderTimerRange.x, wanderTimerRange.y);
            
            SetAITarget(wanderPos3D);
        }
    }

    void SetAITarget(Vector3 targetLocation)
    {
        aiNavMesh.SetDestination(targetLocation);
    }

    private void MoveTowardsStraight(GameObject target)
    {
        aiNavMesh.SetDestination(target.transform.position);
    }
}
