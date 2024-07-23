using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class TimingGame : MonoBehaviour
{
    public Image successZone;
    public Image baseZone;

    private float successZoneWidth;// 90
    private float baseZoneWidth;// 300
    private float successPosition;
    private float sliderValue;
    private float zoneStart;
    private float zoneEnd;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
        if (sliderValue < slider.maxValue)
        {
            slider.value += 0.05f;
        }
    }

    void CheckSuccess()
    {
        if (slider.value >= zoneStart && slider.value <= zoneEnd)
        {
            Debug.Log("Success!" + zoneStart + "," + zoneEnd);
            // ¬Œ÷Žž‚Ìˆ—‚ð‚±‚±‚É’Ç‰Á
        }
        else
        {
            Debug.Log("Failed." + zoneStart+ "," + zoneEnd);
            // Ž¸”sŽž‚Ìˆ—‚ð‚±‚±‚É’Ç‰Á
        }

        SetRandomSuccessPosition();
    }

    private void SetRandomSuccessPosition()
    {
        // Base‚©‚ç‚Í‚Ýo‚È‚¢‚æ‚¤‚É‚·‚é
        successPosition = Mathf.Clamp(Random.Range(0, 100f), 15f, 85f);
        zoneStart = successPosition - (successZoneWidth / 6);
        zoneEnd = successPosition + (successZoneWidth / 6);

        successZone.rectTransform.transform.localPosition = new Vector3(
            (successPosition - 50) * 3, 
            successZone.rectTransform.position.y, 
            successZone.rectTransform.position.z);
    }
}