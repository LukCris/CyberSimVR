using UnityEngine;
using TMPro;
using System;

public class ClockUI : MonoBehaviour
{
    public TMP_Text clockText;

    void Start()
    {
        InvokeRepeating("UpdateClock", 0f, 1f); // Chiama ogni secondo
    }

    void UpdateClock()
    {
        DateTime now = DateTime.Now;
        clockText.text = now.ToString("HH:mm:ss");
    }
}
