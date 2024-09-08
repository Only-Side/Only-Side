using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void Fixed60FPS()
    {
        Application.targetFrameRate = 60;
    }

    public void Fixed30FPS()
    {
        Application.targetFrameRate = 30;
    }
}
