using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 offset;

    void Start()
    {
        if (target) {
            offset = transform.position - target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) {
            return;
        }

        transform.position = target.position + offset;
    }
}
