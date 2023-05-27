using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlum : MonoBehaviour
{
    string gameObjName; 
    public List<GameObject> firstPlumArray = new List<GameObject>();
    public List<GameObject> secondPlumArray = new List<GameObject>();
    public Sprite seedImage;
    // Start is called before the first frame update
    public int firstCount = 0;
    public int secondCount = 0;
    static System.TimeSpan timeCal; //= NowDate - StartDate;
    public int timeCalDay; // = timeCal.Days;
    public static System.DateTime StartDate; // = System.Convert.ToDateTime(PlayerPrefs.GetString("시작일"));
    public static System.DateTime NowDate; // = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));//System.Convert.ToDateTime()
    public List<int> plumLeft = new List<int>();
    public List<int> plumRight = new List<int>();

    public int count;

    void Awake()
    {    
        //PlayerPrefs.SetString("시작일", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));//DateTime.Now.ToString("yyyy-MM-dd"));
        StartDate = Convert.ToDateTime(PlayerPrefs.GetString("시작일"));
        print(PlayerPrefs.GetString("시작일"));
        NowDate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        timeCal = NowDate - StartDate;
        timeCalDay = timeCal.Days;
        //dayOddOrEven = GameObject.Find("PlumMaker").GetComponent<AddPlum>().timeCalDay;
        int numPlum = PlayerPrefs.GetInt("사용시간")/10;
        firstPlumArray = GameObject.Find("PlumMaker").GetComponent<AddPlum>().firstArray;
        secondPlumArray = GameObject.Find("PlumMaker").GetComponent<AddPlum>().secondArray;
        
        for(int i = 0; i < numPlum; i++)
         {
             plumLeft.Add(1); // 0은 씨앗, 1은 자두
             plumRight.Add(1); // 처음엔 전부 1임
        }
    }

    public void LeftToRight()
    {
        count++;    
    }

    public void RightToLeft()
    {
        count--;
    }

    public void distinguishSeed()
    {
        int numPlum = PlayerPrefs.GetInt("사용시간")/10;
        if(timeCalDay % 2 == 0) // 첫째날
        {
            if(firstCount<numPlum && firstPlumArray[firstCount].GetComponent<Button>().interactable == true)
            {
                plumLeft[firstCount] = 0;
                firstCount++;
            }
            else if(secondCount < numPlum && secondPlumArray[numPlum-secondCount-1].GetComponent<Button>().interactable == true)
            {
                plumRight[numPlum-secondCount-1] = 0;
                secondCount++;
            }
        }

        if(timeCalDay % 2 == 1) // 둘째날
        {
            if(secondCount < numPlum && secondPlumArray[secondCount].GetComponent<Button>().interactable == true)
            {
                plumRight[secondCount] = 0;
                secondCount++;
            }
            else if(firstPlumArray[numPlum-firstCount-1].GetComponent<Button>().interactable == true)
            {
                plumLeft[numPlum-firstCount-1] = 0;
                firstCount++;
            }

        }
        

    }

}