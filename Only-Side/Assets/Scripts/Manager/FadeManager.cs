using UnityEngine;
using UnityEngine.UI; // Unity UIを使うために必要
using System.Collections;

public class FadeManager : MonoBehaviour
{
    #region Singleton

    private static FadeManager instance;

    public static FadeManager Instance
    {
        get
        {
            // インスタンスが存在しない場合、シーンから探す
            if (instance == null)
            {
                instance = (FadeManager)FindObjectOfType(typeof(FadeManager));

                if (instance == null)
                {
                    // インスタンスが見つからない場合、新しく作成
                    GameObject go = new GameObject("FadeManager");
                    instance = go.AddComponent<FadeManager>();
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    public bool DebugMode = false; // デバッグモードの設定
    public float fadeDuration = 0.5f; // フェードにかける時間
    public Color fadeColor = Color.black; // フェード色（黒）
    private Image fadeImage; // フェード用のImageコンポーネント
    private bool isFading = false; // フェード中かどうか
    private float fadeAlpha = 0; // 現在のフェードの透明度

    private void Awake()
    {
        if (this != Instance)
        {
            // Singletonの制約を守るために重複を防ぐ
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject); // シーン間で破棄されないように設定

        // CanvasとImageのセットアップ
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // 画面上に表示
        fadeImage = new GameObject("FadeImage").AddComponent<Image>();
        fadeImage.transform.SetParent(canvas.transform, false);
        fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height); // 画面全体をカバー
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAlpha); // フェード色を設定
        fadeImage.raycastTarget = false; // フェード画像がクリックを受け取らないようにする
    }

    // フェードアウトを開始
    public void FadeOut(System.Action onComplete = null)
    {
        if (isFading) return; // 既にフェード中なら処理を中断
        StartCoroutine(Fade(1f, onComplete)); // フェードアウトを実行
    }

    // フェードインを開始
    public void FadeIn(System.Action onComplete = null)
    {
        if (isFading) return; // 既にフェード中なら処理を中断
        StartCoroutine(Fade(0f, onComplete)); // フェードインを実行
    }

    // フェード処理のコルーチン
    private IEnumerator Fade(float targetAlpha, System.Action onComplete)
    {
        isFading = true; // フェード中フラグを設定
        float startAlpha = fadeImage.color.a; // フェード開始時の透明度
        float elapsed = 0f; // 経過時間

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha); // 色と透明度を更新
            yield return null; // 次のフレームまで待機
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, targetAlpha); // 最終的な透明度を設定
        isFading = false; // フェード終了フラグを設定
        onComplete?.Invoke(); // 完了時のコールバック
    }
}
