using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 0.1f;
    public Vector3 target = Vector3.zero;
    public GameObject target2;
    private float height;

    private bool movingTarget = false;

    private void Start()
    {
        height = gameObject.GetComponent<AutoRotate>().height;
    }

    void Update()
    {
        if (movingTarget) {
            target = target2.transform.position;
        }
        if (target != Vector3.zero) {
            Vector3 direction = target - transform.position;
            float distance = MyMath.sqrDistanceFromPointToPoint(target, transform.position);
            Vector3 nextStep = direction.normalized * Time.deltaTime * speed;
            float nextStepDistance = MyMath.sqrDistanceFromPointToPoint(nextStep, Vector3.zero);
            if (0.005f > distance)
            {
                transform.position = target;
                target = Vector3.zero;
            }
            else {
                transform.position = transform.position + nextStep;
            }
        }
    }

    public void SetDirection(Vector3 newTarget) {
        movingTarget = false;
        target = newTarget;
    }

    public void SetDirection(GameObject newTarget)
    {
        movingTarget = true;
        target2 = newTarget;
    }
}
