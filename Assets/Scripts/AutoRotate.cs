using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float height;

    public bool StaticMesh;

    private void Start()
    {
        AutoRotation();
    }

    private void Update()
    {
        if (!StaticMesh) {
            AutoRotation();
        }
    }

    public void AutoRotation(){
        transform.rotation = Quaternion.FromToRotation(transform.up, transform.position.normalized) * transform.rotation;
        transform.position = transform.position * (height / Vector3.Distance(transform.position, Vector3.zero));
    }
}
