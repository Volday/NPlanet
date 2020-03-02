using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFormer : MonoBehaviour
{
    private GameObject planet;
    public List<PartOfPlanet> partsOfPlanet = new List<PartOfPlanet>();

    void Awake()
    {
        planet = gameObject;
        for (int t = 0; t < planet.transform.childCount; t++) {
            PartOfPlanet partOfPlanet = new PartOfPlanet();
            planet.transform.GetChild(t).gameObject.AddComponent<MeshCollider>();
            partOfPlanet.SetPart(planet.transform.GetChild(t).gameObject);
            partsOfPlanet.Add(partOfPlanet);
        }

        for (int t = 0; t < partsOfPlanet.Count; t++) {
            partsOfPlanet[t].FindANeighbors(partsOfPlanet);
        }
    }

    public PartOfPlanet FindPartsOfPlanet(GameObject obj) {
        for (int t = 0; t < partsOfPlanet.Count; t++) {
            if (partsOfPlanet[t].GetPart() == obj) {
                return partsOfPlanet[t];
            }
        }
        return null;
    }
}
