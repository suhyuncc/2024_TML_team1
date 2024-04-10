using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;
    float currentTime;          // 경과 시간
    [SerializeField]
    float remainTime;           // 카운트 다운

    private void Update()
    {
        // 경과 시간 확인
        /*
        currentTime += Time.deltaTime;
        int min = Mathf.FloorToInt(currentTime / 60);
        int sec = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
        */

        // 시간 카운트 다운
        if(remainTime > 0)
        {
            remainTime -= Time.deltaTime;
        }
        else if(remainTime < 0)
        {
            remainTime = 0;
            timerText.color = Color.red;
        }
        int min = Mathf.FloorToInt(remainTime / 60);
        int sec = Mathf.FloorToInt(remainTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", min, sec);

    }
}
