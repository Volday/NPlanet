using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Country
{
    public List<PartOfPlanet> parts = new List<PartOfPlanet>();
    public float population = 0;
    public List<int> challengersID = new List<int>();
    public List<int> challengersDamage = new List<int>();

    public void ChallengersUpdate(int damage, int challengerID) {
        if (!challengersID.Contains(challengerID)) {
            challengersID.Add(challengerID);
            challengersDamage.Add(0);
        }
        int currentID = challengersID.IndexOf(challengerID);
        challengersDamage[currentID] += damage;
    }

    public void LossCountry() {
        GameObject planet = GameObject.FindGameObjectWithTag("Planet");
        int teamIndex = parts[0].teamIndex;
        int maxDamage = -1;
        int topChallendger = -1;
        for (int t = 0; t < challengersDamage.Count; t++) {
            if (maxDamage < challengersDamage[t]) {
                maxDamage = challengersDamage[t];
                topChallendger = challengersID[t];
            }
        }

        for (int t = 0; t < planet.GetComponent<Teams>().teams[teamIndex].buildings.Count; t++) {
            if (planet.GetComponent<Teams>().teams[teamIndex].buildings[t] != null && parts.Contains(planet.GetComponent<Teams>().teams[teamIndex].buildings[t].partOfPlanet)) {
                planet.GetComponent<Teams>().teams[teamIndex].buildings[t].partOfPlanet.building = null;
                planet.GetComponent<Teams>().teams[teamIndex].buildings[t].partOfPlanet.full = false;
                planet.GetComponent<Teams>().teams[teamIndex].buildings[t].LossBuilding();
            }
        }

        for (int t = 0; t < parts.Count; t++) {
            planet.GetComponent<Teams>().teams[topChallendger].AddPartOfPlanet(parts[t]);
            planet.GetComponent<Teams>().teams[teamIndex].RemovePartOfPlanet(parts[t]);
        }

        planet.GetComponent<Teams>().teams[topChallendger].countries.Add(this);
        population = planet.GetComponent<Teams>().startPopulationAtPart * parts.Count;
        planet.GetComponent<Teams>().teams[topChallendger].population += planet.GetComponent<Teams>().startPopulationAtPart * parts.Count;
        planet.GetComponent<Teams>().teams[teamIndex].countries.Remove(this);

        for (int t = 0; t < planet.GetComponent<Teams>().teams.Length; t++)
        {
            planet.GetComponent<Teams>().teams[topChallendger].UpdateColor();
        }

        
    } 
}
