using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

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
    public PlayableDirector playableDirector;
    public TextMeshProUGUI scriptTextObject;     // 現行のテキストを表示するオブジェクト
    public TextMeshProUGUI nameTextObejct;     // 名前のテキストを表示するオブジェクト
    public TextMeshProUGUI roleTextObejct;     // 役職のテキストを表示するオブジェクト
    public GameObject choiceButtonOneObject;     // 選択肢1ボタンオブジェクト
    public GameObject choiceButtonTwoObject;     // 選択肢2ボタンオブジェクト
    public TextMeshProUGUI choiceButtonOneTextObject;     // 選択肢1テキストオブジェクト
    public TextMeshProUGUI choiceButtonTwoTextObject;     // 選択肢2テキストオブジェクト
    public ClickCheck clickCheck;     // クリックの判定
    public string[] soundEffectTexts;     // CSVで読み込まれたSE名が格納される配列
    public int textNumber;     // n番目
    public static bool isAutoMode;     // 自動再生がオンか

    private string[] scriptTexts;     // CSVで読み込まれた原稿のテキストが格納される配列
    private string[] nameTexts;     // CSVで読み込まれた名前が格納される配列
    private string[] roleTexts;     // CSVで読み込まれた役職が格納される配列
    private string[] textDisplayInterval;     // CSVで読み込まれた表示間隔の数値が格納される配列
    private string[] choiceOne;     // CSVで読み込まれた選択肢1
    private string[] choiceTwo;     // CSVで読み込まれた選択肢2
    public string[] trasitionLineOne;     // 選択肢1で選ばれたときの遷移先
    public string[] trasitionLineTwo;     // 選択肢2で選ばれたときの遷移先
    public string[] normalTrasitionLine;     // 遷移先(選択のときに使う用)
    private string displayScriptText;     // 実際に表示される原稿のテキスト
    private int textCharNumber;     // 現在表示しているn番目のテキスト
    private float displayTextIntervalCount = 0;     // 文字を表示するための間隔カウント
    private float displayFinishedCount = 0;     // 文字の表示が終わってからの秒数カウント
    private bool isClick = false;     // クリックされたか
    private bool isDisplayFinished = false;     // 文字の表示が終わったか
    private bool isInstantDisplayMode = false;     // 文字表示中にクリックで即時に表示終了させる

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
        if(isClick && !isDisplayFinished && !isInstantDisplayMode)
        {
            isInstantDisplayMode = true;
        }
        if(isInstantDisplayMode)
        {
            DisplayText((float)0.01);
        }
        else
        {
            if (textDisplayInterval[textNumber] != "")
            {
                DisplayText(Convert.ToSingle(textDisplayInterval[textNumber]));
            }
            else
            {
                DisplayText((float)0.1);
            }
        }
        // テキストの表示が終わったらカウントを開始する
        if (isDisplayFinished)
        {
            displayFinishedCount += Time.deltaTime;
        }
        Choices();
    }

    // テキストを表示させる
    private void DisplayText(float _displayTextInterval)
    {
        if(displayTextIntervalCount >= _displayTextInterval)
        {
            if (textCharNumber != scriptTexts[textNumber].Length)
            {
                // 一文字ずつ表示させる
                displayScriptText += scriptTexts[textNumber][textCharNumber];
                textCharNumber += 1;
            }
            else
            {
                if (textNumber != scriptTexts.Length - 1)
                {
                    isDisplayFinished = true;
                    // クリックかオート再生で6秒たったら
                    if (isClick || (isAutoMode && displayFinishedCount > 6))
                    {
                        // 入力がされているか確認
                        if (soundEffectTexts[textNumber + 1] != "")
                        {
                            // SEの再生
                            //AudioManager.instance.Play(soundEffectTexts[textNumber + 1]);
                        }
                        isDisplayFinished = false;
                        isInstantDisplayMode = false;
                        // 入力されていた文字を消す
                        displayScriptText = "";
                        // 現在表示の番数を初期化
                        textCharNumber = 0;
                        // 通常時の遷移
                        if (normalTrasitionLine[textNumber] != "")
                        {
                            textNumber = int.Parse(normalTrasitionLine[textNumber]) - 3;
                        }
                        textNumber += 1;
                        // 終了のカウントを初期化
                        displayFinishedCount = 0;
                        // タイムラインを再開する
                        playableDirector.Resume();
                    }
                }
                else
                {
                    isDisplayFinished = false;
                }
            }
            displayTextIntervalCount = 0;
            scriptTextObject.text = displayScriptText;
            nameTextObejct.text = nameTexts[textNumber];
            roleTextObejct.text = roleTexts[textNumber];
            clickCheck.isClick = false;
        }
    }

    // 選択肢の表示、非表示
    private void Choices()
    {
        // ボタンのテキストがあるか判定
        if (choiceOne[textNumber] != "" && choiceTwo[textNumber] != "")
        {
            // 選択肢が見えるようになる
            choiceButtonOneObject.SetActive(true);
            choiceButtonTwoObject.SetActive(true);
            // ボタンの文字をCSVで入力された奴にする
            choiceButtonOneTextObject.text = choiceOne[textNumber];
            choiceButtonTwoTextObject.text = choiceTwo[textNumber];
            // クリック判定の受付をしない
            clickCheck.gameObject.SetActive(false);
        }
        else
        {
            // 選択肢を見えなくする
            choiceButtonOneObject.SetActive(false);
            choiceButtonTwoObject.SetActive(false);
            // クリック判定の受付をする
            clickCheck.gameObject.SetActive(true);
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
}
