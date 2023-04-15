using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionController : MonoBehaviour
{
    private NavMeshAgent companionAgent;
    public RotationScript refPoint;
    public GameObject refPointEdge;

    private int numEnemies;
    private bool enemyCheck;
    private float theta;

    public float rad = 2;

    private enum State
    {
        AttackEnemy,
        Idle,
        ToIdle,
        Cooldown
    }

    private State currentState;

    void Start()
    {
        companionAgent = GetComponent<NavMeshAgent>();

        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                companionAgent.enabled = false;
                refPoint.enabled = true;

                if(enemyCheck == true)
                {
                    currentState = State.AttackEnemy;
                }

                break;

            case State.ToIdle:

                break;

            case State.AttackEnemy:
                refPoint.enabled = false;

                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.isTrigger == false)
        {

            enemyCheck = true;
            SetNavTarget(other.transform.position);
        }

    }

    private void SetNavTarget(Vector3 targetPos)
    {
        companionAgent.SetDestination(targetPos);
    }
}
