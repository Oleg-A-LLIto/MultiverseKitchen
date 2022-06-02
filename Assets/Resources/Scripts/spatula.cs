using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class spatula : MonoBehaviour
{
    enum mode { cut, spat };
    mode m = mode.spat;
    Hand holding;
    [SerializeField] GameObject razor1;
    [SerializeField] GameObject razor2;
    [SerializeField] GameObject picker;
    [SerializeField] Transform rotatable;
    Hand[] allHands = new Hand[2];
    
    // Start is called before the first frame update
    void Start()
    {
        allHands = FindObjectsOfType<Hand>();
    }

    Hand getOtherHand(Hand a) {
        if (allHands[0] == a)
        {
            return allHands[1];
        }
        else
        {
            return allHands[0];
        }
    }

    public void CheckMode()
    {
        holding = GetComponent<Interactable>().attachedToHand;
        if(holding != null)
        {
            Hand other = getOtherHand(holding);
            Debug.Log(other);
            if (other.currentAttachedObject == null)
            {
                if (m != mode.spat)
                {
                    StartCoroutine(GotoSpat());
                }
                return;
            }
            if (other.currentAttachedObject.TryGetComponent<cuttable>(out _))
            {
                if (m != mode.cut)
                {
                    StartCoroutine(GotoKnife());
                }
            }
            else
            {
                if (m != mode.spat)
                {
                    StartCoroutine(GotoSpat());
                }
            }
        }
    }

    IEnumerator GotoKnife()
    {
        float initangle = rotatable.rotation.eulerAngles.z;
        m = mode.cut;
        for (float i = 0; i < 1; i += Time.deltaTime*2)
        {
            rotatable.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(0, -90, i));
            yield return 0;
        }
        rotatable.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90);
        picker.SetActive(false);
        razor1.SetActive(true);
        razor2.SetActive(true);
        yield return 0;
    }

    IEnumerator GotoSpat()
    {
        float initangle = rotatable.rotation.eulerAngles.z;
        m = mode.spat;
        for (float i = 0; i < 1; i += Time.deltaTime * 2)
        {
            rotatable.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(-90, 0, i));
            yield return 0;
        }
        rotatable.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        picker.SetActive(true);
        razor1.SetActive(false);
        razor2.SetActive(false);
        yield return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
