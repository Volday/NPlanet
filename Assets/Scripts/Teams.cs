using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Teams : MonoBehaviour
{
    public float income = 0.5f;
    public float populationIncreace = 0.2f;

    public int bombDamage = 25000;
    public int nukeDamage = 35000;

    public int startMoney = 100000;
    public int maxPopulationAtPart;
    public int startPopulationAtPart = 3000;



    public GameObject aeroPort;
    public GameObject aeroPortFull;
    public GameObject airplane;

    public GameObject NukeExplosion;

    public GameObject ship;

    public GameObject nuke;

    public GameObject port;
    public GameObject portFull;
    public GameObject nukeSetap;
    public GameObject nukeSetapFull;

    public int startPartsCount;

    public int mainlandCount;

    public Team[] teams;

    public List<PartOfPlanet> water;

    List<PartOfPlanet> mainland;

    void Start()
    {
        for (int t = 0; t < teams.Length; t++) {
            teams[t].material.SetColor("o",teams[t].color);
            teams[t].teamIndex = t;
        }

        PlanetFormer planetFormer = gameObject.GetComponent<PlanetFormer>();

        mainland = new List<PartOfPlanet>();
        water = new List<PartOfPlanet>();
        for (int t = 0; t < planetFormer.partsOfPlanet.Count; t++) {
            water.Add(planetFormer.partsOfPlanet[t]);
        }
        Queue<PartOfPlanet> qp = new Queue<PartOfPlanet>();
        for (int t = 0; t < startPartsCount; t++) {
            int newIndex = Random.Range(0, planetFormer.partsOfPlanet.Count);
            if (mainland.IndexOf(planetFormer.partsOfPlanet[newIndex]) == -1)
            {
                mainland.Add(planetFormer.partsOfPlanet[newIndex]);
                water.Remove(planetFormer.partsOfPlanet[newIndex]);
                qp.Enqueue(planetFormer.partsOfPlanet[newIndex]);
            }
            else {
                t--;
            }
        }

        for (int t = 0; t < mainlandCount - startPartsCount; t++)
        {
            if (qp.Count > 0)
            {
                PartOfPlanet partOfPlanet = qp.Dequeue();
                bool neighborsCheck = false;
                for (int i = 0; i < partOfPlanet.GetNeighbors().Count; i++)
                {
                    if (mainland.IndexOf(partOfPlanet.GetNeighbors()[i]) == -1)
                    {
                        neighborsCheck = true;
                        if (Random.Range(0, 2) == 0)
                        {
                            mainland.Add(partOfPlanet.GetNeighbors()[i]);
                            water.Remove(partOfPlanet.GetNeighbors()[i]);
                            qp.Enqueue(partOfPlanet.GetNeighbors()[i]);
                        }
                        else {
                            t--;
                        }
                        qp.Enqueue(partOfPlanet);
                        break;
                    }
                }
                if (!neighborsCheck) {
                    t--;
                }
            }
            else {
                break;
            }
        }

        for (int t = 0; t < teams.Length; t++) {
            teams[t].population = 0;
            int rngIndex = Random.Range(0, mainland.Count);
            PartOfPlanet startPartOfPlanet = mainland[rngIndex];
            teams[t].SetPartsOfPlanet(startPartOfPlanet, mainland, mainlandCount / teams.Length - 1, t);
        }

        for (int t = 0; t < teams.Length; t++) {
            for (int i = 0; i < teams[t].GetPartsOfPlanet().Count; i++)
            {
                teams[t].GetPartsOfPlanet()[i].GetPart().GetComponent<MeshRenderer>().sharedMaterial = teams[t].material;
            }
        }

        for (int t = 0; t < teams.Length; t++)
        {
            teams[t].UpdateCountries(teams);
            teams[t].countries.Clear();
        }

        for (int t = 0; t < teams.Length; t++)
        {
            teams[t].UpdateCountries(teams);
            teams[t].countries.Clear();
        }

        for (int t = 0; t < teams.Length; t++)
        {
            teams[t].UpdateCountries(teams);
        }

        for (int t = 0; t < teams.Length; t++)
        {
            teams[t].money = startMoney;
            if (t != 0) {
                teams[t].money -= Random.Range(1000, 15000);
            }
            for (int i = 0; i < teams[t].countries.Count; i++)
            {
                teams[t].countries[i].population = startPopulationAtPart * teams[t].countries[i].parts.Count;
                teams[t].population += startPopulationAtPart * teams[t].countries[i].parts.Count;
            }
        }

        for (int t = 0; t < teams.Length; t++)
        {
            teams[t].UpdateColor();
        }
    }

    private void Update()
    {
        for (int t = 0; t < teams.Length; t++) {
            if (teams[t].money < 300000 && teams[t].population > 0)
            {
                teams[t].money += (int)(teams[t].population * income * Time.deltaTime);
            }
            for (int i = 0; i < teams[t].countries.Count; i++) {
                if (teams[t].countries[i].population < maxPopulationAtPart * teams[t].countries[i].parts.Count && teams[t].population > 0)
                {
                    teams[t].countries[i].population += teams[t].countries[i].population * populationIncreace * Time.deltaTime;
                    teams[t].population += teams[t].countries[i].population * populationIncreace * Time.deltaTime;
                }
            }
        }

        //Bots maind
        List<PartOfPlanet> allPartOfEnemies = new List<PartOfPlanet>();
        List<Ship> allCariers = new List<Ship>();
        for (int t = 1; t < teams.Length; t++)
        {
            if (teams[t].population > 0 && teams[t].GetPartsOfPlanet().Count > 0)
            {
                if (teams[t].money > 50000)
                {
                    int num = Random.Range(0, teams[t].GetPartsOfPlanet().Count - 1);
                    if (!teams[t].GetPartsOfPlanet()[num].full)
                    {
                        teams[t].GetPartsOfPlanet()[num].full = true;
                        if (num % 3 == 0)
                        {
                            wheelAeroport wheelA = gameObject.GetComponent<wheelAeroport>();
                            wheelA.Create(teams[t].GetPartsOfPlanet()[num], t);
                        }
                        if (num % 3 == 1)
                        {
                            teams[t].money += 25000;
                            wheelNuke wheelN = gameObject.GetComponent<wheelNuke>();
                            wheelN.Create(teams[t].GetPartsOfPlanet()[num], t);
                            teams[t].money -= 25000;
                        }
                        if (num % 3 == 2)
                        {
                            teams[t].money += 50000; 
                            wheelPort wheelP = gameObject.GetComponent<wheelPort>();
                            for (int i = 0; i < teams[t].GetPartsOfPlanet().Count; i++) {
                                if (!teams[t].GetPartsOfPlanet()[i].full && (
                                    teams[t].GetPartsOfPlanet()[i].GetNeighbors()[0].teamIndex == -1 ||
                                    teams[t].GetPartsOfPlanet()[i].GetNeighbors()[1].teamIndex == -1 ||
                                    teams[t].GetPartsOfPlanet()[i].GetNeighbors()[2].teamIndex == -1)) {
                                    wheelP.Create(teams[t].GetPartsOfPlanet()[i], t);
                                }
                            }
                            teams[t].money -= 50000;
                        }
                    }
                }

                for (int i = 0; i < teams.Length; i++)
                {
                    if (i != t)
                    {
                        for (int j = 0; j < teams[i].GetPartsOfPlanet().Count; j++)
                        {
                            allPartOfEnemies.Add(teams[i].GetPartsOfPlanet()[j]);
                        }
                    }
                }
                allPartOfEnemies = Mix(allPartOfEnemies);

                for (int i = 0; i < teams.Length; i++)
                {
                    if (i != t)
                    {
                        for (int j = 0; j < teams[i].carriers.Count; j++)
                        {
                            allCariers.Add(teams[i].carriers[j]);
                        }
                    }
                }
                allCariers = MixC(allCariers);

                if (Random.Range(0, 3) == 0)
                {
                    for (int i = 0; i < teams[t].carriers.Count; i++)
                    {
                        for (int j = 0; j < allCariers.Count; j++)
                        {
                            if (MyMath.sqrDistanceFromPointToPoint(allCariers[j].gameObject.transform.position,
                                teams[t].carriers[i].transform.position)
                                < teams[t].carriers[i].Range * teams[t].carriers[i].Range && allCariers[j].teamIndex != t)
                            {
                                teams[t].carriers[i].Attack(allCariers[j].ClosestPartOfPlanet());
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < teams[t].buildings.Count; i++)
                    {
                        if (teams[t].buildings[i].GetComponent<Port>() != null && teams[t].buildings[i].GetComponent<Port>().carrier != null)
                        {

                        }
                        if (teams[t].buildings[i].CheckCooldown())
                        {
                            for (int j = 0; j < allCariers.Count; j++)
                            {
                                if (MyMath.sqrDistanceFromPointToPoint(allCariers[j].gameObject.transform.position,
                                    teams[t].buildings[i].transform.position)
                                    < teams[t].buildings[i].Range * teams[t].buildings[i].Range && allCariers[j].teamIndex != t)
                                {
                                    teams[t].buildings[i].Attack(allCariers[j].ClosestPartOfPlanet());
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < teams[t].carriers.Count; i++)
                    {
                        for (int j = 0; j < allPartOfEnemies.Count; j++)
                        {
                            if (MyMath.sqrDistanceFromPointToPoint(allPartOfEnemies[j].GetPart().transform.position,
                                teams[t].carriers[i].transform.position)
                                < teams[t].carriers[i].Range * teams[t].carriers[i].Range && allPartOfEnemies[j].teamIndex != t)
                            {
                                teams[t].carriers[i].Attack(allPartOfEnemies[j]);
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < teams[t].buildings.Count; i++)
                    {
                        if (teams[t].buildings[i].GetComponent<Port>() != null && teams[t].buildings[i].GetComponent<Port>().carrier != null) {

                        }
                        if (teams[t].buildings[i].CheckCooldown())
                        {
                            for (int j = 0; j < allPartOfEnemies.Count; j++)
                            {
                                if (MyMath.sqrDistanceFromPointToPoint(allPartOfEnemies[j].GetPart().transform.position,
                                    teams[t].buildings[i].transform.position)
                                    < teams[t].buildings[i].Range * teams[t].buildings[i].Range && allPartOfEnemies[j].teamIndex != t)
                                {
                                    teams[t].buildings[i].Attack(allPartOfEnemies[j]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    public List<PartOfPlanet> Mix(List<PartOfPlanet> partOfPlanet) {
        for (int i = partOfPlanet.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = partOfPlanet[j];
            partOfPlanet[j] = partOfPlanet[i];
            partOfPlanet[i] = temp;
        }
        return partOfPlanet;
    }

    public List<Ship> MixC(List<Ship> ships)
    {
        for (int i = ships.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = ships[j];
            ships[j] = ships[i];
            ships[i] = temp;
        }
        return ships;
    }

    [System.Serializable]
    public class Team {

        public int money = 0;
        public float population = 0;

        public List<Building> buildings = new List<Building>();
        public List<Ship> carriers = new List<Ship>();

        public int teamIndex;
        public Vector3 startPoint;
        public Color color;
        public Material material;
        private List<PartOfPlanet> partsOfPlanet = new List<PartOfPlanet>();

        private bool firstCountriesUpdate = false;

        public List<Country> countries = new List<Country>();

        public void TakeDamage(int damage, PartOfPlanet partOfPlanet, int aggressorTeamIndex) {
            int countryId = countries.IndexOf(partOfPlanet.country);
            if (countries[countryId].population < damage)
            {
                population -= countries[countryId].population;
            }
            else
            {
                population -= damage;
            }
            countries[countryId].population -= damage;

            countries[countryId].ChallengersUpdate(damage, aggressorTeamIndex);

            if (countries[countryId].population <= 0) {
                countries[countryId].LossCountry();
            }
        }

        public void AddPartOfPlanet(PartOfPlanet newPartOfPlanet)
        {
            partsOfPlanet.Add(newPartOfPlanet);
        }

        public void RemovePartOfPlanet(PartOfPlanet partOfPlanet)
        {
            if (partsOfPlanet.Contains(partOfPlanet)) {
                partsOfPlanet.Remove(partOfPlanet);
            }
        }

        public void UpdateColor() {
            for (int t = 0; t < partsOfPlanet.Count; t++)
            {
                partsOfPlanet[t].teamIndex = teamIndex;
                partsOfPlanet[t].GetPart().GetComponent<MeshRenderer>().sharedMaterial = material;
            }
        }

        public void UpdateCountries(Team[] teams) {
            List<PartOfPlanet> partsOfPlanetPast = new List<PartOfPlanet>();

            for (int t = 0; t < partsOfPlanet.Count; t++)
            {
                if (partsOfPlanetPast.IndexOf(partsOfPlanet[t]) == -1) {
                    Country newCountry = new Country();

                    UpdateCountriesDFS(partsOfPlanetPast, newCountry, partsOfPlanet[t]);
                    countries.Add(newCountry);
                }
            }

            if (firstCountriesUpdate)
            {
                for (int t = 0; t < countries.Count; t++)
                {
                    if (countries[t].parts.Count == 1)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (countries[t].parts[0].GetNeighbors()[i].teamIndex != -1)
                            {
                                countries[t].parts[0].teamIndex = countries[t].parts[0].GetNeighbors()[i].teamIndex;
                                teams[countries[t].parts[0].GetNeighbors()[i].teamIndex].AddPartOfPlanet(countries[t].parts[0]);
                                partsOfPlanet.Remove(countries[t].parts[0]);
                                countries.RemoveAt(t);
                                break;
                            }
                        }
                    }
                }
                firstCountriesUpdate = false;
            }
            else {
                firstCountriesUpdate = true;
            }
        }

        public void UpdateCountriesDFS(List<PartOfPlanet> partsOfPlanetPast, Country newCountry, PartOfPlanet current)
        {
            newCountry.parts.Add(current);
            current.country = newCountry;
            partsOfPlanetPast.Add(current);
            for (int t = 0; t < current.GetNeighbors().Count; t++)
            {
                if (partsOfPlanetPast.IndexOf(current.GetNeighbors()[t]) == -1 && partsOfPlanet.IndexOf(current.GetNeighbors()[t]) != -1) {
                    UpdateCountriesDFS(partsOfPlanetPast, newCountry, current.GetNeighbors()[t]);
                }
            }
        }

        public Color GetColor() {
            return color;
        }

        public List<PartOfPlanet> GetPartsOfPlanet() {
            return partsOfPlanet;
        }

        public void SetPartsOfPlanet(PartOfPlanet startPart, List<PartOfPlanet> mainland, int partCount, int teamIndex) {
            mainland.Remove(startPart);
            partsOfPlanet.Add(startPart);
            startPart.teamIndex = teamIndex;

            startPoint = startPart.GetPart().transform.position;

            Dictionary<float, PartOfPlanet> distanceAndParts = new Dictionary<float, PartOfPlanet>();

            float[] parts = new float[mainland.Count];
            for (int t = 0; t < mainland.Count; t++) {
                float distance = MyMath.sqrDistanceFromPointToPoint(startPart.GetPart().transform.position, mainland[t].GetPart().transform.position);
                if (distanceAndParts.ContainsKey(distance)) {
                    distance = distance + 0.00101f;
                }
                parts[t] = distance;
                distanceAndParts.Add(distance, mainland[t]);
            }
            Array.Sort(parts);
            for (int t = 0; t < partCount; t++)
            {
                partsOfPlanet.Add(distanceAndParts[parts[t]]);
                distanceAndParts[parts[t]].teamIndex = teamIndex;
                mainland.Remove(distanceAndParts[parts[t]]); 
            }
        }
    }
}
