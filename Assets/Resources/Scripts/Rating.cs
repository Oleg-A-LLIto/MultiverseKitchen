using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Rating : MonoBehaviour
{
    public string category;
    public GameObject plate;
    public GameObject RUI;
    public GameObject star;
    public Text cooking_rating;
    public Text proportions_rating;
    public Text order_rating;
    public Text speed_rating;
    public Text total_rating;
    public AudioClip tally;
    public AudioClip nice;
    public AudioClip ding;
    AudioSource AS;
    float cooking = 0;
    float proportions = 0;
    float order = 0;
    float speed = 0;
    float total = 0;
    bool mutex = false;
    bool done = false;

    float RateFriedness(float friedness)
    {
        return (1 - Mathf.Abs(1 - friedness)) * 100;
    }

    public void FriedRating(float bottom, float overall, float top)
    {
        cooking = (RateFriedness(bottom) + RateFriedness(overall) + RateFriedness(top)) / 3;
    }

    public void PlaySound(AudioClip sound)
    {
        AS.clip = sound;
        AS.Play();
    }

    public void GiveRating()
    {
        if (!mutex)
        {
            if (done)
            {
                SceneManager.LoadScene(0);
                return;
            }
            mutex = true;
            float time = Time.timeSinceLevelLoad;
            if (time < 120)
            {
                speed = 100;
            }
            else
            {
                speed = 120 / time * 100;
            }
            plate.GetComponent<CalculateIngredients>().Calc(out order, out proportions);
            RUI.SetActive(true);
            order = Mathf.Sqrt(order);
            if (proportions < 45)
            {
                order = 0;
                proportions = 0;
                speed = 0;
                cooking = 0;
            }
            StartCoroutine(ScoreAnimation());
        }
    }

    IEnumerator ScoreAnimation()
    {
        PlaySound(ding);
        while (AS.isPlaying)
        {
            yield return 0;
        }
        PlaySound(tally); 
        for(float i = 0; i < 1; i += 2 * Time.deltaTime)
        {
            cooking_rating.text = Mathf.RoundToInt(Mathf.Lerp(0, cooking, i)) + "%";
            proportions_rating.text = Mathf.RoundToInt(Mathf.Lerp(0, proportions, i)) + "%";
            order_rating.text = Mathf.RoundToInt(Mathf.Lerp(0, order, i)) + "%";
            speed_rating.text = Mathf.RoundToInt(Mathf.Lerp(0, speed, i)) + "%";
            yield return 0;
        }
        cooking_rating.text = Mathf.RoundToInt(cooking) + "%";
        proportions_rating.text = Mathf.RoundToInt(proportions) + "%";
        order_rating.text = Mathf.RoundToInt(order) + "%";
        speed_rating.text = Mathf.RoundToInt(speed) + "%";
        PlaySound(ding);
        while (AS.isPlaying)
        {
            yield return 0;
        }
        PlaySound(tally);
        total = (cooking + proportions + order + speed) / 4;
        for (float i = 0; i < 1; i += 2 * Time.deltaTime)
        {
            total_rating.text = Mathf.RoundToInt(Mathf.Lerp(0, total, i)) + "%";
            yield return 0;
        }
        total_rating.text = Mathf.RoundToInt(total) + "%";
        PlaySound(ding);
        if(total >= 69)
        {
            while (AS.isPlaying)
            {
                yield return 0;
            }
            star.SetActive(true);
            PlaySound(nice);
        }
        if(total > 0)
        {
            if(PlayerPrefs.GetInt(category,0) < total)
            {
                PlayerPrefs.SetInt(category, Mathf.RoundToInt(total));
            }
        }
        mutex = false;
        done = true;
    }

    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
