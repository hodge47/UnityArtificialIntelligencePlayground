using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : FSM
{

    public enum FSMState { None, Patrol, Chase, Attack, Dead }

    public FSMState curState;

    private float curSpeed;
    private float curRotSpeed;
    [SerializeField]
    private GameObject bullet;
    private bool isDead = false;
    private int health;
    new private Rigidbody rigidbody;

    protected override void Initialize()
    {
        curState = FSMState.Patrol;
        curSpeed = 5;
        curRotSpeed = 5;
        isDead = false;
        elapsedTime = 0f;
        shootRate = 3f;
        health = 100;

        // Get the list of waypoints
        pointList = GameObject.FindGameObjectsWithTag("WanderPoint");

        // Set random destination point first
        FindNextPoint();

        // Get the target enemy which is the player
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        // Get the rigidbody
        rigidbody = GetComponent<Rigidbody>();

        playerTransform = objPlayer.transform;
        if (!playerTransform)
        {
            Debug.Log("Player does not exist in the scene");
        }

        // Get the turret of the tank
        turret = gameObject.transform.GetChild(0);
        bulletSpwnPoint = turret.GetChild(0);
    }

    protected override void FSMFixedUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol:
                UpdatePatrolState();
                break;
            case FSMState.Chase:
                UpdateChaseState();
                break;
            case FSMState.Attack:
                UpdateAttackState();
                break;
            case FSMState.Dead:
                UpdateDeadState();
                break;
        }

        // Update the time
        elapsedTime += Time.deltaTime;

        // Go to dead state if no health left
        if (health <= 0)
        {
            curState = FSMState.Dead;
        }
    }

    private void UpdatePatrolState()
    {

    }

    private void UpdateChaseState()
    {

    }

    private void UpdateAttackState()
    {

    }

    private void UpdateDeadState()
    {

    }

    private void FindNextPoint()
    {

    }
}
