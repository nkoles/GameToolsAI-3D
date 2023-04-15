using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    //Enemy AI variables
    private NavMeshAgent aiNavMesh;

    //Player Reference Variables
    public GameObject player;
    private bool playerCheck;
    private PlayerController playerContr;

    //Idle timer floats
    public float idleTimer, idleTime;

    //Seting up State Machine
    public enum State
    {
        AttackPlayer,
        Idle,
        RunAway
    }

    private State currentState;

    void Start()
    {
        //Getting AI Nav Component and Trigger Collider
        aiNavMesh = GetComponent<NavMeshAgent>();
        playerContr = FindObjectOfType<PlayerController>();

        currentState = State.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.AttackPlayer:

                if(playerContr.hit == true)
                {
                    aiNavMesh.SetDestination(transform.position);
                }
                else
                {
                    aiNavMesh.SetDestination(player.transform.position);
                }

                if(playerCheck == false)
                {
                    currentState = State.Idle;
                }

                break;
            
            case State.Idle:
                Idle();

                idleTimer += Time.deltaTime;
                
                if(playerCheck == true)
                {
                    currentState = State.AttackPlayer;
                }

                break;

            case State.RunAway:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.isTrigger == false)
        {
            playerCheck = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && other.isTrigger == false)
        {
            playerCheck = false;
        }
    }

    private void Idle()
    {
        if(idleTimer >= idleTime)
        {
            Vector2 wanderTarg = Random.insideUnitCircle * idleTime;
            Vector3 wanderPos = new Vector3(transform.position.x + wanderTarg.x, transform.position.y, transform.position.z + wanderTarg.y);

            aiNavMesh.SetDestination(wanderPos);
            idleTimer = 0;
        }
    }
}
