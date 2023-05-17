using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               //  Nav mesh agent component
    public float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float speedWalk = 6;                     //  Walking speed, speed in the nav mesh agent
    public float speedRun = 9;                      //  Running speed

    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something
    public Animator animator;

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy

    float m_WaitTime;                               //  Variable of the wait time that makes the delay
    float m_TimeToRotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    bool m_playerInRange;                           //  If the player is in range of vision, state of chasing
    bool m_PlayerNear;                              //  If the player is near, state of hearing
    bool m_IsPatrol;                                //  If the enemy is patrol, state of patroling
    bool m_CaughtPlayer;                            //  if the enemy has caught the player

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                
        m_TimeToRotate = timeToRotate;

     
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;            

        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("isChasing", false);
    }

    private void Update()
    {
        EnviromentView();

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            animator.SetBool("isChasing", true);
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);
            if (distanceToPlayer > navMeshAgent.stoppingDistance)  // continue chasing if player is far enough
            {
                Move(speedRun);
                navMeshAgent.SetDestination(m_PlayerPosition);
            }
            else  // stop chasing if player is too close
            {
                Stop();
                m_IsPatrol = true;
            }
        }
    }


    private void Patroling()
    {
        if (m_PlayerNear)
        {
           
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                
            }
            else
            {
                
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }

    }

    private void OnAnimatorMove()
    {

    }


    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }


    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);  

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             
                    m_IsPatrol = false;                
                }
                else
                {
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
             
                m_playerInRange = false;  
            }
            if (m_playerInRange)
            {
                
                m_PlayerPosition = player.transform.position;      
            }
        }
    }
}