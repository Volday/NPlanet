using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    public float timeToDestroy = 0;

    void SetTimeToDestriy(float time) {
        timeToDestroy = time;
    }

    void Update()
    {
        if (timeToDestroy < 0) {
            Destroy(gameObject);
        }
        timeToDestroy -= Time.deltaTime;
    }
}
