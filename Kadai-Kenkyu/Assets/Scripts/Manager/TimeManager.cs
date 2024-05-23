using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    public float tick;     // 時計が進む割合
    public float second;     // 秒
    public int minute;     // 分
    public int hour;     // 時
    public int day = 1;     // 日
    public GameObject volumeObject;     // 明るさを調節するオブジェクト

    private Volume volume = null;

    void Start()
    {
        volume = volumeObject.GetComponent<Volume>();
    }


    void FixedUpdate()
    {
        CalculateTime();
        ControlVolume();
    }

    // 時間の計算
    private void CalculateTime()
    {
        // 経過時間を計測する
        second += Time.fixedDeltaTime * tick;
        // 60秒は1分
        if (second >= 60)
        {
            second = 0;
            minute++;
        }
        // 60分は1時間
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }
        // 24時間は1日
        if (hour >= 24)
        {
            hour = 0;
            day++;
        }
    }

    // 明るさの調整
    private void ControlVolume()
    {
        // 21:00～22:00の間
        if (hour >= 21 && hour < 22)
        {
            // 明るさをゆっくりと増加させる
            volume.weight = (float)minute / 60; 
        }
        // 6:00～7:00の間
        if (hour >= 6 && hour < 7)
        {
            // 明るさをゆっくりと減少させる
            volume.weight = 1 - (float)minute / 60;
        }
    }
}
