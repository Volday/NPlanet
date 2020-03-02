using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ship : MonoBehaviour
{
    public float Range = 7;
    public int teamIndex;
    public PartOfPlanet currentPart;
    List<PartOfPlanet> toMove = new List<PartOfPlanet>();
    float cooldown = 15;
    float currentCooldown = 15;
    GameObject planet;
    GameObject airplane;
    public Port port;
    GameObject airplaneOwn;
    public float rngMoveCooldown = 10;

    private void Awake()
    {
        planet = GameObject.FindGameObjectWithTag("Planet");
        airplane = planet.GetComponent<Teams>().airplane;
        airplaneOwn = gameObject.GetComponentInChildren<Airplain>().gameObject;
        airplaneOwn.GetComponent<Airplain>().enabled = false;
    }

    public void Start()
    {
        gameObject.GetComponentInChildren<FlagColor>().gameObject.GetComponent<MeshRenderer>().material
            = planet.GetComponent<Teams>().teams[teamIndex].GetPartsOfPlanet()[0].GetPart().GetComponent<MeshRenderer>().material;
    }

    public void LossShip() {
        if (port != null)
        {
            port.currentCooldown = 0;
        }
        planet.GetComponent<Teams>().teams[teamIndex].carriers.Remove(this);
        Destroy(gameObject);
    }

    public PartOfPlanet ClosestPartOfPlanet() {
        planet = GameObject.FindGameObjectWithTag("Planet");
        float minValue = float.MaxValue;
        int bestIndex = 0;
        for (int t = 0; t < planet.GetComponent<Teams>().water.Count; t++) {
            float currentValue = MyMath.sqrDistanceFromPointToPoint(planet.GetComponent<Teams>().water[t].GetPart().transform.position, transform.position);
            if (currentValue < minValue) {
                bestIndex = t;
                minValue = currentValue;
            }
        }
        return planet.GetComponent<Teams>().water[bestIndex];
    }

    public void Attack(PartOfPlanet partOfPlanet)
    {
        if (cooldown < currentCooldown)
        {
            if (MyMath.sqrDistanceFromPointToPoint(partOfPlanet.GetPart().transform.position, transform.position) < Range * Range)
            {
                currentCooldown = 0;
                GameObject newAirplane = Instantiate(airplane, transform.position, Quaternion.identity);
                newAirplane.GetComponent<Airplain>().partOfPlanet = partOfPlanet;
                newAirplane.GetComponent<Airplain>().teamIndex = teamIndex;
                newAirplane.GetComponent<Move>().SetDirection(partOfPlanet.GetPart().gameObject.transform.position);
            }
        }
    }

    public void Spawn(PartOfPlanet partOfPlanet, Port myPort)
    {
        currentPart = partOfPlanet;
        transform.position = currentPart.GetPart().transform.position;
        port = myPort;
    }

    public void MoveToTarget(GameObject target)
    {
        toMove.Clear();
        PartOfPlanet partOfPlanetOfTarget = new PartOfPlanet();
        GameObject planet = GameObject.FindGameObjectWithTag("Planet");
        List<PartOfPlanet> water = planet.GetComponent<Teams>().water;
        Dictionary<PartOfPlanet, PartOfPlanet> roads = new Dictionary<PartOfPlanet, PartOfPlanet>();
        List<PartOfPlanet> passed = new List<PartOfPlanet>();
        Queue<PartOfPlanet> qp = new Queue<PartOfPlanet>();
        qp.Enqueue(currentPart);
        bool alreadyFind = false;
        PartOfPlanet currentPartAtQP = qp.Peek();
        passed.Add(currentPartAtQP);
        while (qp.Count > 0) {
            currentPartAtQP = qp.Dequeue();
            for (int t = 0; t < 3; t++) {
                if (currentPartAtQP.GetNeighbors()[t].GetPart() == target)
                {
                    alreadyFind = true;
                    partOfPlanetOfTarget = currentPartAtQP.GetNeighbors()[t];
                    roads.Add(currentPartAtQP.GetNeighbors()[t], currentPartAtQP);
                    break;
                }
                else {
                    if (currentPartAtQP.GetNeighbors()[t].teamIndex == -1 && passed.IndexOf(currentPartAtQP.GetNeighbors()[t]) == -1)
                    {
                        qp.Enqueue(currentPartAtQP.GetNeighbors()[t]);
                        roads.Add(currentPartAtQP.GetNeighbors()[t], currentPartAtQP);
                        passed.Add(currentPartAtQP.GetNeighbors()[t]);
                    }
                }
            }
            if (alreadyFind) {
                break;
            }
        }
        if (alreadyFind) {
            while (true) {
                if (roads.ContainsKey(partOfPlanetOfTarget) && roads[partOfPlanetOfTarget] != null)
                {
                    toMove.Add(roads[partOfPlanetOfTarget]);
                    partOfPlanetOfTarget = roads[partOfPlanetOfTarget];
                }
                else {
                    break;
                }
            }
        }
    }

    private void Update()
    {
        currentCooldown += Time.deltaTime;
        rngMoveCooldown -= Time.deltaTime;
        if (GetComponent<Move>().target == Vector3.zero && toMove.Count != 0) {
            PartOfPlanet nextPartOfPlanet = toMove[toMove.Count - 1];
            toMove.RemoveAt(toMove.Count - 1);
            GetComponent<Move>().SetDirection(nextPartOfPlanet.GetPart().transform.position);
            currentPart = nextPartOfPlanet;
        }
        if (currentCooldown < cooldown)
        {
            airplaneOwn.SetActive(false);
        }
        else
        {
            airplaneOwn.SetActive(true);
        }
        if (teamIndex != 0 && rngMoveCooldown < 0) {
            MoveToTarget(planet.GetComponent<Teams>().Mix(planet.GetComponent<Teams>().water)
                [Random.Range(0, planet.GetComponent<Teams>().water.Count)].GetPart());
            rngMoveCooldown = Random.Range(1, 20);
        }
    }
}
