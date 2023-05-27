using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ShowToday : MonoBehaviour
{
    // public static System.DateTime StartDate; // = System.Convert.ToDateTime(PlayerPrefs.GetString("시작일"));
    // public static System.DateTime NowDate; // = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));//System.Convert.ToDateTime()

    // static System.TimeSpan timeCal; //= NowDate - StartDate;
    // int timeCalDay; // = timeCal.Days;

    // public GameObject TodayLeft;
    // public GameObject TodayRight;
    // public GameObject Yesterday;
    // public GameObject Tomorrow;
    public GameObject LeftBG;
    public GameObject RightBG;
    public GameObject plumManager;
    
    // public GameObject SelectUIDay1;
    // public GameObject SelectUIDay2;

    public TextMeshProUGUI LeftDate;
    public TextMeshProUGUI RightDate;

    public ButtonMetaData ButtonData;
    
    public PlumMetaData PlumData;

    void Start()
    {
        string folderPath = (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer ?
        Application.persistentDataPath : Application.dataPath) + "/Save/";

		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

        ButtonData = ButtonMetaData.readOrCreate(folderPath);
    }

    public void ChangeBackground(bool isDay1)
    {
        if(isDay1 == true) // 첫째날
        {
            LeftDate.text = "이번 사용 ";
            RightDate.text = "다음 사용 ";
            RightBG.SetActive(false);
            LeftBG.SetActive(true);
            // SelectUIDay1.SetActive(true);
            // SelectUIDay2.SetActive(false);
        }
        else
        {
            LeftDate.text = "저번 사용 ";
            RightDate.text = "이번 사용 ";
            RightBG.SetActive(true);
            LeftBG.SetActive(false);
            // SelectUIDay1.SetActive(false);
            // SelectUIDay2.SetActive(true);

        }

        // string leftMonth = plumManager.GetComponent<PlumManager>().day1FileName.Substring(5, 2);
        // string rightMonth = plumManager.GetComponent<PlumManager>().day2FileName.Substring(5, 2);
        // string leftDay = plumManager.GetComponent<PlumManager>().day1FileName.Substring(8, 2);
        // string rightDay = plumManager.GetComponent<PlumManager>().day2FileName.Substring(8, 2);

        // string LeftInput = Regex.Replace(plumManager.GetComponent<PlumManager>().day1FileName, "[^a-zA-Z0-9% ._]", string.Empty);
        // string RightInput = Regex.Replace(plumManager.GetComponent<PlumManager>().day2FileName, "[^a-zA-Z0-9% ._]", string.Empty);

        //LeftDate.text += leftMonth + "월 " + leftDay + "일";//LeftInput;
        //RightDate.text += rightMonth + "월 " + rightDay + "일";//LeftInput;
    }
}