using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelPort : wheel
{
    public GameObject portFull;

    public override void Create(PartOfPlanet partOfPlanet, int teamIndex)
    {
        if (partOfPlanet.GetNeighbors()[0].teamIndex == -1 || partOfPlanet.GetNeighbors()[1].teamIndex == -1 || partOfPlanet.GetNeighbors()[2].teamIndex == -1) {
            GameObject planet = GameObject.FindGameObjectWithTag("Planet");
            if (planet.GetComponent<Teams>().teams[partOfPlanet.teamIndex].money >= 100000)
            {
                planet.GetComponent<Teams>().teams[partOfPlanet.teamIndex].money -= 100000;
                portFull = planet.GetComponent<Teams>().portFull;
                GameObject created = Instantiate(portFull, partOfPlanet.GetPart().transform.position, Quaternion.identity);
                planet.GetComponent<Teams>().teams[partOfPlanet.teamIndex].buildings.Add(created.GetComponent<Building>());
                created.GetComponent<Port>().partOfPlanet = partOfPlanet;
                partOfPlanet.full = true;
                partOfPlanet.building = created.GetComponent<Building>();
                created.GetComponent<Building>().partOfPlanet = partOfPlanet;
                created.GetComponent<Building>().teamIndex = teamIndex;
            }
        }
    }
}
