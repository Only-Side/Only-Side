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

    private int choiceSelectNumber;
    private int previousChoiceSelectNumberSaveListLength;

    private void Start()
    {
        previousChoiceSelectNumberSaveListLength = 
            StoryManager.choiceSelectNumberSaveList.Count;
    }

    public void Update()
    {
        if (StoryManager.scriptTexts[textNumber] != "")
        {
            scriptTextObject.text = StoryManager.scriptTexts[textNumber];
            nameTextObject.text = StoryManager.nameTexts[textNumber];
        }
        else
        {
            if (StoryManager.choiceSelectNumberSaveList[choiceSelectNumber] == 1) 
            {
                scriptTextObject.text = StoryManager.choiceOne[textNumber];
            }
            else if (StoryManager.choiceSelectNumberSaveList[choiceSelectNumber] == 2)
            {
                scriptTextObject.text = StoryManager.choiceTwo[textNumber];
            }
        }
        monitorChoiceSelectNumber();
    }

    private void monitorChoiceSelectNumber()
    {
        if(previousChoiceSelectNumberSaveListLength != StoryManager.choiceSelectNumberSaveList.Count)
        {
            choiceSelectNumber = StoryManager.choiceSelectNumberSaveList.Count;
            previousChoiceSelectNumberSaveListLength = 
                StoryManager.choiceSelectNumberSaveList.Count;
        }
    }
}
