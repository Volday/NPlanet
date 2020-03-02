using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    int teamIndex = 0;

    public bool countrySelect = true;
    public float countrySelectColorShift = 0.05f;
    public Material selectedMaterial;
    private Country currentSelectedObject;
    private Material currentSelectedMaterial;
    private List<PartOfPlanet> selectedCountry;
    public Text selectedCountryPopulation;
    public Text money;

    public GameObject attackRangeSphire;
    private GameObject activAttackRangeSphire;
    public float rangeScale = 0.8f;

    LayerMask layer;
    LayerMask unitsLayer;

    public GameObject Menu;
    public GameObject selectedUnit;
    Camera cam;
    GameObject planet;
    List<PartOfPlanet> water;

    public GameObject Wheel;

    public GameObject currentWheel;
    public PartOfPlanet wheelPartOfPlanet;

    public GameObject youSuck;
    public GameObject perfect;

    private bool gameEnd = false;

    private float timer = 5;

    private void Start()
    {
        activAttackRangeSphire = Instantiate(attackRangeSphire, new Vector3(0,0,0), Quaternion.identity);
        planet = GameObject.FindGameObjectWithTag("Planet");
        water = planet.GetComponent<Teams>().water;
        cam = GetComponent<CameraRotation>().cam;
        layer = LayerMask.GetMask("Surface");
        unitsLayer = LayerMask.GetMask("YourUnit");
    }

    void Update()
        {

        if (!gameEnd)
        {

            if (planet.GetComponent<Teams>().teams[0].countries.Count <= 0)
            {
                youSuck.SetActive(true);
                gameEnd = true;
            }

            bool endTest = true;
            for (int t = 0; t < planet.GetComponent<Teams>().teams.Length; t++)
            {
                if (planet.GetComponent<Teams>().teams[t].countries.Count > 0)
                {
                    endTest = false;
                    break;
                }
            }

            if (endTest)
            {
                perfect.SetActive(true);
                gameEnd = true;
            }
        }
        else {
            if (timer > 0) {
                timer -= Time.deltaTime;
            }
        }

        if (timer <= 0) {
            perfect.SetActive(false);
            youSuck.SetActive(false);
        }

        money.text = "Money: " + planet.GetComponent<Teams>().teams[teamIndex].money;
        selectedCountryPopulation.text = "Selected Country Population: ";

        if (countrySelect) {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                GameObject target = hit.transform.gameObject;
                PartOfPlanet targetPartOfPlanet = planet.GetComponent<PlanetFormer>().FindPartsOfPlanet(target);
                if (targetPartOfPlanet != null && targetPartOfPlanet.teamIndex >= 0)
                {
                    if (currentSelectedObject != null) {
                        selectedCountryPopulation.text = "Selected Country Population: " + (int)currentSelectedObject.population;
                    }
                    if (currentSelectedObject != null && currentSelectedObject.parts.Contains(targetPartOfPlanet))
                    {

                    }
                    else
                    {
                        if (selectedCountry != null)
                        {
                            for (int t = 0; t < selectedCountry.Count; t++)
                            {
                                selectedCountry[t].GetPart().GetComponent<MeshRenderer>().sharedMaterial = planet.GetComponent<Teams>().teams[selectedCountry[t].teamIndex].material;
                            }
                        }
                        currentSelectedObject = targetPartOfPlanet.country;
                        selectedCountry = currentSelectedObject.parts;
                        currentSelectedMaterial = target.GetComponent<MeshRenderer>().sharedMaterial;
                        Vector4 newColor = new Vector4(0, 0, 0, 1);
                        newColor.x = currentSelectedMaterial.color.r + countrySelectColorShift;
                        newColor.y = currentSelectedMaterial.color.g + countrySelectColorShift;
                        newColor.z = currentSelectedMaterial.color.b + countrySelectColorShift;
                        selectedMaterial.SetColor("_BaseColor", newColor);
                        selectedMaterial.SetColor("_Color", newColor);
                        for (int t = 0; t < selectedCountry.Count; t++)
                        {
                            selectedCountry[t].GetPart().GetComponent<MeshRenderer>().sharedMaterial = selectedMaterial;
                        }
                    }
                }
                else {
                    currentSelectedObject = null;
                    if (selectedCountry != null)
                    {
                        for (int t = 0; t < selectedCountry.Count; t++)
                        {
                            selectedCountry[t].GetPart().GetComponent<MeshRenderer>().sharedMaterial = planet.GetComponent<Teams>().teams[selectedCountry[t].teamIndex].material;
                        }
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ && selectedUnit != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.GetComponentInParent<Teams>() != null)
                    {
                        selectedUnit = null;
                        Destroy(currentWheel);
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ)
        {
            if (Input.GetMouseButtonDown(1) && selectedUnit == null)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                {
                    GameObject target = hit.transform.gameObject;
                    PartOfPlanet targetPartOfPlanet = planet.GetComponent<PlanetFormer>().FindPartsOfPlanet(target);
                    if (targetPartOfPlanet.teamIndex == 0 && !targetPartOfPlanet.full)
                    {
                        GameObject newWheel = Instantiate(Wheel, targetPartOfPlanet.GetPart().transform.position, Quaternion.identity);
                        if (currentWheel != null)
                        {
                            Destroy(currentWheel);
                        }
                        selectedUnit = null;
                        currentWheel = newWheel;
                        wheelPartOfPlanet = targetPartOfPlanet;
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ && selectedUnit != null && selectedUnit.GetComponent<Ship>() != null && selectedUnit.GetComponent<Ship>().teamIndex == 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                {
                    GameObject target = hit.transform.gameObject;
                    PartOfPlanet targetPartOfPlanet = planet.GetComponent<PlanetFormer>().FindPartsOfPlanet(target);
                    bool itsAShip = false;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.gameObject.GetComponent<Ship>() != null
                        && hit.transform.gameObject.GetComponent<Ship>().teamIndex == teamIndex)
                    {
                        itsAShip = true;
                    }
                    if (target.GetComponentInParent<Teams>() != null && (targetPartOfPlanet.teamIndex != -1 || itsAShip))
                    {
                        selectedUnit.GetComponent<Ship>().Attack(targetPartOfPlanet);
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ && selectedUnit != null && selectedUnit.GetComponent<Ship>() != null && selectedUnit.GetComponent<Ship>().teamIndex == 0)
        {
            if (Input.GetMouseButtonDown(2))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                {
                    GameObject target = hit.transform.gameObject;
                    PartOfPlanet targetPartOfPlanet = planet.GetComponent<PlanetFormer>().FindPartsOfPlanet(target);
                    if (target.GetComponentInParent<Teams>() != null)
                    {
                        selectedUnit.GetComponent<Ship>().Attack(targetPartOfPlanet);
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ && selectedUnit != null && selectedUnit.GetComponent<NukeSetap>() != null && selectedUnit.GetComponent<NukeSetap>().teamIndex == 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.GetComponentInParent<Teams>() != null)
                    {
                        PartOfPlanet targetPartOfPlanet = planet.GetComponent<PlanetFormer>().FindPartsOfPlanet(target);
                        selectedUnit.GetComponent<NukeSetap>().Attack(targetPartOfPlanet);
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ && selectedUnit != null && selectedUnit.GetComponent<Aeroport>() != null && selectedUnit.GetComponent<Aeroport>().teamIndex == 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.GetComponentInParent<Teams>() != null)
                    {
                        PartOfPlanet targetPartOfPlanet = planet.GetComponent<PlanetFormer>().FindPartsOfPlanet(target);
                        selectedUnit.GetComponent<Aeroport>().Attack(targetPartOfPlanet);
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ && currentWheel != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.GetComponent<wheel>() != null && wheelPartOfPlanet != null)
                    {
                        Destroy(currentWheel);
                        target.GetComponent<wheel>().Create(wheelPartOfPlanet, teamIndex);
                        wheelPartOfPlanet = null;
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ && selectedUnit != null && selectedUnit.GetComponent<Ship>() != null) {
            if (Input.GetMouseButtonDown(1)) {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer)) {
                    GameObject target = hit.transform.gameObject;
                    PartOfPlanet targetPartOfPlanet = planet.GetComponent<PlanetFormer>().FindPartsOfPlanet(target);
                    if (targetPartOfPlanet.teamIndex == -1 &&
                        selectedUnit.GetComponent<Ship>().currentPart != targetPartOfPlanet) {
                        selectedUnit.GetComponent<Ship>().MoveToTarget(target);
                    }
                }
            }
        }

        if (!Menu.GetComponent<Menu>().activ) {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitsLayer))
                {
                    GameObject target = hit.transform.gameObject;
                    if ((target.GetComponent<Building>() != null && target.GetComponent<Building>().teamIndex == teamIndex) 
                        || (target.GetComponent<Ship>() != null && target.GetComponent<Ship>().teamIndex == teamIndex))
                    {
                        selectedUnit = target;
                    }
                }
            }
        }

        if (selectedUnit != null)
        {
            activAttackRangeSphire.transform.position = selectedUnit.transform.position;
            activAttackRangeSphire.transform.localScale = new Vector3(1,1,1);
            activAttackRangeSphire.GetComponent<AutoRotate>().enabled = true;

            if (selectedUnit.GetComponent<Aeroport>() != null) {
                activAttackRangeSphire.transform.localScale = new Vector3(
                    selectedUnit.GetComponent<Aeroport>().Range, 
                    selectedUnit.GetComponent<Aeroport>().Range * rangeScale, 
                    selectedUnit.GetComponent<Aeroport>().Range) * 2;
            }
            if (selectedUnit.GetComponent<Ship>() != null)
            {
                activAttackRangeSphire.transform.localScale = new Vector3(
                    selectedUnit.GetComponent<Ship>().Range,
                    selectedUnit.GetComponent<Ship>().Range * rangeScale,
                    selectedUnit.GetComponent<Ship>().Range) * 2;
            }
            if (selectedUnit.GetComponent<NukeSetap>() != null)
            {
                activAttackRangeSphire.transform.localScale = new Vector3(
                    selectedUnit.GetComponent<NukeSetap>().Range,
                    selectedUnit.GetComponent<NukeSetap>().Range * rangeScale,
                    selectedUnit.GetComponent<NukeSetap>().Range) * 2;
            }
        }
        else {
            activAttackRangeSphire.GetComponent<AutoRotate>().enabled = false;
            activAttackRangeSphire.transform.localScale = new Vector3(1, 1, 1);
            activAttackRangeSphire.transform.position = Vector3.zero;
        }

        //if (!Menu.GetComponent<Menu>().activ)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        RaycastHit hit;
        //        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            GameObject target = hit.transform.gameObject;
        //            if (target.GetComponentInParent<Teams>() != null) {
        //                selectedUnit = null;
        //            }
        //        }
        //    }
        //}
    }
}
