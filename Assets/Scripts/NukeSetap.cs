using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeSetap : Building
{
    GameObject planet;
    GameObject nuke;
    public float cooldown = 10;
    public float currentCooldown = 10;
    GameObject nukeOwn;

    private void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet");
        nuke = planet.GetComponent<Teams>().nuke;
        nukeOwn = gameObject.GetComponentInChildren<Nuke>().gameObject;
    }

    private void Update()
    {
        currentCooldown += Time.deltaTime;
        if (currentCooldown < cooldown)
        {
            nukeOwn.SetActive(false);
        }
        else
        {
            nukeOwn.SetActive(true);
        }
    }

    public override void Attack(PartOfPlanet partOfPlanet)
    {
        if (cooldown < currentCooldown)
        {
            if (MyMath.sqrDistanceFromPointToPoint(partOfPlanet.GetPart().transform.position, transform.position) < Range * Range)
            {
                currentCooldown = 0;
                GameObject newNuke = Instantiate(nuke, transform.position, Quaternion.identity);
                newNuke.GetComponent<Nuke>().partOfPlanet = partOfPlanet;
                newNuke.GetComponent<Nuke>().teamIndex = teamIndex;
                newNuke.GetComponent<Move>().SetDirection(partOfPlanet.GetPart().gameObject.transform.position);
            }
        }
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
}
