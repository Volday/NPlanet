using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    public int teamIndex;
    public PartOfPlanet partOfPlanet;

    private void Update()
    {
        if (teamIndex != -2 && GetComponent<Move>().target == Vector3.zero)
        {
            GameObject planet = GameObject.FindGameObjectWithTag("Planet");
            Instantiate(planet.GetComponent<Teams>().NukeExplosion, transform.position, Quaternion.identity);

            if (partOfPlanet.teamIndex != teamIndex && partOfPlanet.teamIndex != -1)
            {
                planet.GetComponent<Teams>().teams[partOfPlanet.teamIndex].TakeDamage(planet.GetComponent<Teams>().nukeDamage, partOfPlanet, teamIndex);
            }

            List<GameObject> toDestroy = new List<GameObject>();
            for (int t = 0; t < planet.GetComponent<Teams>().teams.Length; t++)
            {
                for (int i = 0; i < planet.GetComponent<Teams>().teams[t].buildings.Count; i++)
                {
                    if (planet.GetComponent<Teams>().teams[t].buildings[i] != null
                        && planet.GetComponent<Teams>().teams[t].buildings[i].teamIndex != teamIndex
                        && MyMath.sqrDistanceFromPointToPoint(planet.GetComponent<Teams>().teams[t].buildings[i].transform.position,
                        partOfPlanet.GetPart().transform.position) < 4)
                    {
                        toDestroy.Add(planet.GetComponent<Teams>().teams[t].buildings[i].gameObject);
                    }
                }
            }

            for (int t = 0; t < planet.GetComponent<Teams>().teams.Length; t++)
            {
                for (int i = 0; i < planet.GetComponent<Teams>().teams[t].carriers.Count; i++)
                {
                    if (planet.GetComponent<Teams>().teams[t].carriers[i] != null
                        && planet.GetComponent<Teams>().teams[t].carriers[i].teamIndex != teamIndex
                        && MyMath.sqrDistanceFromPointToPoint(planet.GetComponent<Teams>().teams[t].carriers[i].transform.position,
                        partOfPlanet.GetPart().transform.position) < 5)
                    {
                        toDestroy.Add(planet.GetComponent<Teams>().teams[t].carriers[i].gameObject);

                    }
                }
            }

            for (int t = toDestroy.Count - 1; t > -1; t--)
            {
                if (toDestroy[t].GetComponent<Building>() != null)
                {
                    planet.GetComponent<Teams>().teams[toDestroy[t].GetComponent<Building>().teamIndex].buildings.Remove(toDestroy[t].GetComponent<Building>());
                    toDestroy[t].GetComponent<Building>().partOfPlanet.building = null;
                    toDestroy[t].GetComponent<Building>().partOfPlanet.full = false;
                    toDestroy[t].GetComponent<Building>().LossBuilding();
                    toDestroy.RemoveAt(t);
                }
                else
                {
                    if (toDestroy[t].GetComponent<Ship>() != null)
                    {
                        toDestroy[t].GetComponent<Ship>().LossShip();
                        toDestroy.RemoveAt(t);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}
