using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAiScript : MonoBehaviour
{
    private NavMeshAgent aiNavMesh;
    private Collider triggerCol;
    public GameObject player;
    public GameObject enemy;
    private bool playerCheck;

    float timer, wanderTime;
    
    public float movementRange;
    public float movementRatio;
    public Vector2 wanderTimerRange;
    public float minDistance;
        

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
        //Getting AI Nav Component and Trigger Collider
        aiNavMesh = GetComponent<NavMeshAgent>();
        triggerCol = player.GetComponent<Collider>();

        currentState = State.Wander;
        wanderTime = wanderTimerRange.x;
    }

    void Update()
    {
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
    }

    void OnTriggerEnter(Collider triggerCol)
    {
        if(triggerCol.gameObject.tag == "Player")
        {
            currentState = State.FindCharacter;
        }
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
