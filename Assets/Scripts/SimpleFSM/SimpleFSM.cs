using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : FSM
{

    public enum FSMState { None, Patrol, Chase, Attack, Dead }
    [SerializeField]
    private float chaseDistance = 20f;
    [SerializeField]
    private float attackDistance = 2f;

    public FSMState curState;

    private float curSpeed;
    private float curRotSpeed;
    [SerializeField]
    private GameObject tankBody;
    [SerializeField]
    private Material patrolMat;
    [SerializeField]
    private Material chaseMat;
    [SerializeField]
    private Material attackMat;
    private Renderer renderer;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject deathExplosion;
    private bool isDead = false;
    private int health;
    new private Rigidbody rigidbody;

    protected override void Initialize()
    {
        curState = FSMState.Patrol;
        curSpeed = 3;
        curRotSpeed = 5;
        isDead = false;
        elapsedTime = 0f;
        shootRate = 3f;
        health = 200;

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

        renderer = tankBody.GetComponent<Renderer>();
        renderer.material = patrolMat;
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
        renderer.material = patrolMat;
        // Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 2f)
        {
            print("Reached the destination point!\nCalculating next point.");
            FindNextPoint();
        }
        // Check the distance with the player tank when the distance is near, transition to chase state
        else if (Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance)
        {
            print("Switched to chase state!");
            curState = FSMState.Chase;
        }

        // Rotate to the target point
        Quaternion _targetRotation = Quaternion.LookRotation(destPos - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * curRotSpeed);
        // Move forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    private void UpdateChaseState()
    {
        renderer.material = chaseMat;
        // Set the target position as the player position
        destPos = playerTransform.position;
        // Check the distance with the player tank. When the distance is near, transition to attack state
        float _dist = Vector3.Distance(transform.position, playerTransform.position);

        if (_dist <= attackDistance)
        {
            curState = FSMState.Attack;
        }
        // Go back to patrol if the player is too far
        else if (_dist >= chaseDistance)
        {
            curState = FSMState.Patrol;
        }

        // Move forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    private void UpdateAttackState()
    {
        renderer.material = attackMat;
        // Set the target position as the player position
        destPos = playerTransform.position;

        // Check the distance from the player tank
        float _dist = Vector3.Distance(transform.position, playerTransform.position);

        if (_dist >= 1f && _dist < attackDistance)
        {
            // Rotate to the target point
            Quaternion _targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * curRotSpeed);
        }

        // Transition to the patrol state if the player tank is too far
        else if (_dist >= chaseDistance)
        {
            curState = FSMState.Patrol;
        }

        // Always turn the turret toward the player
        Quaternion _turretRotation = Quaternion.LookRotation(destPos - turret.position);

        turret.rotation = Quaternion.Slerp(turret.rotation, _turretRotation, Time.deltaTime * curRotSpeed);

        // Shoot bullets
        ShootBullet();
    }

    private void UpdateDeadState()
    {
        // Show the dying animation with some physics effects
        if (!isDead)
        {
            isDead = true;
            Explode();
        }
    }

    private void FindNextPoint()
    {
        print("Finding next point!");

        int _randIndex = Random.Range(0, pointList.Length);
        float _randRadius = 10f;
        Vector3 _randPosition = Vector3.zero;

        destPos = pointList[_randIndex].transform.position + _randPosition;

        // Check range to decide the random point isnt the same as before
        if (IsInCurrentRange(destPos))
        {
            _randPosition = new Vector3(Random.Range(-_randRadius, _randRadius), 0f, Random.Range(-_randRadius, _randRadius));
            destPos = pointList[_randIndex].transform.position + _randPosition;
        }
    }

    private bool IsInCurrentRange(Vector3 _position)
    {
        float _xPos = Mathf.Abs(_position.x - transform.position.x);
        float _zPos = Mathf.Abs(_position.z - transform.position.z);

        if (_xPos <= 50f && _zPos <= 50f)
        {
            return true;
        }
        return false;
    }

    private void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            // Shoot bullet
            Instantiate(bullet, bulletSpwnPoint.position, bulletSpwnPoint.rotation);
            elapsedTime = 0f;
        }
    }

    private void Explode()
    {
        // this.GetComponent<Rigidbody>().isKinematic = false;
        // float _randX = Random.Range(10f, 30f);
        // float _randZ = Random.Range(10f, 30f);
        // for (int i = 0; i < 3; i++)
        // {
        //     rigidbody.AddExplosionForce(10000f, transform.position - new Vector3(_randX, 10f, _randZ), 40f, 10f);
        //     rigidbody.velocity = transform.TransformDirection(new Vector3(_randX, 20f, _randZ));
        // }
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject, 1.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }
}
