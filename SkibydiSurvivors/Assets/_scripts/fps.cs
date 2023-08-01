using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class fps : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshPro;
    public float timer, refresh, avgFramerate;
    string display = "FPS {0}";

    private void Start()
    {
       m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        m_TextMeshPro.text = string.Format(display, avgFramerate.ToString());
    }
}
