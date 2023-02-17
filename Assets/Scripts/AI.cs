using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Chasing,
        Travelling,
        Waiting,
        Attacking
    }

    public State currentState;

    NavMeshAgent agent;

    public Transform[] destinationPoints;
    public int destinationIndex = 0;
    public Transform player;
    [SerializeField] float visionRange;
    [SerializeField] float attackRange;
    [SerializeField] float patrolRange = 10f;
    [SerializeField] float timeRemaining = 5;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            case State.Chasing:
                Chase();
            break;
            case State.Travelling:
                Travel();
            break;
            case State.Waiting:
                Waiting();
            break;
            case State.Attacking:
                Attacking();
            break;
            default:
                Chase();
            break;
            
        }
    }

    void Patrol() 
    {
        agent.destination = destinationPoints[destinationIndex].position;
        
        
        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }

        currentState = State.Travelling;
    }

    void Travel()
    {
        if(agent.remainingDistance <= 0.2)
        {
            
            destinationIndex++;
            if(destinationIndex == 5)
            {
                destinationIndex = 0;
            }
            currentState = State.Waiting;

        }

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }
    }

    void Chase()
    {
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = State.Attacking;
        }

        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }
    }

    void Waiting()
    {
        
        if(timeRemaining <= 0)
        {
            timeRemaining = 1;
            currentState = State.Patrolling;

        }
        else
        {
            timeRemaining -= Time.deltaTime; 
        }
        
        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }
    }

    void Attacking()
    {
        Debug.Log("Toma ostia nene");

        currentState = State.Chasing;
    }

    void OnDrawGizmos() 
    {
        foreach(Transform point in destinationPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
