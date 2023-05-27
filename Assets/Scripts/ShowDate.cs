using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowDate : MonoBehaviour
{
    public TextMeshProUGUI FirstWeekDate;
    public TextMeshProUGUI SecondWeekDate;
    public string startDateString; string firstWeekEndString;
    string secondWeekStartString; string secondWeekEndString;
    string firststartMonth; string firststartDay;
    string firstEndDay; string firstEndMonth;
    string secondStartMonth; string secondStartDay;
    string secondEndMonth; string secondEndDay;

    // Start is called before the first frame update
    void Start()
    {
        startDateString = PlayerPrefs.GetString("사용일");
    }

    // Update is called once per frame
    void Update()
    {
        firstWeekEndString = DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).AddDays(6).ToString("yyyy_MM_dd");
        secondWeekStartString = DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).AddDays(7).ToString("yyyy_MM_dd");
        secondWeekEndString = DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).AddDays(14).ToString("yyyy_MM_dd");

        firststartMonth = startDateString.Substring(5, 2);
        firststartDay = startDateString.Substring(8, 2);
        firstEndMonth = firstWeekEndString.Substring(5, 2);
        firstEndDay = firstWeekEndString.Substring(8, 2);
        secondStartMonth = secondWeekStartString.Substring(5, 2);
        secondStartDay = secondWeekStartString.Substring(8, 2);
        secondEndMonth = secondWeekEndString.Substring(5, 2);
        secondEndDay = secondWeekEndString.Substring(8, 2);

        FirstWeekDate.text = "첫째 주(" + firststartMonth+"월 "+firststartDay+"일 ~ "+firstEndMonth+"월 "+firstEndDay+"일)";
        SecondWeekDate.text = "둘째 주(" + secondStartMonth+"월 "+secondStartDay+"일 ~ "+secondEndMonth+"월 "+secondEndDay+"일)";
    }
}
