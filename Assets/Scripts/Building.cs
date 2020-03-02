using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public int teamIndex = 0;
    public int Range = 8;
    public PartOfPlanet partOfPlanet;

    public abstract bool CheckCooldown();

    public abstract void Attack(PartOfPlanet partOfPlanet);

    public void LossBuilding() {
        GameObject planet = GameObject.FindGameObjectWithTag("Planet");
        planet.GetComponent<Teams>().teams[teamIndex].buildings.Remove(this);
        Destroy(gameObject);
    }
}
