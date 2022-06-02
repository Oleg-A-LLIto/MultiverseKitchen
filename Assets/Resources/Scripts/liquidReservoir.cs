using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class liquidReservoir : MonoBehaviour
{
    [SerializeField] GameObject sourceRotor;
    //[SerializeField] GameObject catcher;
    [SerializeField] GameObject liquidParticlePrefab;
    [SerializeField] float reservoirRadius;
    [SerializeField] float bottomOffset = 0.1f;
    [SerializeField] float pourAngleFull;
    [SerializeField] float pourAngleEmpty;
    [SerializeField] float ejectionPower;
    [SerializeField] float emptyLiquidLevel;
    [SerializeField] float fullLiquidLevel;
    [SerializeField] float emptyLiquidLevelFlipped;
    [SerializeField] float fullLiquidLevelFlipped;
    [SerializeField] float reservoirVolume;
    [SerializeField] Vector3 finalScale;
    [SerializeField] Renderer liquidRenderer;
    [SerializeField] string[] initialIngredients;
    [SerializeField] float[] ingredientRatios;
    public Dictionary<string, float> ingredients { get; private set; }
    Vector3 initialCatcherPosition;
    Vector3 initialCatcherScale;
    float pourAngleCurrent;
    float fullnessIncrement;
    public float fullness = 1;

    void Start()
    {
        fullnessIncrement = 1 / reservoirVolume;
        //initialCatcherPosition = sourceRotor.transform.localPosition;
        //initialCatcherScale = sourceRotor.transform.localScale;
        pourAngleCurrent = Mathf.Lerp(pourAngleEmpty, pourAngleFull, fullness);
        ingredients = new Dictionary<string, float>();
        for(int i = 0; i < initialIngredients.Length; i++)
        {
            ingredients[initialIngredients[i]] = ingredientRatios[i];
        }
    }
    
    public float compareContents(Dictionary<string, float> ISO)
    {
        float totalError = 0;
        foreach (string Key in ISO.Keys.ToList())
        {
            if (ingredients.ContainsKey(Key))
            {
                float maxError = Mathf.Max(ISO[Key], 1 - ISO[Key]);
                float error = Mathf.Abs(ISO[Key] - ingredients[Key])/maxError;
                totalError += error;
            }
            else
            {
                totalError += 1;
            }
        }
        totalError /= ISO.Count();
        return (1 - totalError);
    }

    public void addLiquid(string ingredientType, float vol)
    {
        if (!ingredients.ContainsKey(ingredientType))
        {
            ingredients[ingredientType] = 0;
        }
        foreach(string Key in ingredients.Keys.ToList())
        {
            ingredients[Key] *= fullness;
            if (Key == ingredientType)
            {
                ingredients[Key] += fullnessIncrement*vol;
            } 
            ingredients[Key] /= fullness + fullnessIncrement*vol;
            Debug.Log(Key + ": " + ingredients[Key]);
        }
        fullness += fullnessIncrement*vol;
    }

    void Update()
    {
        /*
        Vector3 eulersRotationNoY = transform.rotation.eulerAngles;
        eulersRotationNoY = new Vector3(eulersRotationNoY.x, 0, eulersRotationNoY.z);
        Quaternion rotationNoY = Quaternion.Euler(eulersRotationNoY);
        Vector3 dir = (rotationNoY * Vector3.ProjectOnPlane(transform.up, Vector3.up)).normalized;
        */
        //sourceRotor.transform.position = transform.position + ((initialRotorPosition - transform.position) * (fullness+bottomOffset));

        //Make catcher follow the liquid level
        //catcher.transform.localPosition = initialCatcherPosition * (fullness + bottomOffset);
        //catcher.transform.localScale = Vector3.Lerp(finalScale, initialCatcherScale, fullness);

        //draw liquid
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);
        liquidRenderer.material.SetFloat("_FillAmount", Mathf.Lerp(Mathf.Lerp(emptyLiquidLevel, emptyLiquidLevelFlipped, tiltAngle / 180), Mathf.Lerp(fullLiquidLevel, fullLiquidLevelFlipped, tiltAngle / 180), fullness));

        //eject liquid if needed
        if (fullness <= 0)
        {
            fullness = 0;
            return;
        }
        if (tiltAngle > pourAngleCurrent)
        {
            //Calculating ejection point
            Quaternion rotationNoY = Quaternion.Euler(transform.up);
            Vector3 dir = (rotationNoY * Vector3.ProjectOnPlane(transform.up, Vector3.up)).normalized;
            Vector3 ejectionPoint = (Vector3.ProjectOnPlane(dir, transform.up)).normalized * reservoirRadius;

            if (tiltAngle > 90) ejectionPoint *= -1;

            //building ejection line
            Vector3 sidewaysOffset = Vector3.Cross(ejectionPoint, transform.up);
            Vector3 ejectionA = sourceRotor.transform.position + sidewaysOffset / 2 + ejectionPoint * 0.8f;
            Vector3 ejectionB = sourceRotor.transform.position - sidewaysOffset / 2 + ejectionPoint * 0.8f;

            //ejecting liquid
            float pourRate = (tiltAngle - pourAngleCurrent)/10 * Time.deltaTime * 6;
            Debug.Log("pouring!");
            fullness -= pourRate/reservoirVolume;
            if(Random.value < (pourRate))
            {
                GameObject droplet = Instantiate(liquidParticlePrefab);
                droplet.transform.position = ejectionPoint;
                droplet.transform.position = Vector3.Lerp(ejectionA, ejectionB, Random.value);
                droplet.GetComponent<Rigidbody>().AddForce(transform.up * ejectionPower);
            }
            pourAngleCurrent = Mathf.Lerp(pourAngleEmpty, pourAngleFull, fullness);
        }
        //Vector3 dir = Vector3.ProjectOnPlane(transform.up, Vector3.up).normalized;
        //Debug.DrawLine(sourceRotor.transform.position, sourceRotor.transform.position + dir * reservoirRadius);
        //Debug.DrawLine(transform.position, sourceRotor.transform.position);
        //sourceRotor.transform.localRotation = Quaternion.LookRotation(dir);
    }
}
