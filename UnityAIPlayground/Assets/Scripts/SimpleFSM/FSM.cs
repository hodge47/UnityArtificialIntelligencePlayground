using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{

    protected Transform playerTransform;
    protected Vector3 destPos;
    protected GameObject[] pointList;

    // Bullet Shooting rate
    protected float shootRate;
    protected float elapsedTime;

    // Tank turret
    public Transform turret { get; set; }
    public Transform bulletSpwnPoint { get; set; }

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}
