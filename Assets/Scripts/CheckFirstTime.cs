using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using System;

public class CheckFirstTime : MonoBehaviour
{
    public int firstRun = 0;

    public string stato;
    public string folderPath;

    // Start is called before the first frame update
    void Start()    
    {
        //Debug.Log(stato);
        folderPath = (Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer ?
			Application.persistentDataPath : Application.dataPath) + "/Save/";

        if (!Directory.Exists(folderPath))
        {
            PlayerPrefs.SetString("시작일", DateTime.Now.AddDays(1).ToString("yyyy_MM_dd")); //시작일에 하루를 더한다\
            PlayerPrefs.Save();
            Directory.CreateDirectory(folderPath);
        }

        string filePath = folderPath + "MetaData.json";
        if (File.Exists(filePath)){
            SceneManager.LoadScene("HomeScene");
        }
        
    }

}