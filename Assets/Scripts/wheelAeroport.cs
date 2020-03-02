using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelAeroport : wheel
{
    public GameObject aeroportFull;

    public override void Create(PartOfPlanet partOfPlanet, int teamIndex)
    {
        GameObject planet = GameObject.FindGameObjectWithTag("Planet");
        if (planet.GetComponent<Teams>().teams[partOfPlanet.teamIndex].money >= 50000)
        {
            planet.GetComponent<Teams>().teams[partOfPlanet.teamIndex].money -= 50000;
            aeroportFull = planet.GetComponent<Teams>().aeroPortFull;
            GameObject created = Instantiate(aeroportFull, partOfPlanet.GetPart().transform.position, Quaternion.identity);
            planet.GetComponent<Teams>().teams[partOfPlanet.teamIndex].buildings.Add(created.GetComponent<Building>());
            partOfPlanet.full = true;
            partOfPlanet.building = created.GetComponent<Building>();
            created.GetComponent<Building>().partOfPlanet = partOfPlanet;
            created.GetComponent<Building>().teamIndex = teamIndex;
        }
    }
}
