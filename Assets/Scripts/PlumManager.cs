using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using FinishPlum;
using TMPro;


[Serializable]
public class PlumObject
{
    // 0은 씨앗, 1은 일반 자두, 2는 대기중 자두, 3은 사용중 자두
    public int plumStatus = 1;

    public string usingStartDate;
    public string usingEndDate;
}

[Serializable]
public class PlumMetaData
{
    public static PlumMetaData readOrCreate(string folderPath)
    {
        string filePath = folderPath + "MetaData.json";
         if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            PlumMetaData data = JsonUtility.FromJson<PlumMetaData>(jsonString);
            //data.startDate = DateTime.Parse(data.startDateStr);
            return data;
        }
        else
        {
            PlumMetaData data = new PlumMetaData();
            data.filePath = filePath;
            data.Save();
            return data;
        }
    }

    //[SerializeField]
    public DateTime startDate;

    [SerializeField]
    private string startDateStr;

    [SerializeField]
    public DateTime AppStartDate;


    [SerializeField]
    public string filePath;
    [SerializeField]
    public int frequency = 0;

    [SerializeField]
    public int SecPerPlum = 10;

    [SerializeField]
    public int PlumPerDay = 12;

    [SerializeField]
    public int durationDay = 14;

    public List<String> usedDate = new List<String>();


    public void Save()
    {
        //startDateStr = startDate.ToString();
        string toJson = JsonUtility.ToJson(this, true);
        File.WriteAllText(filePath, toJson);
    }
    
}

public class PlumManager : MonoBehaviour
{
    
    public int numPlum = 0;
    public bool isDay1 = true;

    public PlumMetaData metaData;
    public TMP_InputField password;

    public string day1FileName;
    public string day2FileName;
    public string tempFileName;

    public List<PlumObject> day1 = new List<PlumObject>();
    public List<PlumObject> day2 = new List<PlumObject>();
    public List<PlumObject> temp = new List<PlumObject>();

    public DateTime startDate;
    public DateTime nowDate;

    private string jsonPathTemplate;

    public event EventHandler UpdatedPlumStatus;

    public GameObject TodayDateManager;
    public GameObject TimerTracker;
    public GameObject TimerUIPanel;
    public ButtonMetaData ButtonData;

    public PlumMetaData MetaData;

    string startDateString;

    bool pauseStatus;
    bool projectDone;
    void Start()
    {
        
		string folderPath = (Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer ?
			Application.persistentDataPath : Application.dataPath) + "/Save/";

		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

        metaData = PlumMetaData.readOrCreate(folderPath);

		jsonPathTemplate = folderPath + "PlumData_{0}.json";
        numPlum = metaData.PlumPerDay;

        //startDate = metaData.startDate;
        nowDate = DateTime.Now;

        ButtonData = ButtonMetaData.readOrCreate(folderPath);

        startDateString = PlayerPrefs.GetString("시작일");

        projectDone = false;
        isDay1 = true; // 새로 정해야함
        if(metaData.frequency %2 == 1) // 사용횟수 홀수일 경우
        {
            if(DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+7) // 첫째주
            {
                if(ButtonData.firstWeekDateStr.Count % 2 == 1) //
                {
                    isDay1 = true;
                }
                else
                {
                    isDay1 = false;
                }
            }
            else if (DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+13)// 둘째주
            {
                if(ButtonData.secondWeekDateStr.Count % 2 == 0)
                {
                    isDay1 = true;
                }
                else
                {
                    isDay1 = false;
                }
            }
            
        }
        else // 사용횟수 짝수일 경우
        {
            if(DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+7) // 첫째주
            {
                if(ButtonData.firstWeekDateStr.Count % 2 == 1)
                {
                    isDay1 = true;
                }
                else
                {
                    isDay1 = false;
                }
            }
            else if (DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+13)// 둘째주
            {
                if(ButtonData.secondWeekDateStr.Count == 0) // 첫날
                {
                    isDay1 = true;
                }
                else if(ButtonData.secondWeekDateStr.Count % 2 == 1) // 홀수번째
                {
                    isDay1 = true;
                }
                else // 짝수번째
                {
                    isDay1 = false;
                }
            }
        }
        // 홀수일 경우 week1 count가 홀수면서 week2 count가 0 or 짝수일 때
        // 짝수일 경우 week1 count가 홀수거나 week2 count가 홀수일 때

        int index = ButtonData.usedDays-1;

        day1FileName = nowDate.ToString("yyyy_MM_dd");
        tempFileName = "temp";

        if(DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+7)// 첫째주면
        {
            // 이번 주 안에 다 썻는지 덜 썼는지 --> 안 봐도 되지 않나?
            if(!isDay1) // 짝수번째 사용일
            {
                day1FileName = ButtonData.firstWeekDateStr[index-1];
                day2FileName = nowDate.ToString("yyyy_MM_dd");
            }
            else // 홀수번째 사용일
            {
                day1FileName = nowDate.ToString("yyyy_MM_dd");
               // File.Delete(folderPath+"PlumData_temp.json");
                day2FileName ="temp";
            }
        }
        
        else if (DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+13)// 둘째주
        {
            if(ButtonData.secondWeekDateStr.Count < metaData.frequency) //아직 이번주에 남았음
            {
                if(metaData.frequency % 2 == 0) // 사용횟수가 짝수이면
                {
                    if(!isDay1) // 짝수번째 사용일
                    {
                        //if(ButtonData.firstWeekDateStr.Count < metaData.frequency) // 첫째주에 덜 썼으면 -> 인덱스를 바꿔줘야함
                        day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-2];
                        day2FileName = nowDate.ToString("yyyy_MM_dd");
                    }
                    else // 홀수번째 사용일
                    {
                        day1FileName = nowDate.ToString("yyyy_MM_dd");
                        //File.Delete(folderPath+"PlumData_temp.json");
                        day2FileName ="temp";
                    }
                }
                else // 사용횟수가 홀수이면 
                {
                    if(!isDay1) // 홀수 번째 사용일
                    {
                        if(ButtonData.secondWeekDateStr.Count == 1) // 첫째날
                        {
                            if(ButtonData.firstWeekDateStr.Count < metaData.frequency) // 첫째주 덜썼음
                            {
                                Debug.Log("help");
                                if(!File.Exists(folderPath+"PlumData_"+DateTime.Now.ToString("yyyy_MM_dd")+".json")) // 그날 첫번째 실행이면
                                {
                                    File.Delete(folderPath+"PlumData_temp.json");
                                }
                                
                                day1FileName = "temp";
                                day2FileName = DateTime.Now.ToString("yyyy_MM_dd");
                            }
                            else // 첫째주 다썻다!
                            {
                                day1FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1]; // 마지막거
                                day2FileName = "temp";
                            }
                            
                        }
                        else // 셋째날~
                        {
                            day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-2];
                        }
                        day2FileName = nowDate.ToString("yyyy_MM_dd");
                        if(ButtonData.secondWeekDateStr.Count == metaData.frequency) // 둘째주 다채웟음
                        {
                            day2FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
                        }
                        // else
                        // {
                        //     if(DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).AddDays(1).DayOfYear+14)
                        //     {
                        //         day2FileName = nowDate.ToString("yyyy_MM_dd");
                        //     }
                        // }

                    }
                    else // 짝수 번째 사용일
                    {
                        if(ButtonData.secondWeekDateStr.Count == metaData.frequency)
                        {
                            day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-2];
                            day2FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
                        }
                        else
                        {
                            day1FileName = nowDate.ToString("yyyy_MM_dd");
                            if(!File.Exists(folderPath+"PlumData_"+DateTime.Now.ToString("yyyy_MM_dd")+".json")) // 그날 첫번째 실행이면
                            {
                                File.Delete(folderPath+"PlumData_temp.json");
                            }
                            day2FileName = "temp";
                        }
                    }
                }
            }
            else // 다 쓰고 날짜 지난 날
            {
                day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-2];
                day2FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
            }

        // else // 다 쓰고 날짜 지난 날
        // {
        //     projectDone = true;
        //     if(ButtonData.secondWeekDateStr.Count < metaData.frequency) //둘째주 덜 썼을 때
        //     {
        //         if(metaData.frequency % 2 == 0) // 짝수일 때
        //         {
        //             if(ButtonData.secondWeekDateStr.Count == 0) // 둘째주 한 번도 안 썼을 때 
        //             {
        //                 if(ButtonData.firstWeekDateStr.Count % 2 == 1) // 첫째주 홀수번 썻을 때
        //                 {
        //                     day1FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1];
        //                     day2FileName = "temp";
        //                 }
        //                 else // 첫째주 짝수번 썻을 때
        //                 {
        //                     if(ButtonData.firstWeekDateStr.Count == 0) // 첫째주에 한 번도 안 썼음
        //                     {
        //                         //File.Delete(folderPath+"PlumData_temp.json");
        //                         day1FileName = "temp";
        //                         day2FileName = "temp";
        //                     }

        //                     else // 첫째주에 두번은 썼음
        //                     {
        //                         day1FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-2];
        //                         day2FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1];    
        //                     }
        //                 }

        //             }
        //             else if(ButtonData.secondWeekDateStr.Count == 1) // 둘째주에 한 번 밖에 안 썼음
        //             {
        //                 day1FileName = ButtonData.secondWeekDateStr[0];
        //                 day2FileName = "temp";
                        
        //             }
        //             else // 둘째주에 두 번 이상 썼음
        //             {
        //                 if(ButtonData.secondWeekDateStr.Count % 2 == 0) // 둘째주에 짝수번 씀
        //                 {
        //                     day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-2];
        //                     day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
        //                 }
                        
        //                 else // 둘째주에 홀수번 씀
        //                 {
        //                     day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
        //                     day2FileName = "temp";    
        //                 }
        //             }
        //         }

        //         else // 사용횟수 홀수
        //         {
        //             if(ButtonData.secondWeekDateStr.Count == 0) // 둘째 주에 한 번도 안 씀
        //             {
        //                 if(ButtonData.firstWeekDateStr.Count % 2 == 1) // 첫째주에 홀수번 씀
        //                 {
        //                     day1FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1];
        //                     day2FileName = "temp";
        //                 }
        //                 else // 첫째주에 짝수만큼 씀
        //                 {
        //                     if(ButtonData.firstWeekDateStr.Count == 0) // 첫째주에 한 번도 안 썼음
        //                     {
        //                         //File.Delete(folderPath+"PlumData_temp.json");
        //                         day1FileName = "temp";
        //                         day2FileName = "temp";
        //                     }

        //                     else
        //                     {
        //                         day1FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-2];
        //                         day2FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1];    
        //                     }
        //                 }

        //             }
        //             else if(ButtonData.secondWeekDateStr.Count == 1) // 둘째주에 한 번 밖에 안 썼음
        //             {
        //                 if(ButtonData.firstWeekDateStr.Count == metaData.frequency) // 첫째주에 다 썼음
        //                 {
        //                     day1FileName = ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1];
        //                 }   
        //                 else // 첫째주에 덜씀
        //                 {
        //                     //File.Delete(folderPath+"PlumData_temp.json");
        //                     day1FileName = "temp";
        //                 }
        //                 day2FileName = ButtonData.secondWeekDateStr[0];
                        
        //             }
        //             else // 둘째주에 두번이상 씀
        //             {
        //                 if(ButtonData.secondWeekDateStr.Count % 2 == 0) // 짝수번 씀
        //                 {
        //                     day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
        //                     day2FileName = "temp";
                            
        //                 }
        //                 else // 홀수번 씀
        //                 {
        //                     day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-2];
        //                     day2FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
        //                 }
                        
        //             }
        //         }
        //     }
        //     else // 둘째주 다 썻을때
        //     {
        //         day1FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-2];
        //         day2FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
        //     }
 
        }

        TodayDateManager.GetComponent<ShowToday>().ChangeBackground(isDay1);

        bool secondFileExists = File.Exists(folderPath+"PlumData_"+DateTime.Now.ToString("yyyy_MM_dd")+".json");

        day1 = LoadOrCreate(day1FileName);
        day2 = LoadOrCreate(day2FileName); // day2가 null로 들어가있는 상황
        temp = LoadOrCreate(tempFileName);
        if(!isDay1 && !secondFileExists && !projectDone)
        {
            swapSecondDayPlum();
        }        
        
        updatePlumList(day1);
        updatePlumList(day2);
        if( (ButtonData.secondWeekDateStr.Count == metaData.frequency && DateTime.ParseExact(ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1], "yyyy_MM_dd", null).DayOfYear < DateTime.Now.DayOfYear))// || DateTime.Now.DayOfYear >= DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+13) // 횟수 다 채웠고 시간이 다 됐을 때
        {
            //File.Delete(folderPath+"PlumData_"+DateTime.Now.ToString("yyyy_MM_dd")+".json");
            day2FileName = ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1];
            updatePlumList(day2);
            updateFinalPlumList(day1);
            updateFinalPlumList(day2);
        }

        initTimerTracker();
    }

    void OnApplicationFocus(bool pause)
    {
        if(pause) // 일시정지 되었을 때
        {
            pauseStatus = true;
            List<PlumObject> changePlumList = isDay1 ? day1 : day2;
            List<PlumObject> usingPlumList = new List<PlumObject>();
            foreach (PlumObject pob in changePlumList)
            {
                if (String.IsNullOrEmpty(pob.usingEndDate))
                    break;
                if (DateTime.Parse(pob.usingEndDate) > DateTime.Now)
                    usingPlumList.Add(pob);
            }
            updatePlumList(day1);
            updatePlumList(day2);
            if(usingPlumList.Count == 0)
            {
                TimerUIPanel.SetActive(false);
            }
        }
            
        else
        {
            if(pauseStatus) // 일시정지에서 다시 돌아왔을 때
            {
                pauseStatus = false;
                List<PlumObject> changePlumList = isDay1 ? day1 : day2;
                List<PlumObject> usingPlumList = new List<PlumObject>();
                foreach (PlumObject pob in changePlumList)
                {
                    if (String.IsNullOrEmpty(pob.usingEndDate))
                        break;
                    if (DateTime.Parse(pob.usingEndDate) > DateTime.Now)
                        usingPlumList.Add(pob);
                }
                updatePlumList(day1);
                updatePlumList(day2);
                if(usingPlumList.Count == 0)
                {
                    TimerUIPanel.SetActive(false);
                }
            }
        }
    }

    /*
     * 처음 앱을 켰을 때, 사용하고 있는 플럼이 있는지 체크한 후, 타이머를 초기화
     */
    private void initTimerTracker()
    {
        List<PlumObject> changePlumList = isDay1 ? day1 : day2;
        List<PlumObject> usingPlumList = new List<PlumObject>();
        foreach (PlumObject pob in changePlumList)
        {
            if (String.IsNullOrEmpty(pob.usingEndDate))
                break;
            if (DateTime.Parse(pob.usingEndDate) > DateTime.Now)
                usingPlumList.Add(pob);
        }
        if (usingPlumList.Count != 0)
        {
            TimerTracker.GetComponent<RecordUsageTime>().SettingTimerWPlumList(usingPlumList);
            TimerUIPanel.SetActive(true);
        }
    }

    /*
     * 처음 앱 실행했을 때, 상태정보가 잘못 되어 있는 플럼을 수정
     * 시간상 씨앗이어야 하는데, 씨앗이 아닌 플럼을 변경함 등등
     */
    public void updatePlumList(List<PlumObject> plumList)
    {
        DateTime nowTime = DateTime.Now;     
        foreach(PlumObject pob in plumList)
        {
            if(!String.IsNullOrEmpty(pob.usingStartDate) && !String.IsNullOrEmpty(pob.usingEndDate))
            {
                DateTime usingStartTime = DateTime.Parse(pob.usingStartDate);
                DateTime usingEndTime = DateTime.Parse(pob.usingEndDate);
                if (nowTime > usingStartTime && nowTime < usingEndTime)
                {
                    pob.plumStatus = 3;
                }
                else if (nowTime >= usingEndTime)
                {
                    pob.plumStatus = 0;
                }
                else if(nowTime <= usingStartTime)
                {
                    pob.plumStatus = 2;
                }

            }
        }
        CreateUIEvent();
    }

        public void updateFinalPlumList(List<PlumObject> plumList)
    {
        DateTime nowTime = DateTime.Now;     
        foreach(PlumObject pob in plumList)
        {
            pob.plumStatus = 4;
        }
        CreateUIEvent();
    }


    public void moveToDay1()
	{
        List<PlumObject> movePlumList = day2.FindAll((item) => item.plumStatus == 2);
            foreach(PlumObject movePlum in movePlumList)
            {
                if(day2.Remove(movePlum)){
                    movePlum.plumStatus=1;
                    day1.Add(movePlum);
                }
            }
        CreateUIEvent();
    }

	public void moveToDay2()
	{
        List<PlumObject> movePlumList = day1.FindAll((item) => item.plumStatus == 2);
        foreach(PlumObject movePlum in movePlumList)
            {
                if(day1.Remove(movePlum)){
                    movePlum.plumStatus=1;
                    day2.Add(movePlum);
                }
            }
        CreateUIEvent();
    }

    public void usePlum(int day, int plumNum)
    {
        List<PlumObject> changePlumList = isDay1 ? day1 : day2;

        int lastPlum = plumNum;
        for (int i = 0; i < changePlumList.Count; i++)
        {
            if (day1[i].plumStatus == 1) continue;
            if (lastPlum <= 0) break;
            day1[i].plumStatus = 0;
            lastPlum--;
        }
        CreateUIEvent();
    }


    public void addPlumUsingTimeLogInDay1OrDay(DateTime startTime, int plumSec)
    {
        List<PlumObject> changePlumList = isDay1 ? day1 : day2;

        DateTime tempStartTime = startTime;
        DateTime tempEndTime = startTime.AddSeconds(plumSec);
        foreach(PlumObject pob in changePlumList)
        {
            if(pob.plumStatus == 2 || pob.plumStatus == 3)
            {
                pob.usingStartDate = tempStartTime.ToString();
                pob.usingEndDate = tempEndTime.ToString();

                tempStartTime = tempEndTime;
                tempEndTime = tempStartTime.AddSeconds(plumSec);
            }
        }

        CreateUIEvent();
    }

    public void changePlumStatusInDay1OrDay2(int plumNum, int fromPlumStatus, int toPlumStatus)
    {
        List<PlumObject> changePlumList = isDay1 ? day1 : day2;

        int lastPlum = plumNum;
        for (int i = 0; i < changePlumList.Count; i++)
        {
            if (lastPlum <= 0) break;
            int curPlumStatus = changePlumList[i].plumStatus;
            if (curPlumStatus == 0) continue;
            if (curPlumStatus == fromPlumStatus)
            {
                changePlumList[i].plumStatus = toPlumStatus;
                lastPlum--;
            }
        }
        CreateUIEvent();
    }

    public void changePlumStatusWClick(int plumIdx, int statusp, int day)
    {
        List<PlumObject> changePlumList;// = isDay1 ? day1 : day2;
        if(day == 1)
        {
            changePlumList = day1;
        }
        else
        {
            changePlumList = day2;
        }
        
        changePlumList[plumIdx].plumStatus = statusp;
        
        CreateUIEvent();
    }

    public int getSelectedPlumNum()
    {
        List<PlumObject> changePlumList = isDay1 ? day1 : day2;
        int n = 0;
        foreach(PlumObject p in changePlumList){
            if (p.plumStatus == 2){
                n ++;
            }
        }
        return n;
    }

    public int getPlumNumWStatus(int plumStatus, bool isDay1)
    {
        List<PlumObject> changePlumList = isDay1 ? day1 : day2;
        int n = 0;
        foreach(PlumObject p in changePlumList){
            if (p.plumStatus == plumStatus){
                n++;
            }
        }
        return n;
    }

    public void changePlumStatusStopDay1OrDay2()
    {
        List<PlumObject> changePlumList = isDay1 ? day1 : day2;

        foreach(PlumObject p in changePlumList){
            if (p.plumStatus == 2){
                p.plumStatus = 1;
                p.usingStartDate = "";
                p.usingEndDate = "";
            }
            else if (p.plumStatus == 3){
                p.plumStatus = 0;
                p.usingEndDate = DateTime.Now.ToString();
            }
        }
        CreateUIEvent();
    }

    // 홀수날: 홀수날이랑 temp 안에서 사용
    // 짝수날: 홀수날이랑 짝수날로 바뀐 temp를 사용
    // 즉 temp를 짝수날 데이터로 지정해서 거기서부터 쓰면 됨
    // 이미 null 로 생성이 되어있는 걸 어떻게 day2로 가져올 것인가? -> null 이름을 바꾸고 새로 null로 생성
    // LoadOrCreate는 해당 이름을 가지고 있는 파일이 없으면 새로 만들고 있으면 그걸 불러옴
    void swapSecondDayPlum() // 짝수날 실행시 자두 날짜를 지정해줌
    {
        string folderPath = (Application.platform == RuntimePlatform.Android ||
	    		Application.platform == RuntimePlatform.IPhonePlayer ?
		    	Application.persistentDataPath : Application.dataPath) + "/Save/";
        string filePath = String.Format("{0}PlumData_temp.json", folderPath);

        if(DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+13)
        {
            day2FileName =  nowDate.ToString("yyyy_MM_dd");// 날짜를 받아와서 그 날짜를 day2로 집어넣고
            SaveJson(String.Format(jsonPathTemplate, day2FileName), temp);// plumdata_temp 이름을 day2로 바꿔줌
            day2 = LoadOrCreate("temp");

            File.Delete(filePath); // 기존에 잇던 null 파일 삭제
            temp = LoadOrCreate("temp"); // 새로 null 파일 만들어줌
            updatePlumList(day2);
            CreateUIEvent();
        }
    }

    // public void Reset()
    // {
    //     if(password.text == "CCLABHELLO")
    //     {
    //         string folderPath = (Application.platform == RuntimePlatform.Android ||
	// 		Application.platform == RuntimePlatform.IPhonePlayer ?
	// 		Application.persistentDataPath : Application.dataPath) + "/Save/";
            
    //         DateTime startDay = metaData.startDate;
    //         PlumObject[] pobjArr = new PlumObject[metaData.PlumPerDay];
    //         string toJson = JsonHelper.ToJson<PlumObject>(pobjArr, prettyPrint: true);

    //         String filePath = String.Format("{0}PlumData_{1}.json", folderPath, startDay.ToString("yyyy_MM_dd"));
    //         File.WriteAllText(filePath, toJson);
    //         filePath = String.Format("{0}PlumData_{1}.json", folderPath, startDay.AddDays(1).ToString("yyyy_MM_dd"));
    //         File.WriteAllText(filePath, toJson);
            
    //         day1 = LoadOrCreate(day1FileName);
    //         day2 = LoadOrCreate(day2FileName);
    //         CreateUIEvent(); //GameObject.Find("PlumManager").GetComponent<PlumManager>().
    //     }
    // }

    public Dictionary<String, List<PlumObject>> readHistory()
    {
        Dictionary<String, List<PlumObject>> returnDic = new Dictionary<string, List<PlumObject>>();

        string folderPath = (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer ?
            Application.persistentDataPath : Application.dataPath) + "/Save/";

        string[] filePathArr = Directory.GetFiles(folderPath, "PlumData*.json");
        foreach (string p in filePathArr)
        {
            string fileName = Path.GetFileNameWithoutExtension(p);
            returnDic.Add(fileName, LoadJson(p));
        }

        return returnDic;
    }

    private void CreateUIEvent()
    {
        if (this.UpdatedPlumStatus != null)
        {
            UpdatedPlumStatus(this, EventArgs.Empty);
        }
        SaveJson(String.Format(jsonPathTemplate, day1FileName), day1);
        SaveJson(String.Format(jsonPathTemplate, day2FileName), day2);
    }
    public void historyUpdate()
    {
        updatePlumList(day1);
        updatePlumList(day2);
        SaveJson(String.Format(jsonPathTemplate, day1FileName), day1);
        SaveJson(String.Format(jsonPathTemplate, day2FileName), day2);
        
    }

    private List<PlumObject> LoadOrCreate(string getDate)
    {
        List<PlumObject> tempList = new List<PlumObject>();
        string jsonPath = String.Format(jsonPathTemplate, getDate);
        if (File.Exists(jsonPath))
        {
            tempList = LoadJson(jsonPath);
        }
        else
        {
            // 처음 저장된 파일을 읽어오는 코드
            // 없으면 파일을 생성해서 만들어야 함
            for (int i = 0; i < numPlum; i++)
            {
                tempList.Add(new PlumObject());
            }

            SaveJson(jsonPath, tempList);
        }
        return tempList;
    }

    public void SaveJson(string filePath, List<PlumObject> saveData)
    {
        string toJson = JsonHelper.ToJson<PlumObject>(saveData.ToArray(), prettyPrint: true);
        File.WriteAllText(filePath, toJson);
    }

    public List<PlumObject> LoadJson(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        var data = JsonHelper.FromJson<PlumObject>(jsonString);
        List<PlumObject> returnData = new List<PlumObject>(data);
        return returnData;
    }


}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.plums;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.plums = array;
        return JsonUtility.ToJson(wrapper);

    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.plums = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] plums;
    }
}
