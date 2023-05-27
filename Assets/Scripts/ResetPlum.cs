using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class ResetPlum : MonoBehaviour
{
    public string folderPath;
    public TMP_InputField password;
    public PlumMetaData plumMeta;

    public event EventHandler UpdatedPlumStatus;
    public GameObject plumManager;

    public PlumManager manager;

    void Start()
    {
        folderPath = (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer ?
        Application.persistentDataPath : Application.dataPath) + "/Save/";
    }

    public void Reset()
    {
        
        plumMeta = GameObject.Find("PlumManager").GetComponent<PlumManager>().metaData;
        // List<PlumObject> day1 = GameObject.Find("PlumManager").GetComponent<PlumManager>().day1;
        // List<PlumObject> day2 = GameObject.Find("PlumManager").GetComponent<PlumManager>().day2;

        if(password.text == "CCLABHELLO")
        {
            // for(int i = 0; i < day1.Count; i++)
            // {
            //     day1[i].plumStatus = 1;
            // }

            // for(int i = 0; i < day2.Count; i++)
            // {
            //     day2[i].plumStatus = 1;
            // }
            DateTime startDay = plumMeta.startDate;
            Debug.Log(startDay);
            PlumObject[] pobjArr = new PlumObject[plumMeta.PlumPerDay];
            string toJson = JsonHelper.ToJson<PlumObject>(pobjArr, prettyPrint: true);

            for(int i = 0; i < plumMeta.PlumPerDay; i++)
            {
                Debug.Log(pobjArr[i].plumStatus);
            }

            String filePath = String.Format("{0}PlumData_{1}.json", folderPath, startDay.ToString("yyyy_MM_dd"));
            File.WriteAllText(filePath, toJson);
            filePath = String.Format("{0}PlumData_{1}.json", folderPath, startDay.AddDays(1).ToString("yyyy_MM_dd"));
            File.WriteAllText(filePath, toJson);

            // CreateUIEvent(); //GameObject.Find("PlumManager").GetComponent<PlumManager>().
        }
    }

    // private void CreateUIEvent()
    // {
    //     Debug.Log("UI Update");
    //     plumManager.GetComponent<PlumManager>().UpdatedPlumStatus += PlumViewUpdate;
    // }

    // private void PlumViewUpdate(object sender, EventArgs e)
    // {


    //     int numPlum1 = manager.day1.Count;
    //     int numPlum2 = manager.day2.Count;

    //     int i = 0;
    //     foreach(GameObject go in PlumTotalList){
    //         go.transform.parent = null;
    //     }


    //     foreach(PlumObject plums in manager.day1)
    //     {
    //         PlumTotalList[i].GetComponent<PlumStatus>().setStatus(plums.plumStatus);
    //         PlumTotalList[i].transform.SetParent( firstDayGameObject.transform);
    //         PlumTotalList[i].GetComponent<PlumStatus>().whichDay = 1;
    //         i++;
    //     }

    //     foreach(PlumObject plums in manager.day2)
    //     {
    //         PlumTotalList[i].GetComponent<PlumStatus>().setStatus(plums.plumStatus);
    //         PlumTotalList[i].transform.SetParent(secondDayGameObject.transform);
    //         PlumTotalList[i].GetComponent<PlumStatus>().whichDay = 2;
    //         i++;
    //     }
    // }
}
