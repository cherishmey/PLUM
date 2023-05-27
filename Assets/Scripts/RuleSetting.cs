using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class RuleSetting : MonoBehaviour
{
    public TMP_InputField inputTime;
    public TMP_InputField inputFreq;
    
    public CheckFirstTime checkft;

    public PlumMetaData metaPlum;

    public string folderPath;
    public GameObject plzNumUI;

    private void Start()
    {
        //PlayerPrefs.SetString("시작일", DateTime.Now.AddDays(1).ToString("yyyy_MM_dd")); //시작일에 하루를 더한다
        folderPath = (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer ?
            Application.persistentDataPath : Application.dataPath) + "/Save/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }
    // public void Save()
    // {
    //     PlayerPrefs.SetInt("사용시간", int.Parse(inputTime.text));
    //     PlayerPrefs.SetInt("사용횟수", int.Parse(inputFreq.text));
    //     PlayerPrefs.SetString("시작일", DateTime.Now.ToString("yyyy_MM_dd")); //시작일에 하루를 더한다
    // }
    public void SettingRule(int frequency, int secPerPlum, int plumPerDay, int durationDay)
    {
        PlayerPrefs.SetString("시작일", DateTime.Now.AddDays(1).ToString("yyyy_MM_dd")); //시작일에 하루를 더한다\
        PlayerPrefs.Save();
        metaPlum = PlumMetaData.readOrCreate(folderPath);
        //metaPlum.filePath = filePath;
        // metaPlum.AppStartDate = DateTime.Now;
        // metaPlum.startDate = DateTime.Now;
        metaPlum.SecPerPlum = secPerPlum;
        metaPlum.PlumPerDay = plumPerDay;
        metaPlum.durationDay = durationDay;
        metaPlum.frequency = frequency;
        metaPlum.Save();
        CreateFuturePlumData(metaPlum);
    }

    private void CreateFuturePlumData(PlumMetaData plumMeta)
    {
        DateTime startDay = plumMeta.startDate;
        PlumObject[] pobjArr = new PlumObject[plumMeta.PlumPerDay];
        string toJson = JsonHelper.ToJson<PlumObject>(pobjArr, prettyPrint: true);

        // for (int i = 1; i < metaPlum.durationDay+1; i++)
        // {
            // String filePath = String.Format("{0}PlumData_{1}.json", folderPath, startDay.AddDays(i).ToString("yyyy_MM_dd")); // 파일 생성을 설치 다음날부터
        //     File.WriteAllText(filePath, toJson);
        // }
    }


    private void CreatePlumMetaData()
    {
        metaPlum = PlumMetaData.readOrCreate(folderPath);
        metaPlum.SecPerPlum = 10;
        metaPlum.Save();
    }

    public void RuleSettingButtonClicked()
    {

		int minute = 0;
        int freq = 0;
        bool resultTime = int.TryParse(inputTime.text, out minute);
        bool resultFreq = int.TryParse(inputFreq.text, out freq);
        
        if(resultTime && resultFreq)
        {
            //minute = Int32.Parse(inputTime.text);
            int plumPerDay = (int) minute / 10;
            int frequency = (int) freq;
            int durationDay = 22;
            int secPerPlum = 600;

            SettingRule(secPerPlum: secPerPlum, plumPerDay: plumPerDay, durationDay: durationDay, frequency: frequency);

            SceneManager.LoadScene("HomeScene");
            //SceneManager.LoadScene("DailyUsageScene");
        }
        else
        {
            plzNumUI.SetActive(true);
        }           
	}
}