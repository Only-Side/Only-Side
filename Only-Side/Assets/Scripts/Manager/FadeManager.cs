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
            if (instance == null)
            {
                instance = (FadeManager)FindObjectOfType(typeof(FadeManager));

                if (instance == null)
                {
                    GameObject go = new GameObject("FadeManager");
                    instance = go.AddComponent<FadeManager>();
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    public bool DebugMode = true;
    public float fadeDuration = 0.5f; // フェードにかける時間
    public Color fadeColor = Color.black;
    private Image fadeImage;
    private bool isFading = false;
    private float fadeAlpha = 0;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        // CanvasとImageのセットアップ
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeImage = new GameObject("FadeImage").AddComponent<Image>();
        fadeImage.transform.SetParent(canvas.transform, false);
        fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        fadeImage.color = fadeColor;
        fadeImage.raycastTarget = false; // フェード画像がクリックを受け取らないようにする
    }

    public void FadeOut(System.Action onComplete = null)
    {
        if (isFading) return;
        StartCoroutine(Fade(1f, onComplete));
    }

    public void FadeIn(System.Action onComplete = null)
    {
        if (isFading) return;
        StartCoroutine(Fade(0f, onComplete));
    }

    private IEnumerator Fade(float targetAlpha, System.Action onComplete)
    {
        isFading = true;
        float startAlpha = fadeImage.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, targetAlpha);
        isFading = false;
        onComplete?.Invoke();
    }
}
