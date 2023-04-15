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
    private bool playerCheck;

    float timer, wanderTime;
    
    public float movementRange;
    public Vector2 wanderTimerRange;
        

    public enum State
    {
        FindCharacter,
        Wander,
        Idle,
        FindZombie,
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
                break;
            case State.Wander:
                timer += Time.deltaTime;
                Wandering();
                break;
            case State.Idle:
                break;
            case State.FindZombie:
                break;
            
        }
    }

    void OnTriggerEnter(Collider triggerCol)
    {
        if(triggerCol.gameObject.tag == "Player")
        {
            playerCheck = true;
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

    void SetAITarget(Vector3 targetLocation)
    {
        aiNavMesh.SetDestination(targetLocation);
    }
}
