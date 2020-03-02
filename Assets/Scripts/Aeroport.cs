using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aeroport : Building
{
    GameObject planet;
    GameObject airplane;
    public float cooldown = 15;
    public float currentCooldown = 15;
    GameObject airplaneOwn;

    private void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet");
        airplane = planet.GetComponent<Teams>().airplane;
        airplaneOwn = gameObject.GetComponentInChildren<Airplain>().gameObject;
    }

    private void Update()
    {
        currentCooldown += Time.deltaTime;
        if (currentCooldown < cooldown)
        {
            airplaneOwn.SetActive(false);
        }
        else {
            airplaneOwn.SetActive(true);
        }
    }

    public override void Attack(PartOfPlanet partOfPlanet) {
        if (cooldown < currentCooldown) {
            if (MyMath.sqrDistanceFromPointToPoint(partOfPlanet.GetPart().transform.position, transform.position) < Range * Range) {
                currentCooldown = 0;
                GameObject newAirplane = Instantiate(airplane, transform.position, Quaternion.identity);
                newAirplane.GetComponent<Airplain>().partOfPlanet = partOfPlanet;
                newAirplane.GetComponent<Airplain>().teamIndex = teamIndex;
                newAirplane.GetComponent<Move>().SetDirection(partOfPlanet.GetPart().gameObject.transform.position);
            }
        }
    }

    public override bool CheckCooldown()
    {
        if (currentCooldown > cooldown)
        {
            return true;
        }
        else {
            return false;
        }
    }
}
