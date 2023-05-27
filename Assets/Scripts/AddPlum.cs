using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPlum : MonoBehaviour
{
    public GameObject plum;
    private GameObject normalPanel;
    public static System.DateTime StartDate; // = System.Convert.ToDateTime(PlayerPrefs.GetString("시작일"));
    public static System.DateTime NowDate; // = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));//System.Convert.ToDateTime()
    public List<GameObject> firstArray = new List<GameObject>(); 
    public List<GameObject> secondArray = new List<GameObject>();

    public List<GameObject> PlumTotalList = new List<GameObject>();

    public GameObject firstDayGameObject;
    public GameObject secondDayGameObject;
    static System.TimeSpan timeCal; //= NowDate - StartDate;
    public int timeCalDay; // = timeCal.Days;
    public GameObject plumManager;
    public PlumManager manager;

    private GameObject cantMovePlumPanel;

    // Start is called before the first frame update
    void Start()
    {
        StartDate = manager.startDate;
        NowDate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        timeCal = NowDate - StartDate;
        timeCalDay = timeCal.Days;

        cantMovePlumPanel = GameObject.Find("Canvas").transform.Find("CantMovePlumUI").gameObject;

        int numPlum = manager.day1.Count;
        int i = 0;
        foreach(PlumObject pob in manager.day1){
            GameObject go = Instantiate (plum, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            // go.ClickedPlum
            go.GetComponent<PlumStatus>().ClickedPlum += PlumClicked;
            go.GetComponent<PlumStatus>().idx = 10 + i;
            go.GetComponent<PlumStatus>().setStatus(pob.plumStatus);
            go.GetComponent<PlumStatus>().whichDay = 1;
            go.name = "Plum_Day1_" + i.ToString();
            go.transform.SetParent(firstDayGameObject.transform);
            //go.GetComponent<Toggle>().interactable = false;

            firstArray.Add(go);
            PlumTotalList.Add(go);
            i ++;
        }

        numPlum = manager.day2.Count;
        i=0;
        foreach(PlumObject pob in manager.day2){
            GameObject go = Instantiate (plum, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.GetComponent<PlumStatus>().ClickedPlum += PlumClicked;
            go.GetComponent<PlumStatus>().idx = 20 + i;
            go.GetComponent<PlumStatus>().setStatus(pob.plumStatus);
            go.GetComponent<PlumStatus>().whichDay = 2;
            go.name = "Plum_Day2_" + i.ToString();
            go.transform.SetParent(secondDayGameObject.transform);
            secondArray.Add(go);
            PlumTotalList.Add(go);
//            if(timeCalDay % 2 == 0) // 오른쪽
                //go.GetComponent<Toggle>().interactable = false;

            i++;
        }

        plumManager.GetComponent<PlumManager>().UpdatedPlumStatus += PlumViewUpdate;
        // firstDayGameObject.GetComponent<DynamicGrid>().SetDynamicGrid(firstArray.Count, 6, 6);
        
    }

    private void PlumViewUpdate(object sender, EventArgs e)
    {
        int numPlum1 = manager.day1.Count;
        int numPlum2 = manager.day2.Count;

        int i = 0;
        foreach(GameObject go in PlumTotalList){
            go.transform.SetParent(null);
        }


        foreach(PlumObject plums in manager.day1)
        {
            PlumTotalList[i].GetComponent<PlumStatus>().setStatus(plums.plumStatus);
            PlumTotalList[i].transform.SetParent( firstDayGameObject.transform);
            PlumTotalList[i].GetComponent<PlumStatus>().whichDay = 1;
            i++;
        }

        foreach(PlumObject plums in manager.day2)
        {
            PlumTotalList[i].GetComponent<PlumStatus>().setStatus(plums.plumStatus);
            PlumTotalList[i].transform.SetParent(secondDayGameObject.transform);
            PlumTotalList[i].GetComponent<PlumStatus>().whichDay = 2;
            i++;
        }
    }

    public void PlumClicked(object sender, EventArgs e)
    {
        GameObject plumObj  = (GameObject) sender;
        // Debug.Log(plumStatus.idx);
        if(plumObj.GetComponent<PlumStatus>().status == 1){
            if(plumObj.GetComponent<PlumStatus>().whichDay == 1) { //첫째날
                plumManager.GetComponent<PlumManager>().changePlumStatusWClick(plumObj.transform.GetSiblingIndex(), 1, 1);
            }
            else{
                plumManager.GetComponent<PlumManager>().changePlumStatusWClick(plumObj.transform.GetSiblingIndex(), 1, 2);
            }
            // plumObj.GetComponent<PlumStatus>().setStatus(2);
            // plumManager.GetComponent<PlumManager>().changePlumStatusInDay1OrDay2(1, 2, 1);
            

        }
        else if(plumObj.GetComponent<PlumStatus>().status == 2){
            // plumObj.GetComponent<PlumStatus>().setStatus(1);
            if(plumObj.GetComponent<PlumStatus>().whichDay == 1)
            {
                plumManager.GetComponent<PlumManager>().changePlumStatusWClick(plumObj.transform.GetSiblingIndex(), 2, 1);
            }
            else
            {
                plumManager.GetComponent<PlumManager>().changePlumStatusWClick(plumObj.transform.GetSiblingIndex(), 2, 2);
            }
            
            // plumManager.GetComponent<PlumManager>().changePlumStatusInDay1OrDay2(1, 1, 2);
        }
    }

    public void movePlumToRight()
    {
        int firstDayCnt = firstArray.Count;

        if(firstDayCnt != 0)
        {
            manager.moveToDay2();
        }
        else
        {
            cantMovePlumPanel.SetActive(true);
        }
    }

    public void movePlumToLeft(){
        int secondDayCnt = secondArray.Count;
        
    
        if(secondDayCnt != 0)
        {
            manager.moveToDay1();
        
        }
        else
        {
            cantMovePlumPanel.SetActive(true);
        }
    }

    
    public void TransPlum()
    {
        MovePlum movePlum = GameObject.Find("Canvas").transform.Find("MovementCounter").GetComponent<MovePlum>();
        int count = movePlum.count;
    }            
}