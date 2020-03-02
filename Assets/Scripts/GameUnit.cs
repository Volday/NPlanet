using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameUnit : MonoBehaviour
{
    public PartOfPlanet currentPart;

    public abstract void MoveToTarget(GameObject target);

}
