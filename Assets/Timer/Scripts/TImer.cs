using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TImer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("TimeSettings")]
    public float currentTime;
    public bool countDown;

    [Header("Limit Settings")]
    public float timerLimit;
    public bool hasLimit;


    [Header("LastMinutePulse")]
    public float timeLastMinutePulsingStart;
    public float timeLastMinutePulsingEnd;
    public Color LastMinutePulsingEnd;

    //public GameObject End;

    private void Start()
    {
        //End.SetActive(false);
    }

    void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

        if (currentTime <= timeLastMinutePulsingStart && currentTime >= timeLastMinutePulsingEnd)
        {
            StartSwinging(transform, 5.0f, 10.0f);
            
            timerText.color = Color.Lerp(timerText.color, LastMinutePulsingEnd, 1f);
        }

        if (hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit)))
        {
            currentTime = timerLimit;
            SetTimerText();
            timerText.color = Color.black;
            enabled = false;

            //End.SetActive(true);
        }
        SetTimerText();

    }
    private void SetTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;
    }

    public void StartSwinging(Transform objectToSwing, float angle, float speed)
    {
        StartCoroutine(SwingObject(objectToSwing, angle, speed));
    }

    private IEnumerator SwingObject(Transform objectToSwing, float maxAngle, float speed)
    {
        Quaternion startRotation = objectToSwing.rotation;
        float timer = 0.0f;

        while (true) // Toto bude pokračovat, dokud coroutine nebude explicitně zastavena
        {
            // Vypočítá se nový úhel pomocí sinusové funkce pro plynulý kolébavý pohyb
            float angle = maxAngle * Mathf.Sin(timer * speed);
            // Aplikujeme novou rotaci
            objectToSwing.rotation = startRotation * Quaternion.Euler(0, 0, angle);
            // Časovač se aktualizuje podle času uplynulého od posledního snímku
            timer += Time.deltaTime;
            // Coroutine čeká na další snímek
            yield return null;
        }
    }

}
