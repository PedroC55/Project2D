using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{   

    [SerializeField] TextMeshProUGUI timerText;
    float elapsed_time;

    // Update is called once per frame
    void Update()
    {
        elapsed_time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsed_time / 60);
        int seconds = Mathf.FloorToInt(elapsed_time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
