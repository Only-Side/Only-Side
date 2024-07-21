using UnityEngine;
using UnityEngine.UI;

public class TimingGame : MonoBehaviour
{
    public Image successZone;
    public Image baseZone;

    private float successZoneWidth;// 90
    private float successPosition;
    private float baseZoneWidth;// 300
    private float sliderValue;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 100f;
        successZoneWidth = successZone.rectTransform.sizeDelta.x;
        baseZoneWidth = successZone.rectTransform.sizeDelta.x;
        SetRandomSuccessPosition();
    }

    void Update()
    {
        sliderValue = slider.value;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
        if(sliderValue < slider.maxValue)
        {
            sliderValue += 0.05f;
        }
    }

    void SetRandomSuccessPosition()
    {
        successPosition = Random.Range(0f, slider.maxValue);
        float minPosition = -(baseZoneWidth - successZoneWidth) / 2;
        float maxPosition = (baseZoneWidth - successZoneWidth) / 2;
        float zoneStart = Mathf.Clamp(successPosition - successZoneWidth / 2, minPosition, maxPosition);
        float zoneEnd = Mathf.Clamp(successPosition + successZoneWidth / 2, minPosition, maxPosition);

        // 成功ゾーンの位置をスライダーに合わせて設定
        successZone.rectTransform.anchorMin = new Vector2(zoneStart / slider.maxValue, successZone.rectTransform.anchorMin.y);
        successZone.rectTransform.anchorMax = new Vector2(zoneEnd / slider.maxValue, successZone.rectTransform.anchorMax.y);
    }

    void CheckSuccess()
    {
        float zoneStart = successPosition - successZoneWidth / 2;
        float zoneEnd = successPosition + successZoneWidth / 2;

        if (sliderValue >= zoneStart && sliderValue <= zoneEnd)
        {
            Debug.Log("Success!");
            // 成功時の処理をここに追加
        }
        else
        {
            Debug.Log("Failed.");
            // 失敗時の処理をここに追加
        }

        SetRandomSuccessPosition();
    }
}
