using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{

    #region シングルトン化

    public static StoryManager instance;

    private void Awake()
    {
        // シングルトン化
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion シングルトン化

    // 原稿の内容を格納する構造体
    public struct SCRIPT
    {
        public string script_text;
        public string name_text;
        public string role_text;
        public string sound_effect;
        public string text_display_interval;
        public string choice_1;
        public string choice_2;
        public string trasition_line_1;
        public string trasition_line_2;
        public string normal_trasition_line;
    }
    public static List<SCRIPT> script = new List<SCRIPT>();     // 原稿の構造体を格納するリスト
    public static bool isAutoMode;     // 自動再生がオンか判定
    public static bool isOpenMainMenu;     // メインメニューを開いているか判定
    public static string[] scriptTexts;     // CSVで読み込まれた原稿のテキストが格納される配列
    public static string[] nameTexts;     // CSVで読み込まれた名前が格納される配列
    public static string[] roleTexts;     // CSVで読み込まれた役職が格納される配列
    public static string[] choiceOne;     // CSVで読み込まれた選択肢1
    public static string[] choiceTwo;     // CSVで読み込まれた選択肢2
    public static string[] soundEffectTexts;     // CSVで読み込まれたSE名が格納される配列
    public static string[] trasitionLineOne;     // 選択肢1で選ばれたときの遷移先
    public static string[] trasitionLineTwo;     // 選択肢2で選ばれたときの遷移先
    public static string[] normalTrasitionLine;     // 遷移先(選択のときに使う用)
    public string[] textDisplayInterval;     // CSVで読み込まれた表示間隔の数値が格納される配列

    public List<int> textNumberSaveList = new List<int>();     // 表示したテキストの番号を保存するリスト
    public static List<int> choiceSelectNumberSaveList = new List<int>();
    public PlayableDirector playableDirector;     // タイムラインの挿入
    public TextMeshProUGUI scriptTextObject;     // 原稿のテキストを表示するオブジェクト
    public TextMeshProUGUI nameTextObject;     // 名前のテキストを表示するオブジェクト
    public TextMeshProUGUI roleTextObject;     // 役職のテキストを表示するオブジェクト
    public List<GameObject> spawnedPrefabLogTextList = new List<GameObject>();
    public GameObject choiceButtonOneObject;     // 選択肢1ボタンオブジェクト
    public GameObject choiceButtonTwoObject;     // 選択肢2ボタンオブジェクト
    public GameObject logTextPrefabObject;     // ログのテキストを表示するプレハブオブジェクト
    public GameObject logMenuContentObject;     // ログメニュー内に表示するオブジェクト
    public TextMeshProUGUI choiceButtonOneTextObject;     // 選択肢1テキストオブジェクト
    public TextMeshProUGUI choiceButtonTwoTextObject;     // 選択肢2テキストオブジェクト
    public ClickCheck clickCheck;     // クリックの判定
    public Animator canTextDisplayAnim;     // 次のテキストが表示可能なときのアニメーション
    public ScriptDataBase scriptDataBase;
    public AudioSource _audioSource;
    public bool isTimelineFinished = false;     // タイムラインの終了判定

    private int _textNumber;
    private int textCharNumber;     // 現在表示しているn番目のテキスト
    private float displayTextIntervalCount = 0;     // 文字を表示するための間隔カウント
    private float displayFinishedCount = 0;     // 文字の表示が終わってからの秒数カウント
    private bool isClick = false;     // クリックされたか
    private bool isDisplayFinished = false;     // 文字の表示が終わったか
    private bool isInstantDisplayMode = false;     // 文字表示中にクリックで即時に表示終了させる
    private bool isDisplayChoice = false;     // 選択肢に文字が入力されているか
    private string displayScriptText;     // 実際に表示される原稿のテキスト
    private Dictionary<string, string> objectPaths = new Dictionary<string, string>();

    // CSVの表示しているn番目
    public int textNumber
    {
        get { return _textNumber; }
        set
        {
            // 値が変わった場合のみ処理を行う
            if (_textNumber != value)
            {
                _textNumber = value;
                StartCoroutine(VoiceSpeak(scriptTexts[_textNumber]));
                textNumberSaveList.Add(_textNumber);
            }
        }
    }

    // 選択肢の文字が入力されているか
    public bool IsInputChoiceText()
    {
        // 両方ともの選択肢に何か文字が入力されているか
        if (choiceOne[textNumber] != "" && choiceTwo[textNumber] != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        // シーンがロードされたときのイベントにハンドラを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
        // textNumberの初期値を追加
        textNumberSaveList.Add(textNumber);
        LoadScriptList();
        ChaceObjectPaths();
    }

    void Update()
    {
        // クリックの判定を持ってくる
        isClick = clickCheck.isClick;
        // テキスト表示間隔の計測開始
        displayTextIntervalCount += Time.deltaTime;
        // 全てのテキストの表示が終わっていない場合
        if (isClick && !isDisplayFinished && !isInstantDisplayMode)
        {
            isInstantDisplayMode = true;
        }
        if (isInstantDisplayMode)
        {
            DisplayText(0.01f);
        }
        else
        {
            // 間隔が指定されているか判定
            if (textDisplayInterval[textNumber] != "")
            {
                DisplayText(Convert.ToSingle(textDisplayInterval[textNumber]));
            }
            else
            {
                DisplayText(0.1f);
            }
        }
        // テキストの表示が終わったらカウントを開始する
        if (isDisplayFinished)
        {
            displayFinishedCount += Time.deltaTime;
        }
        // 選択肢の表示,非表示
        Choices();
        // ログテキストを同期
        LoadLogText();
    }

    private void ChaceObjectPaths()
    {
        objectPaths[nameof(scriptTextObject)] = Hierarchy.GetHierarchyPath(scriptTextObject.transform);
        objectPaths[nameof(nameTextObject)] = Hierarchy.GetHierarchyPath(nameTextObject.transform);
        objectPaths[nameof(roleTextObject)] = Hierarchy.GetHierarchyPath(roleTextObject.transform);
        objectPaths[nameof(choiceButtonOneObject)] = Hierarchy.GetHierarchyPath(choiceButtonOneObject.transform);
        objectPaths[nameof(choiceButtonTwoObject)] = Hierarchy.GetHierarchyPath(choiceButtonTwoObject.transform);
        objectPaths[nameof(choiceButtonOneTextObject)] = Hierarchy.GetHierarchyPath(choiceButtonOneTextObject.transform);
        objectPaths[nameof(choiceButtonTwoTextObject)] = Hierarchy.GetHierarchyPath(choiceButtonTwoTextObject.transform);
        objectPaths[nameof(logMenuContentObject)] = Hierarchy.GetHierarchyPath(logMenuContentObject.transform);
        objectPaths[nameof(clickCheck)] = Hierarchy.GetHierarchyPath(clickCheck.transform);
        objectPaths[nameof(canTextDisplayAnim)] = Hierarchy.GetHierarchyPath(canTextDisplayAnim.transform);
    }

    private void AssignObjectReferences()
    {
        scriptTextObject = FindComponent<TextMeshProUGUI>(nameof(scriptTextObject));
        nameTextObject = FindComponent<TextMeshProUGUI>(nameof(nameTextObject));
        roleTextObject = FindComponent<TextMeshProUGUI>(nameof(roleTextObject));
        choiceButtonOneObject = FindGameObject(nameof(choiceButtonOneObject));
        choiceButtonTwoObject = FindGameObject(nameof(choiceButtonTwoObject));
        logMenuContentObject = FindGameObject(nameof(logMenuContentObject));
        choiceButtonOneTextObject = FindComponent<TextMeshProUGUI>(nameof(choiceButtonOneTextObject));
        choiceButtonTwoTextObject = FindComponent<TextMeshProUGUI>(nameof(choiceButtonTwoTextObject));
        clickCheck = FindComponent<ClickCheck>(nameof(clickCheck));
        canTextDisplayAnim = FindComponent<Animator>(nameof(canTextDisplayAnim));
    }

    // テキストを表示させる
    private void DisplayText(float _displayTextInterval)
    {
        if (displayTextIntervalCount >= _displayTextInterval)
        {
            if (textCharNumber != scriptTexts[textNumber].Length)
            {
                // セリフテキストに表示するテキストを一文字ずつ増やす
                displayScriptText += scriptTexts[textNumber][textCharNumber];
                textCharNumber += 1;
            }
            else
            {
                // 全てのテキストを表示し終わったら
                if (textNumber != scriptTexts.Length - 1 && !isDisplayChoice)
                {
                    isDisplayFinished = true;
                    // タイムラインの再生が終わり、クリックかオート再生で6秒たったら
                    if (isTimelineFinished && (isClick || (isAutoMode && displayFinishedCount > 6)))
                    {
                        isDisplayFinished = false;
                        isInstantDisplayMode = false;
                        isTimelineFinished = false;
                        // 入力されていた文字を消す
                        displayScriptText = "";
                        // 現在表示の番数を初期化
                        textCharNumber = 0;
                        // タイムラインを再開する
                        playableDirector.Resume();
                        // 遷移先が指定されている場合
                        if (normalTrasitionLine[textNumber] != "")
                        {
                            // 遷移先に移動
                            textNumber = int.Parse(normalTrasitionLine[textNumber]) - 2;
                        }
                        // 通常時の遷移
                        else
                        {
                            textNumber += 1;
                        }
                        // 終了のカウントを初期化
                        displayFinishedCount = 0;
                        //StartCoroutine(VoiceSpeak(scriptTexts[textNumber + 1]));
                    }
                }
                else
                {
                    isDisplayFinished = false;
                }
            }
            // テキスト表示間隔の初期化
            displayTextIntervalCount = 0;
            // 一文字ずつテキストを表示させる
            scriptTextObject.text = displayScriptText;
            // 名前のテキストを変更
            nameTextObject.text = nameTexts[textNumber];
            // 役職のテキストを変更
            roleTextObject.text = roleTexts[textNumber];
            // アニメーションにテキストの表示が終わったかの条件を付ける
            canTextDisplayAnim.SetBool("isDisplayFinished", isDisplayFinished);
            clickCheck.isClick = false;
        }
    }

    // 選択肢の表示、非表示
    private void Choices()
    {
        // ボタンのテキストがあるか判定
        if (IsInputChoiceText())
        {
            // 選択肢がある
            isDisplayChoice = true;
            // 選択肢が見えるようになる
            choiceButtonOneObject.SetActive(true);
            choiceButtonTwoObject.SetActive(true);
            // ボタンの文字をCSVで入力された奴にする
            choiceButtonOneTextObject.text = choiceOne[textNumber];
            choiceButtonTwoTextObject.text = choiceTwo[textNumber];
            // クリックの判別をオフに
            DisableClickCheck();
            isDisplayFinished = false;
        }
        else
        {
            // 選択肢がない
            isDisplayChoice = false;
            // 選択肢を見えなくする
            choiceButtonOneObject.SetActive(false);
            choiceButtonTwoObject.SetActive(false);
            // クリックの判別をオンに
            EnableClickCheck();
        }
    }

    // ScriptDataBaseからデータを読み込む
    private void LoadScriptList()
    {
        if (scriptDataBase != null)
        {
            // 配列の初期化
            int length = scriptDataBase.scriptDatas.Length;
            scriptTexts = new string[length];
            nameTexts = new string[length];
            roleTexts = new string[length];
            soundEffectTexts = new string[length];
            textDisplayInterval = new string[length];
            choiceOne = new string[length];
            choiceTwo = new string[length];
            trasitionLineOne = new string[length];
            trasitionLineTwo = new string[length];
            normalTrasitionLine = new string[length];
            for (int i = 0; i < length; i++)
            {
                // ScriptDataBaseから持ってきたリストを読み込む
                scriptTexts[i] = scriptDataBase.scriptDatas[i].script_text;
                nameTexts[i] = scriptDataBase.scriptDatas[i].name_text;
                roleTexts[i] = scriptDataBase.scriptDatas[i].role_text;
                soundEffectTexts[i] = scriptDataBase.scriptDatas[i].sound_effect;
                textDisplayInterval[i] = scriptDataBase.scriptDatas[i].text_display_interval;
                choiceOne[i] = scriptDataBase.scriptDatas[i].choice_1;
                choiceTwo[i] = scriptDataBase.scriptDatas[i].choice_2;
                trasitionLineOne[i] = scriptDataBase.scriptDatas[i].trasition_line_1;
                trasitionLineTwo[i] = scriptDataBase.scriptDatas[i].trasition_line_2;
                normalTrasitionLine[i] = scriptDataBase.scriptDatas[i].normal_trasition_line;
            }
        }
        else
        {
            print("ScriptDataBase is not found!");
        }
    }

    // ログテキストを読み込む
    private void LoadLogText()
    {
        // プレハブの数をtextNumberSaveListの要素数に合わせる
        while (spawnedPrefabLogTextList.Count < textNumberSaveList.Count - 1)
        {
            // プレハブを生成
            GameObject _logTextPrefabObject = Instantiate(logTextPrefabObject,
                transform.position, Quaternion.identity, logMenuContentObject.transform);
            spawnedPrefabLogTextList.Add(_logTextPrefabObject);
        }

        while (spawnedPrefabLogTextList.Count > textNumberSaveList.Count)
        {
            Destroy(spawnedPrefabLogTextList[spawnedPrefabLogTextList.Count - 1]);
            /* spawnedPrefabLogTextList.Count - 1 番目を削除*/
            spawnedPrefabLogTextList.RemoveAt(spawnedPrefabLogTextList.Count - 1);
        }

        // 生成されたプレハブのスクリプトの値を更新
        for (int i = 0; i < textNumberSaveList.Count - 1; i++)
        {
            // 生成したプレハブからLogTextsのコンポーネントを取得
            LogTexts logTexts = spawnedPrefabLogTextList[i].GetComponent<LogTexts>();
            // LogTextsのコンポーネントを取得できているか
            if (logTexts != null)
            {
                // リストに入っている数値と同じやつを代入
                logTexts.textNumber = textNumberSaveList[i];
            }
        }
    }

    // シーンがロードされたときに呼び出される
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Novel")
        {
            AssignObjectReferences();
        }
    }

    // クリックの判別をオンに
    public void EnableClickCheck()
    {
        clickCheck.enabled = true;
    }

    // クリックの判別をオフに
    public void DisableClickCheck()
    {
        clickCheck.enabled = false;
    }

    private IEnumerator VoiceSpeak(string text)
    {
        // VOICEVOXのREST-APIクライアント
        VoiceVoxApiClient client = new VoiceVoxApiClient();

        // テキストからAudioClipを生成（話者は「3:ずんだもん」）
        yield return client.TextToAudioClip(3, text);

        if (client.AudioClip != null)
        {
            // AudioClipを取得し、AudioSourceにアタッチ
            _audioSource.clip = client.AudioClip;
            // AudioSourceで再生
            _audioSource.Play();
        }
    }

    private T FindComponent<T>(string objectName) where T : Component
    {
        return transform.Find(objectPaths[objectName]).GetComponent<T>();
    }

    public GameObject FindGameObject(string objectName)
    {
        return transform.Find(objectPaths[objectName]).gameObject;
    }
}
