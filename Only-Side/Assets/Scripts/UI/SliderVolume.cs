using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    private Slider slider;
    public float stepSize = 1.0f; // ステップサイズを指定

    private void Start()
    {
        slider = GetComponent<Slider>();
        // スライダーの初期値をステップに合わせる
        slider.value = Mathf.Round(slider.value / stepSize) * stepSize;

        // スライダーの値が変更されたときのイベントリスナーを追加
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // ステップサイズに従って値を調整
        slider.value = Mathf.Round(value / stepSize) * stepSize;
    }
}
