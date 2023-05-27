using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlumCheckEnabler : MonoBehaviour
{
    public static System.DateTime StartDate; // = System.Convert.ToDateTime(PlayerPrefs.GetString("시작일"));
    public static System.DateTime NowDate; // = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));//System.Convert.ToDateTime()
    static System.TimeSpan timeCal; //= NowDate - StartDate;
    public int timeCalDay; // = timeCal.Days;

    
    public GameObject firstDay;
    public GameObject secondDay;
    // Start is called before the first frame update


    // Update is called once per frame
    public void EnablePlumCheck()
    {
        StartDate = System.Convert.ToDateTime(PlayerPrefs.GetString("시작일"));
        NowDate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        timeCal = NowDate - StartDate;
        timeCalDay = timeCal.Days;

         if(timeCalDay % 2 == 0) // 오른쪽
        {
            // firstDay.GetComponent<CanvasGroup>().interactable = true;
            int childCount = firstDay.transform.childCount;
            for (int i = 0 ; i < childCount; i++){
                GameObject childGOB = firstDay.transform.GetChild(i).gameObject;
                if(childGOB.GetComponent<PlumStatus>().status != 0)
                    childGOB.GetComponent<Toggle>().interactable = true;
                
            }
        }
            
        else // 왼쪽
        {
            
            int childCount = secondDay.transform.childCount;
            for (int i = 0 ; i < childCount; i++){
                GameObject childGOB = secondDay.transform.GetChild(i).gameObject;
                Debug.Log(childGOB.GetComponent<PlumStatus>().status);
                if(childGOB.GetComponent<PlumStatus>().status != 0)
                    childGOB.GetComponent<Toggle>().interactable = true;
                
            }
        }

    }

    public void DisablePlumCheck()
    {
        StartDate = System.Convert.ToDateTime(PlayerPrefs.GetString("시작일"));
        NowDate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        timeCal = NowDate - StartDate;
        timeCalDay = timeCal.Days;

         if(timeCalDay % 2 == 0) // 오른쪽
        {
            int childCount = firstDay.transform.childCount;
            for (int i = 0 ; i < childCount; i++){
                GameObject childGOB = firstDay.transform.GetChild(i).gameObject;
                //if(childGOB.GetComponent<PlumStatus>().status != 0)
                    childGOB.GetComponent<Toggle>().interactable = false;
                
            }
        }
            
        else // 왼쪽
        {
            int childCount = secondDay.transform.childCount;
            for (int i = 0 ; i < childCount; i++){
                GameObject childGOB = secondDay.transform.GetChild(i).gameObject;
                Debug.Log(childGOB.GetComponent<PlumStatus>().status);
                //if(childGOB.GetComponent<PlumStatus>().status != 0)
                    childGOB.GetComponent<Toggle>().interactable = false;
                
            }
        }
        
    }
}
