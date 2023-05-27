using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhichPlumUsed : MonoBehaviour
{
    public GameObject plumMaker;

    // Update is called once per frame
    public void HelpMe()
    {
        
        int numPlum = PlayerPrefs.GetInt("사용시간")/10;

        // 왼쪽 옮기기
        plumMaker.GetComponent<AddPlum>().movePlumToRight();

        // 오른쪽 옮기기
        plumMaker.GetComponent<AddPlum>().movePlumToRight();
    }
}
