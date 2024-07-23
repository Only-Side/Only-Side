using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class Cooking : MonoBehaviour
{
    public Image successZoneImage;     // 成功の背景画像
    public Image baseZoneImaqge;     // ベースの背景画像

    private float successZoneWidth;     // 成功背景画像の幅
    private float baseZoneWidth;     // ベース背景画像の幅
    private float successPosition;     // 成功の基準位置
    private float zoneStart;     // 成功の基準の開始位置
    private float zoneEnd;     // 成功の基準の終了位置
    private Slider slider;

    void Start()
    {
        // スライダーのコンポーネント取得
        slider = GetComponent<Slider>();
        // スライダーの最小値と最大値の設定
        slider.minValue = 0f;
        slider.maxValue = 100f;
        // 幅の取得
        successZoneWidth = successZoneImage.rectTransform.sizeDelta.x;
        baseZoneWidth = successZoneImage.rectTransform.sizeDelta.x;
        SetRandomSuccessPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
        if (slider.value < slider.maxValue)
        {
            slider.value += 0.05f;
        }
    }

    void CheckSuccess()
    {
        // 成功位置に入っていた場合の処理
        if (slider.value >= zoneStart && slider.value <= zoneEnd)
        {
            Debug.Log("Success!");
        }
        else
        {
            Debug.Log("Failed.");
        }

        SetRandomSuccessPosition();
    }

    private void SetRandomSuccessPosition()
    {
        // Baseからはみ出ないようにするランダムな位置に来るようにする
        successPosition = Mathf.Clamp(Random.Range(0, 100f), 15f, 85f);
        // 成功の開始位置と終了位置を設定する
        zoneStart = successPosition - (successZoneWidth / 6);
        zoneEnd = successPosition + (successZoneWidth / 6);

        // 成功背景画像の位置を設定
        // 真ん中のx座標は0なのでそれに合わせるようにする
        successZoneImage.rectTransform.transform.localPosition = new Vector3(
            (successPosition - 50) * 3, 
            successZoneImage.rectTransform.position.y, 
            successZoneImage.rectTransform.position.z);
    }
}