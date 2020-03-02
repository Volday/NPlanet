using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class wheel : MonoBehaviour
{
    public abstract void Create(PartOfPlanet partOfPlanet, int teamIndex);
}
