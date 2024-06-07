using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogTexts : MonoBehaviour
{
    public TextMeshProUGUI scriptTextObject;     // // 原稿のテキストを表示するオブジェクト
    public TextMeshProUGUI nameTextObject;     // 名前のテキストを表示するオブジェクト
    public int textNumber;     // n番目

    private int choiceSelectNumber;     // 選んだ選択肢の番号
    private int previousChoiceSelectNumberSaveListLength;     // リストの長さの変更確認用

    private void Start()
    {
        // 初期状態でのリストの長さを保存
        previousChoiceSelectNumberSaveListLength = 
            StoryManager.choiceSelectNumberSaveList.Count;
    }

    public void Update()
    {
        // テキストが入力されているとき
        if (StoryManager.scriptTexts[textNumber] != "")
        {
            // テキストを設定する
            scriptTextObject.text = StoryManager.scriptTexts[textNumber];
            nameTextObject.text = StoryManager.nameTexts[textNumber];
        }
        // 選択肢が表示されているとき
        else
        {
            // 名前のテキストを表示させない
            nameTextObject.text = "";
            // choiceSelectNumberがリストの範囲内かチェック
            if (choiceSelectNumber >= 0 && choiceSelectNumber < StoryManager.choiceSelectNumberSaveList.Count)
            {
                // 1の選択肢が選ばれたら
                if (StoryManager.choiceSelectNumberSaveList[choiceSelectNumber] == 1)
                {
                    // 1の選択肢と同じテキストに
                    scriptTextObject.text = StoryManager.choiceOne[textNumber];
                }
                // 2の選択肢が選ばれたら
                else if (StoryManager.choiceSelectNumberSaveList[choiceSelectNumber] == 2)
                {
                    // 2の選択肢と同じテキストに
                    scriptTextObject.text = StoryManager.choiceTwo[textNumber];
                }
            }
        }
        // choiceSelectNumberSaveListのリストの長さを監視
        monitorChoiceSelectNumber();
    }

    // choiceSelectNumberSaveListのリストの長さを監視
    private void monitorChoiceSelectNumber()
    {
        // choiceSelectNumberSaveListの長さが変更されたとき
        if (previousChoiceSelectNumberSaveListLength != StoryManager.choiceSelectNumberSaveList.Count)
        {
            choiceSelectNumber = StoryManager.choiceSelectNumberSaveList.Count;
            // 現在のリストの長さを保存
            previousChoiceSelectNumberSaveListLength = 
                StoryManager.choiceSelectNumberSaveList.Count;
        }
    }
}
