using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : MonoBehaviour
{

    public GameObject bullet;

    [SerializeField]
    private Transform turret;
    [SerializeField]
    private Transform bulletSpawnPoint;
    private float curSpeed;
    private float targetSpeed;
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private float turretRotSpeed = 10f;
    [SerializeField]
    private float maxForwardSpeed = 25f;
    [SerializeField]
    private float maxBackwardSpeed = -25f;

    // Bullet shooting rate
    protected float shootRate = 0f;
    protected float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get the turret of the tank
        turret = gameObject.transform.GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeapon();
        UpdateControl();
    }

    private void UpdateWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (elapsedTime >= shootRate)
            {
                //Reset the time
                elapsedTime = 0.0f;

                //Also Instantiate over the PhotonNetwork
                Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
        }
    }

    private void UpdateControl()
    {
        // Aiming with the mouse
        // Generate a plane that intersects the transform's position with an upwards normal
        Plane playerPlane = new Plane(Vector3.up, transform.position += new Vector3(0, 0, 0));

        // Generate a ray from the cursor position
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane
        float hitDist = 0;
        // If the ray is parallel to the plane, raycast will return false
        if (playerPlane.Raycast(raycast, out hitDist))
        {
            // Get the point along the ray that hits the calculated distance
            Vector3 raycastHitPoint = raycast.GetPoint(hitDist);

            Quaternion targetRotation = Quaternion.LookRotation(raycastHitPoint - transform.position);

            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
        }

        // Tank movement - forward
        if (Input.GetKey(KeyCode.W))
        {
            targetSpeed = maxForwardSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetSpeed = maxBackwardSpeed;
        }
        else
        {
            targetSpeed = 0;
        }

        // Tank movement - turning
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
        }

        // Determine the current speed
        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, 7f * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }
}