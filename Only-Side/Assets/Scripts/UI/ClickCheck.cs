using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCheck : MonoBehaviour
{
    public bool isClick;

    public void OnClicked()
    {
        if(!StoryManager.isOpenMainMenu)
        {
            isClick = true;
        }
    }
}
