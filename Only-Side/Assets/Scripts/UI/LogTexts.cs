using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogTexts : MonoBehaviour
{
    public StoryManager storyManager;
    public TextMeshProUGUI scriptTextObject;
    public TextMeshProUGUI nameTextObject;

    public void Update()
    {
        scriptTextObject.text = storyManager.scriptTexts[storyManager.textNumber - 1];
        nameTextObject.text = storyManager.nameTexts[storyManager.textNumber - 1];
    }
}
