using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Unity.Mathematics;

public class StoryManager : MonoBehaviour
{
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
    public static List<SCRIPT> script = new List<SCRIPT>();
    public static bool isAutoMode;     // 自動再生がオンか判定
    public static bool isOpenMainMenu;     // メインメニューを開いているか判定
    public static string[] scriptTexts;     // CSVで読み込まれた原稿のテキストが格納される配列
    public static string[] nameTexts;     // CSVで読み込まれた名前が格納される配列

    public List<int> textNumberSaveList = new List<int>();     // 表示したテキストの番号を保存するリスト
    public PlayableDirector playableDirector;     // タイムラインの挿入
    public TextMeshProUGUI scriptTextObject;     // 現行のテキストを表示するオブジェクト
    public TextMeshProUGUI nameTextObejct;     // 名前のテキストを表示するオブジェクト
    public TextMeshProUGUI roleTextObejct;     // 役職のテキストを表示するオブジェクト
    public GameObject choiceButtonOneObject;     // 選択肢1ボタンオブジェクト
    public GameObject choiceButtonTwoObject;     // 選択肢2ボタンオブジェクト
    public GameObject logTextPrefabObject;
    public GameObject logMenuContentObject;
    public TextMeshProUGUI choiceButtonOneTextObject;     // 選択肢1テキストオブジェクト
    public TextMeshProUGUI choiceButtonTwoTextObject;     // 選択肢2テキストオブジェクト
    public ClickCheck clickCheck;     // クリックの判定
    public Animator canTextDisplayAnim;     // 次のテキストが表示可能なときのアニメーション
    public string[] soundEffectTexts;     // CSVで読み込まれたSE名が格納される配列
    public string[] trasitionLineOne;     // 選択肢1で選ばれたときの遷移先
    public string[] trasitionLineTwo;     // 選択肢2で選ばれたときの遷移先
    public string[] normalTrasitionLine;     // 遷移先(選択のときに使う用)
    public bool isTimelineFinished = false;     // タイムラインの終了判定

    public List<GameObject> spawnedPrefabLogTextList = new List<GameObject>();
    private string[] roleTexts;     // CSVで読み込まれた役職が格納される配列
    private string[] textDisplayInterval;     // CSVで読み込まれた表示間隔の数値が格納される配列
    private string[] choiceOne;     // CSVで読み込まれた選択肢1
    private string[] choiceTwo;     // CSVで読み込まれた選択肢2
    private string displayScriptText;     // 実際に表示される原稿のテキスト
    private int textCharNumber;     // 現在表示しているn番目のテキスト
    private int _textNumber;
    private float displayTextIntervalCount = 0;     // 文字を表示するための間隔カウント
    private float displayFinishedCount = 0;     // 文字の表示が終わってからの秒数カウント
    private bool isClick = false;     // クリックされたか
    private bool isDisplayFinished = false;     // 文字の表示が終わったか
    private bool isInstantDisplayMode = false;     // 文字表示中にクリックで即時に表示終了させる
    private bool isDisplayChoice = false;     // 選択肢に文字が入力されているか

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
                // 表示したテキスト番号の履歴に追加
                textNumberSaveList.Add(_textNumber);
            }
        }
    }

    // 選択肢の文字が入力されているか
    public bool IsInputChoiceText()
    {
        // 両方ともの選択肢に何か文字が入力されているか
        if(choiceOne[textNumber] != "" && choiceTwo[textNumber] != "")
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
        // SCRIPT構造体にCSVを読み込ませる
        script = ScriptReadCsv();
        // リストにCSVを読み込む関数
        LoadCsvList();
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

    // テキストを表示させる
    private void DisplayText(float _displayTextInterval)
    {
        if(displayTextIntervalCount >= _displayTextInterval)
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
                if (textNumber != scriptTexts.Length - 1  && !isDisplayChoice)
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
            nameTextObejct.text = nameTexts[textNumber];
            // 役職のテキストを変更
            roleTextObejct.text = roleTexts[textNumber];
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
            EnableClickCheck();
        }
    }

    // リストにCSVを読み込む
    private void LoadCsvList()
    {
        // 配列の初期化
        scriptTexts = new string[script.Count];
        nameTexts = new string[script.Count];
        roleTexts = new string[script.Count];
        soundEffectTexts = new string[script.Count];
        textDisplayInterval = new string[script.Count];
        choiceOne = new string[script.Count];
        choiceTwo = new string[script.Count];
        trasitionLineOne = new string[script.Count];
        trasitionLineTwo = new string[script.Count];
        normalTrasitionLine = new string[script.Count];
        for (int i = 0; i < script.Count; i++)
        {
            // CSVから持ってきたリストを読み込む
            scriptTexts[i] = script[i].script_text;
            nameTexts[i] = script[i].name_text;
            roleTexts[i] = script[i].role_text;
            soundEffectTexts[i] = script[i].sound_effect;
            textDisplayInterval[i] = script[i].text_display_interval;
            choiceOne[i] = script[i].choice_1;
            choiceTwo[i] = script[i].choice_2;
            trasitionLineOne[i] = script[i].trasition_line_1;
            trasitionLineTwo[i] = script[i].trasition_line_2;
            normalTrasitionLine[i] = script[i].normal_trasition_line;
        }
    }

    public List<SCRIPT> ScriptReadCsv()
    {
        SCRIPT script = new SCRIPT();
        List<SCRIPT> script_list = new List<SCRIPT>();

        // ResourcesからCSVを読み込む
        TextAsset scriptCsv;

        // CSVを格納
        List<string[]> scriptCsvDates = new List<string[]>();
        
        // CSVの行数
        int height = 0;

        // Resources/CSVにあるファイルを読み込む
        scriptCsv = Resources.Load("CSV/test") as TextAsset;
        // 読み込んだファイルをString型で格納
        StringReader reader = new StringReader(scriptCsv.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            // ,で区切られて格納
            scriptCsvDates.Add(line.Split(','));
            height++;
        }
        for (int i = 1; i < height; i++)
        {
            script.script_text = scriptCsvDates[i][0];
            script.name_text = scriptCsvDates[i][1];
            script.role_text = scriptCsvDates[i][2];
            script.sound_effect = scriptCsvDates[i][3];
            script.text_display_interval = scriptCsvDates[i][4];
            script.choice_1 = scriptCsvDates[i][5];
            script.choice_2 = scriptCsvDates[i][6];
            script.trasition_line_1 = scriptCsvDates[i][7];
            script.trasition_line_2 = scriptCsvDates[i][8];
            script.normal_trasition_line = scriptCsvDates[i][9];
    
            //戻り値のリストに加える
            script_list.Add(script);
        }
        return script_list;
    }

    // ログテキストを読み込む
    private void LoadLogText()
    {
        // プレハブの数をtextNumberSaveListの要素数に合わせる
        while (spawnedPrefabLogTextList.Count < textNumberSaveList.Count)
        {
            // プレハブを生成
            GameObject _logTextPrefabObject = Instantiate(logTextPrefabObject, 
                transform.position, Quaternion.identity, logMenuContentObject.transform);
            spawnedPrefabLogTextList.Add(_logTextPrefabObject);
        }

        while (spawnedPrefabLogTextList.Count > textNumberSaveList.Count)
        {
            Destroy(spawnedPrefabLogTextList[spawnedPrefabLogTextList.Count - 1]);
            spawnedPrefabLogTextList.RemoveAt(spawnedPrefabLogTextList.Count - 1);
        }

        // 生成されたプレハブのスクリプトの値を更新
        for (int i = 0; i < textNumberSaveList.Count; i++)
        {
            // 生成したプレハブからLogTextsのコンポーネントを取得
            LogTexts logTexts = spawnedPrefabLogTextList[i].GetComponent<LogTexts>();
            // LogTextsのコンポーネントを取得できているか
            if (logTexts != null)
            {
                // リストに入っている数値と同じやつを代入
                logTexts.textNumber = textNumberSaveList[i] - 1;
            }
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
}
