using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{

    [SerializeField]
    private float destructTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destructTime);
    }
}
