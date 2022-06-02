using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Rating : MonoBehaviour
{
    public string category;
    [SerializeField] GameObject plate;
    [SerializeField] GameObject RUI;
    [SerializeField] GameObject star;
    [SerializeField] UnityEvent ratingFunction;
    public Text[] rating;
    public Text total_rating;
    public AudioClip tally;
    public AudioClip nice;
    public AudioClip ding;
    AudioSource AS;
    public float[] score;
    float total = 0;
    bool mutex = false;
    bool done = false;

    float RateFriedness(float friedness)
    {
        return (1 - Mathf.Abs(1 - friedness)) * 100;
    }

    public void FriedRating(float bottom, float overall, float top)
    {
        score[0] = (RateFriedness(bottom) + RateFriedness(overall) + RateFriedness(top)) / 3;
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
            RUI.SetActive(true);
            ratingFunction.Invoke();
            StartCoroutine(ScoreAnimation());
        }
    }

    public void RateBurger()
    {
        float time = Time.timeSinceLevelLoad;
        if (time < 120)
        {
            score[3] = 100;
        }
        else
        {
            score[3] = 120 / time * 100;
        }
        plate.GetComponent<CalculateIngredients>().Calc(out score[2], out score[1]);
        score[2] = 100 * Mathf.Sqrt(score[2]/100);
        if (score[1] < 45)
        {
            for (int i = 0; i < score.Length; i++)
                score[i] = 0;
        }
    }

    public void RateCake()
    {
        float time = Time.timeSinceLevelLoad;
        if (time < 240)
        {
            score[4] = 100;
        }
        else
        {
            score[4] = 240 / time * 100;
        }
        bakeableLiquidDough dough = FindObjectOfType<bakeableLiquidDough>();
        dippable glaze = FindObjectOfType<dippable>();
        score[0] = dough.quality * 100;
        score[1] = (1 - Mathf.Abs(1.05f - dough.bakingProgress)) * 100;
        score[2] = glaze.quality * 100;
        score[3] = glaze.countSprinkles()/(FindObjectsOfType<spinklesStick>().Length) * 100;
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
            for (int j = 0; j < score.Length; j++)
                rating[j].text = Mathf.RoundToInt(Mathf.Lerp(0, score[j], i)) + "%";
            yield return 0;
        }
        for (int i = 0; i < score.Length; i++)
            rating[i].text = Mathf.RoundToInt(score[i]) + "%";
        PlaySound(ding);
        while (AS.isPlaying)
        {
            yield return 0;
        }
        PlaySound(tally);
        for (int i = 0; i < score.Length; i++)
            total += score[i];
        total /= score.Length;
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
