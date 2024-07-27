using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

// ボタンなどのUIで使う関数用スクリプト
public class Function : MonoBehaviour
{
    [SerializeField]
    public StoryManager storyManager;
    public GameObject mainMenuGameObject;     // メインメニューのオブジェクト
    public GameObject settingsMenuGameObject; // 設定メニューのオブジェクト
    public string sceneName;                  // シーン名

    // オート再生機能のオンオフ
    public void SwitchAutoMode()
    {
        StoryManager.isAutoMode = !StoryManager.isAutoMode;
    }

    // 選択肢1のときに実行
    public void ChoiceSelect1()
    {
        if(StoryManager.trasitionLineOne[storyManager.textNumber] != "")
        {
            storyManager.textNumber = int.Parse(
                StoryManager.trasitionLineOne[storyManager.textNumber]) - 2;
            // タイムラインを再開する
            storyManager.playableDirector.Resume();
            StoryManager.choiceSelectNumberSaveList.Add(1);
        }
    }

    // 選択肢2のときに実行
    public void ChoiceSelec2()
    {
        if (StoryManager.trasitionLineTwo[storyManager.textNumber] != "")
        {
            storyManager.textNumber = int.Parse(
                StoryManager.trasitionLineTwo[storyManager.textNumber]) - 2;
            // タイムラインを再開する
            storyManager.playableDirector.Resume();
            StoryManager.choiceSelectNumberSaveList.Add(2);
        }
    }

    // タイムラインを止める
    public void StopTimeline()
    {
        StoryManager.instance.playableDirector.Pause();
        StoryManager.instance.isTimelineFinished = true;
    }

    // 効果音を鳴らす
    public void SoundEffect()
    {
        if(StoryManager.soundEffectTexts[storyManager.textNumber] != "")
        {
            AudioManager.instance.Play(
                StoryManager.soundEffectTexts[storyManager.textNumber]);
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

    // 設定メニューを開く
    public void OpenSettingsMenu()
    {
        settingsMenuGameObject.SetActive(true);
    }

    // 設定メニューを閉じる
    public void CloseSettingMenu()
    {
        settingsMenuGameObject.SetActive(false);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
