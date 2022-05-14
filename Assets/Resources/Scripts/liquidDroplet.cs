using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liquidDroplet : MonoBehaviour
{
    public string ingredient;
    public float volume = 1;
    [SerializeField] bool leftSource = false;
    bool dying = false;

    private void OnTriggerStay(Collider other)
    {
        if (dying)
        {
            return;
        }
        if (leftSource)
        {
            liquidDropCatcher dropCatcher;
            if (other.TryGetComponent<liquidDropCatcher>(out dropCatcher))
            {
                dropCatcher.captureDrop(this);
                dying = true;
                StartCoroutine(die());
            }
        }
    }

    void Start()
    {
        if (!leftSource)
        {
            StartCoroutine(leaveSource());
        }
    }

    IEnumerator leaveSource()
    {
        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            yield return 0;
        }
        leftSource = true;
    }

    IEnumerator die()
    {
        Vector3 initialScale = transform.localScale;
        for (float i = 1; i > 0; i -= 3*Time.deltaTime)
        {
            transform.localScale = initialScale * i;
            yield return 0;
        }
        Destroy(gameObject);
    }

    public void setup(string ing, bool left, float volume)
    {
        ingredient = ing;
        leftSource = left;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
