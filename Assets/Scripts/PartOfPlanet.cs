using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfPlanet
{
    private GameObject part;
    public int teamIndex = -1;
    private List<PartOfPlanet> neighbors = new List<PartOfPlanet>();
    public bool full = false;
    public Building building;
    public Country country;

    public void SetPart(GameObject newPart)
    {
        part = newPart;
        //part.AddComponent<MeshCollider>();
    }

    public GameObject GetPart()
    {
        return part;
    }

    public List<PartOfPlanet> GetNeighbors()
    {
        return neighbors;
    }

    public void FindANeighbors(List<PartOfPlanet> partsOfPlanets)
    {
        float neighbor1 = float.MaxValue;
        float neighbor2 = float.MaxValue;
        float neighbor3 = float.MaxValue;
        int neighbor1Index = -1;
        int neighbor2Index = -1;
        int neighbor3Index = -1;

        int myIndex = partsOfPlanets.IndexOf(this);

        for (int t = 0; t < partsOfPlanets.Count; t++)
        {
            if (myIndex != t)
            {
                bool trueNeighbor = false;
                float currentDistance = MyMath.sqrDistanceFromPointToPoint(part.transform.position, partsOfPlanets[t].part.transform.position);
                if (currentDistance < neighbor3)
                {
                    trueNeighbor = true;
                    neighbor3 = currentDistance;
                    neighbor3Index = t;
                }

                if (trueNeighbor)
                {
                    if (neighbor3 < neighbor2)
                    {
                        float nt = neighbor2;
                        neighbor2 = neighbor3;
                        neighbor3 = nt;
                        nt = neighbor2Index;
                        neighbor2Index = neighbor3Index;
                        neighbor3Index = (int)nt;
                        if (neighbor2 < neighbor1)
                        {
                            nt = neighbor1;
                            neighbor1 = neighbor2;
                            neighbor2 = nt;
                            nt = neighbor1Index;
                            neighbor1Index = neighbor2Index;
                            neighbor2Index = (int)nt;
                        }
                    }
                }
            }
        }
        neighbors.Add(partsOfPlanets[neighbor1Index]);
        neighbors.Add(partsOfPlanets[neighbor2Index]);
        neighbors.Add(partsOfPlanets[neighbor3Index]);
    }
}
