using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryUI : MonoBehaviour
{
    [SerializeField]
    LayoutElement layoutElementPrefab;

    public GameObject plumManager;

    [SerializeField]
    Dictionary<string, List<PlumObject>> historyData;

    private bool isStarted = false;

    void Start()
    {
        isStarted = true;
        InitHistoryView();
    }

    void OnEnable()
    {
        if(this.isStarted)
        {
            InitHistoryView();
        }
    }

    void OnDisable()
    {
        if (this.isStarted)
        {
            removeLayer();
        }
    }

    private void InitHistoryView()
    {
        plumManager.GetComponent<PlumManager>().historyUpdate();
        historyData = plumManager.GetComponent<PlumManager>().readHistory(); // returns dictionary of filename and List<PlumObject>

        List<string> fileNameList = new List<string>(historyData.Keys);
        fileNameList.Sort();

        for (int i = 0; i < fileNameList.Count / 2; i++)
        {
            string day1FileName = fileNameList[i * 2];
            string day2FileName = fileNameList[i * 2 + 1];
            
            //Debug.Log(DateTime.ParseExact(day1FileName.Replace("PlumData_", ""),"yyyy_MM_dd",null));
            addLayer(day1FileName.Replace("PlumData_", ""), historyData[day1FileName],
                day2FileName.Replace("PlumData_", ""), historyData[day2FileName]);
        }
    }
//            plumManager.GetComponent<PlumManager>().LoadJson(day1FileName); // returns List<PlumObject>
            //history 열때 json파일 저장

    private void addLayer(string day1, List<PlumObject> day1PlumList, string day2, List<PlumObject> day2PlumList)
    {
        var snap = this.GetComponentInChildren<ScrollSnap>();
        LayoutElement newPage = Instantiate(layoutElementPrefab);
        HistoryPlumComponent historyComponent = newPage.GetComponent<HistoryPlumComponent>();
        historyComponent.CreatePlumGrid(day1, day1PlumList, true);
        historyComponent.CreatePlumGrid(day2, day2PlumList, false);
        snap.PushLayoutElement(newPage);
    }

    private void removeLayer()
    {
        var snap = this.GetComponentInChildren<ScrollSnap>();

        int layoutCount = snap.LayoutElementCount();

        for(int i = 0; i< layoutCount; i++)
        {
            snap.PopLayoutElement();
        }
       
    }

    public void Show() // 도움말 버튼을 눌렀을 때 처리
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
