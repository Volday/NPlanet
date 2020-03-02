using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : Building
{
    GameObject planet;
    GameObject ship;
    public float cooldown = 10;
    public float currentCooldown = 0;
    public PartOfPlanet partOfPlanet;
    public PartOfPlanet partOfPlanetForSpawn;
    public Ship carrier;

    private void Start()
    {
        Respawn();
    }

    public override bool CheckCooldown()
    {
        if (currentCooldown > cooldown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if (currentCooldown > cooldown)
        {
            if (ship == null) {
                Respawn();
            }
        }
        else {
            currentCooldown += Time.deltaTime;
        }
    }

    public void Respawn() {
        planet = GameObject.FindGameObjectWithTag("Planet");
        ship = planet.GetComponent<Teams>().ship;
        if (partOfPlanet.GetNeighbors()[0].teamIndex == -1)
        {
            partOfPlanetForSpawn = partOfPlanet.GetNeighbors()[0];
        }
        else
        {
            if (partOfPlanet.GetNeighbors()[1].teamIndex == -1)
            {
                partOfPlanetForSpawn = partOfPlanet.GetNeighbors()[1];
            }
            else
            {
                partOfPlanetForSpawn = partOfPlanet.GetNeighbors()[2];
            }
        }

        ship = Instantiate(ship, partOfPlanetForSpawn.GetPart().transform.position, Quaternion.identity);
        carrier = ship.GetComponent<Ship>();
        carrier.teamIndex = teamIndex;
        ship.GetComponent<Ship>().Spawn(partOfPlanetForSpawn, this);
        planet.GetComponent<Teams>().teams[teamIndex].carriers.Add(carrier);
        carrier.rngMoveCooldown = Random.Range(1, 20);
    }

    public override void Attack(PartOfPlanet partOfPlanet)
    {
        
    }
}
