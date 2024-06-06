using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// ボタンなどのUIで使う関数用スクリプト
public class Function : MonoBehaviour
{
    [SerializeField]
    private StoryManager storyManager;
    public GameObject mainMenuGameObject;     // メインメニューのオブジェクト

    // オート再生機能のオンオフ
    public void SwitchAutoMode()
    {
        StoryManager.isAutoMode = !StoryManager.isAutoMode;
    }

    // 選択肢1のときに実行
    public void ChoiceSelect1()
    {
        if(storyManager.trasitionLineOne[storyManager.textNumber] != "")
        {
            storyManager.textNumber = int.Parse(
                storyManager.trasitionLineOne[storyManager.textNumber]) - 2;
            // タイムラインを再開する
            storyManager.playableDirector.Resume();
            StoryManager.choiceSelectNumberSaveList.Add(1);
        }
    }

    // 選択肢2のときに実行
    public void ChoiceSelec2()
    {
        if (storyManager.trasitionLineTwo[storyManager.textNumber] != "")
        {
            storyManager.textNumber = int.Parse(
                storyManager.trasitionLineTwo[storyManager.textNumber]) - 2;
            // タイムラインを再開する
            storyManager.playableDirector.Resume();
            StoryManager.choiceSelectNumberSaveList.Add(2);
        }
    }

    // タイムラインを止める
    public void StopTimeline()
    {
        storyManager.playableDirector.Pause();
        storyManager.isTimelineFinished = true;
    }

    // 効果音を鳴らす
    public void SoundEffect()
    {
        if(storyManager.soundEffectTexts[storyManager.textNumber] != "")
        {
            AudioManager.instance.Play(
                storyManager.soundEffectTexts[storyManager.textNumber]);
        }
    }

    // メニューを開く
    public void OpenMainMenu()
    {
        mainMenuGameObject.SetActive(true);
        StoryManager.isOpenMainMenu = true;
    }

    // メニューを閉じる
    public void CloseMainMenu()
    {
        mainMenuGameObject.SetActive(false);
        StoryManager.isOpenMainMenu = false;
    }
}
