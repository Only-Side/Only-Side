using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogTexts : MonoBehaviour
{
    public StoryManager storyManager;
    public TextMeshProUGUI scriptTextObject;
    public TextMeshProUGUI nameTextObject;
    public int textNumber;

    public void Update()
    {
        scriptTextObject.text = StoryManager.scriptTexts[textNumber];
        nameTextObject.text = StoryManager.nameTexts[textNumber];
    }
}
