using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


[Serializable]
public class ButtonMetaData
{
    public static ButtonMetaData readOrCreate(string folderPath)
    {
        string filePath = folderPath + "ButtonData.json";
         if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            ButtonMetaData data = JsonUtility.FromJson<ButtonMetaData>(jsonString);
            // foreach(String str in data.firstWeekDateStr)
            // {
            //     data.firstWeekDate.Add(DateTime.Parse(str));
            // }
            return data;
        }
        else
        {
            ButtonMetaData data = new ButtonMetaData();
            data.filePath = filePath;
            data.Save();
            return data;
        }
    }

    //[SerializeField]
    public DateTime startDate;
    [SerializeField]
    public string filePath;
    //[SerializeField]
    public int usedDays;
    public List<DateTime> firstWeekDate = new List<DateTime>();
    public List<DateTime> secondWeekDate = new List<DateTime>();
    [SerializeField]
    public List<String> firstWeekDateStr = new List<String>();
    [SerializeField]
    public List<String> secondWeekDateStr = new List<String>();

    public void Save()
    {
        string toJson = JsonUtility.ToJson(this, true);
        File.WriteAllText(filePath, toJson);
    }
}

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject firstWeekButtons;
    public GameObject secondWeekButtons;
    private String jsonPathTemplate;
    public ButtonMetaData ButtonData;
    public PlumMetaData metaPlum;
    //public GameObject ;
    int frequency;
    int i;
    string StartDateStr;

    public TextMeshProUGUI FirstWeekDate;
    public TextMeshProUGUI SecondWeekDate;
    public string startDateString; string firstWeekEndString;
    string secondWeekStartString; string secondWeekEndString;
    string firststartMonth; string firststartDay;
    string firstEndDay; string firstEndMonth;
    string secondStartMonth; string secondStartDay;
    string secondEndMonth; string secondEndDay;

    

    //시작 날짜 받아옴

    void Start()
    {
        string folderPath = (Application.platform == RuntimePlatform.Android ||
        Application.platform == RuntimePlatform.IPhonePlayer ?
        Application.persistentDataPath : Application.dataPath) + "/Save/";

		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

        ButtonData = ButtonMetaData.readOrCreate(folderPath);
        metaPlum = PlumMetaData.readOrCreate(folderPath);

        jsonPathTemplate = folderPath + "ButtonData.json";
        i=0;

        StartDateStr = PlayerPrefs.GetString("시작일");

        firstWeekEndString = DateTime.ParseExact(StartDateStr, "yyyy_MM_dd", null).AddDays(6).ToString("yyyy_MM_dd");
        secondWeekStartString = DateTime.ParseExact(StartDateStr, "yyyy_MM_dd", null).AddDays(7).ToString("yyyy_MM_dd");
        secondWeekEndString = DateTime.ParseExact(StartDateStr, "yyyy_MM_dd", null).AddDays(13).ToString("yyyy_MM_dd");

        firststartMonth = StartDateStr.Substring(5, 2);
        firststartDay = StartDateStr.Substring(8, 2);
        firstEndMonth = firstWeekEndString.Substring(5, 2);
        firstEndDay = firstWeekEndString.Substring(8, 2);
        secondStartMonth = secondWeekStartString.Substring(5, 2);
        secondStartDay = secondWeekStartString.Substring(8, 2);
        secondEndMonth = secondWeekEndString.Substring(5, 2);
        secondEndDay = secondWeekEndString.Substring(8, 2);

        FirstWeekDate.text = "첫째 주(" + firststartMonth+"월 "+firststartDay+"일 ~ "+firstEndMonth+"월 "+firstEndDay+"일)";
        SecondWeekDate.text = "둘째 주(" + secondStartMonth+"월 "+secondStartDay+"일 ~ "+secondEndMonth+"월 "+secondEndDay+"일)";
        
    }

    void Update()
    {
        DateTime EndDate = DateTime.Now.AddDays(1);
        int firstDay = 0;
        if(ButtonData.usedDays != 0) // i가 0이 아니면
        {
            i = ButtonData.usedDays-1; // 인덱스 맞춰주기 용
            //if(DateTime.Now.DayOfYear < DateTime.ParseExact(ButtonData.firstWeekDateStr[0], "yyyy_MM_dd", null).DayOfYear+7)
            //firstDay = DateTime.ParseExact(ButtonData.firstWeekDateStr[0], "yyyy_MM_dd", null).DayOfYear;
            firstDay = DateTime.ParseExact(StartDateStr, "yyyy_MM_dd", null).DayOfYear;
            if(DateTime.Now.DayOfYear == firstDay)
            {
                firstWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = true; // 사용 전 or 첫날엔 첫번째 버튼 활성화    
            }
        }

        if(ButtonData.firstWeekDateStr.Count == 0)
        {
            if(DateTime.Now.DayOfYear < DateTime.ParseExact(StartDateStr, "yyyy_MM_dd", null).DayOfYear+7)
            {
                firstWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = true; // 사용 전 or 첫날엔 첫번째 버튼 활성화
            }
            else if(ButtonData.secondWeekDateStr.Count == 0)
            {
                firstWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = false; // 사용 전 or 첫날엔 첫번째 버튼 활성화
                secondWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = true; // 사용 전 or 첫날엔 첫번째 버튼 활성화
            }
        }
        
        if(DateTime.Now.DayOfYear < DateTime.ParseExact(StartDateStr, "yyyy_MM_dd", null).DayOfYear+7)//if(DateTime.Now.DayOfYear < DateTime.ParseExact(ButtonData.firstWeekDateStr[0], "yyyy_MM_dd", null).DayOfYear+7) // 첫째주면
        {
            EndDate = new DateTime(DateTime.ParseExact(ButtonData.firstWeekDateStr[i], "yyyy_MM_dd", null).Year, DateTime.ParseExact(ButtonData.firstWeekDateStr[i], "yyyy_MM_dd", null).Month, DateTime.ParseExact(ButtonData.firstWeekDateStr[i], "yyyy_MM_dd", null).Day, 0, 0, 0).AddDays(1); // 쓴 날 자정
            firstWeekButtons.transform.GetChild(i).GetComponent<Button>().interactable = true; // 사용할 수 있었던 버튼 계속 활성화
            if(i+1 < firstWeekButtons.transform.childCount) // 아직 날짜 다 안 채움 마지막날 에러나면 등호 지울 것 
            {
                if(DateTime.Now > EndDate) // 자정 지나면
                {
                    if(ButtonData.firstWeekDateStr.Count == 1)
                    {
                        firstWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = false; // 사용할 수 있었던 버튼 비활성화        
                        firstWeekButtons.transform.GetChild(1).GetComponent<Button>().interactable = true; // 다음으로 사용해야 하는 버튼 활성화
                    }
                    else if (ButtonData.secondWeekDateStr.Count == metaPlum.frequency)
                    {
                        firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count-1).GetComponent<Button>().interactable = false; // 사용할 수 있었던 버튼 비활성화
                    }
                    else
                    {
                        firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count-1).GetComponent<Button>().interactable = false; // 사용할 수 있었던 버튼 비활성화
                        firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count).GetComponent<Button>().interactable = true; // 다음으로 사용해야 하는 버튼 활성화    
                    }
                }
                else
                {
                    if(ButtonData.firstWeekDateStr.Count > 1)
                    {
                        firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count-2).GetComponent<Button>().interactable = false; // 사용할 수 있었던 버튼 계속 활성화
                    }
                    firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count-1).GetComponent<Button>().interactable = true; // 사용할 수 있었던 버튼 계속 활성화
                }
            }
            else
            {
                if(DateTime.Now > EndDate) // 자정 지나면
                {
                    firstWeekButtons.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
        }
        else if(DateTime.Now.DayOfYear < DateTime.ParseExact(StartDateStr, "yyyy_MM_dd", null).DayOfYear+13)// 2주차
        {
            if(ButtonData.firstWeekDateStr.Count != 0)
            {
                firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count-1).GetComponent<Button>().interactable = false; // 일단 첫째주 마지막 버튼 비활성화
            }
            if(ButtonData.firstWeekDateStr.Count < metaPlum.frequency) // 첫째주에 덜 썻을 경우
            {
                firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count).GetComponent<Button>().interactable = false; // 일단 첫째주 마지막 버튼 비활성화
                // if(ButtonData.firstWeekDateStr.Count != 0)
                // {
                //     firstWeekButtons.transform.GetChild(ButtonData.firstWeekDateStr.Count-1).GetComponent<Button>().interactable = false; // 일단 첫째주 마지막 버튼 비활성화
                // }
            }
            if(ButtonData.secondWeekDateStr.Count == 0 || ButtonData.secondWeekDateStr.Count == 1) // 둘째 주 처음 시작이면
            {
                secondWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = true; // 둘째주 첫번째 활성화
            }
            
            EndDate = new DateTime(DateTime.ParseExact(ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1], "yyyy_MM_dd", null).Year, DateTime.ParseExact(ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1], "yyyy_MM_dd", null).Month, DateTime.ParseExact(ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1], "yyyy_MM_dd", null).Day, 0, 0, 0).AddDays(1); // 쓴 날 자정
            // EndDate = new DateTime(DateTime.ParseExact(ButtonData.secondWeekDateStr[i - frequency], "yyyy_MM_dd", null).Year, DateTime.ParseExact(ButtonData.secondWeekDateStr[i - frequency], "yyyy_MM_dd", null).Month, DateTime.ParseExact(ButtonData.secondWeekDateStr[i - frequency], "yyyy_MM_dd", null).Day+1, 0, 0, 0); // 쓴 날 자정
            if(DateTime.Now > EndDate) // 자정 지나면
            {   
                if(ButtonData.secondWeekDateStr.Count == 1)
                {
                    secondWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    secondWeekButtons.transform.GetChild(1).GetComponent<Button>().interactable = true;
                }
                else if (ButtonData.secondWeekDateStr.Count == metaPlum.frequency)
                {
                    secondWeekButtons.transform.GetChild(ButtonData.secondWeekDateStr.Count-1).GetComponent<Button>().interactable = false;
                }
                else
                {
                    secondWeekButtons.transform.GetChild(ButtonData.secondWeekDateStr.Count-1).GetComponent<Button>().interactable = false;
                    secondWeekButtons.transform.GetChild(ButtonData.secondWeekDateStr.Count).GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                secondWeekButtons.transform.GetChild(ButtonData.secondWeekDateStr.Count-1).GetComponent<Button>().interactable = true;
            }
        }

        else // 둘째주까지 끝나면
        {
            secondWeekButtons.transform.GetChild(ButtonData.secondWeekDateStr.Count).GetComponent<Button>().interactable = false;
            secondWeekButtons.transform.GetChild(ButtonData.secondWeekDateStr.Count-1).GetComponent<Button>().interactable = false;
            secondWeekButtons.transform.GetChild(ButtonData.secondWeekDateStr.Count+1).GetComponent<Button>().interactable = false;
        }


    }


    public void ButtonClicked()
    {
        // DateTime.Now.ToString("yyyy-MM-dd")
        // 클릭할 때마다 List에 클릭한 시점 날짜 추가
        // 일단 두번째 주로 안 넘어가도 되는지 확인
        // 첫번째 List 길이가 사용횟수랑 같아지면 두번째 List로 넘김
        string clickedDate; 
        string startDateString = PlayerPrefs.GetString("시작일");
        int freqPerWeek = metaPlum.frequency;
        clickedDate = DateTime.Now.ToString("yyyy_MM_dd");

        if(ButtonData.usedDays == 0 && DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+7) // 한 번도 안 썼으면
        {   
            ButtonData.usedDays++;
            ButtonData.firstWeekDateStr.Add(clickedDate); // 첫날 추가
            ButtonData.startDate = DateTime.Now; // 첫주에만 세이브해줘도 됨
            ButtonData.Save();
        }

        else if(DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+7) // && 시작일로부터 일주일 이내임
        {
//            foreach(String str in ButtonData.firstWeekDateStr)
            if(DateTime.ParseExact(ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1], "yyyy_MM_dd", null).DayOfYear < DateTime.Now.DayOfYear && ButtonData.firstWeekDateStr.Count < metaPlum.frequency)
            {
                //if(DateTime.ParseExact(str, "yyyy_MM_dd", null).DayOfYear < DateTime.Now.DayOfYear && str != DateTime.Now.ToString("yyyy_MM_dd")) // 날짜가 더 뒤이고 중복되는게 없으면 --> 중복 체크가 제대로 안 됨
                //{
                ButtonData.firstWeekDateStr.Add(clickedDate);
                ButtonData.usedDays++;
                ButtonData.Save();
                PlumDataCreator(metaPlum);
                //    break;
                //} // for문 돌릴 때 처음에 작은 거 부터 하는데 날짜 바뀌면 당연히... 마지막거만 봐도 되지 않나?
            }
            
        }
        else if(DateTime.Now.DayOfYear < DateTime.ParseExact(startDateString, "yyyy_MM_dd", null).DayOfYear+13)// 둘째주 이내
        {
            clickedDate = DateTime.Now.ToString("yyyy_MM_dd");
            if(ButtonData.secondWeekDateStr.Count == 0)
            {
                ButtonData.usedDays++;
                ButtonData.secondWeekDateStr.Add(clickedDate);
                ButtonData.Save();
            }
            if(DateTime.ParseExact(ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1], "yyyy_MM_dd", null).DayOfYear < DateTime.Now.DayOfYear && ButtonData.secondWeekDateStr.Count < metaPlum.frequency)
            {
                ButtonData.secondWeekDateStr.Add(clickedDate);
                ButtonData.usedDays++;
                ButtonData.Save();
                PlumDataCreator(metaPlum);
                //break;
            }
        }
        SceneManager.LoadScene("DailyUsageScene");
    }

    // private void ButtonEnabler() // 버튼 활성화 및 비활성화
    // {
    //     // 현재 날짜, 배열의 count랑 주당 사용횟수 받아서
    //     // count째의 버튼을 활성화하고
    //     // 현재날짜랑 비교해서 
    //     //사용 횟수가 홀수일 때: 첫째주 n-1회까지 다 쓰면 그 다음은 n회랑 2주차 1회
    //     //사용 횟수가 짝수일 때: 첫째주 다 쓰면 둘째주 열림
    //     if(ButtonData.firstWeekDateStr.Count < metaPlum.frequency) // 첫째주
    //     {
    //         this.gameObject.transform.GetChild(ButtonData.firstWeekDateStr.Count).gameObject.GetComponent<Button>().interactable = true;// getchild buttondata.firstweekdatestr.count 첫째주
    //     }
    //     else if((DateTime.Now-DateTime.Parse(ButtonData.firstWeekDateStr[0])).Days >= 7) // 둘째주 ButtonData.firstWeekDateStr.Count == metaPlum.frequency 
    //     {
    //         this.gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;// getchild 둘째주 첫번째
    //     }
    //     else if(ButtonData.secondWeekDateStr.Count > 0)
    //     {  
    //         this.gameObject.transform.GetChild(ButtonData.secondWeekDateStr.Count).gameObject.GetComponent<Button>().interactable = true; // getchild 둘째주 buttondata.secondweekdatestr.count
    //     }
    // }

    public void PlumDataCreator(PlumMetaData plumMeta)
    {
        string folderPath = (Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer ?
			Application.persistentDataPath : Application.dataPath) + "/Save/";

        DateTime startDay = plumMeta.startDate;

        PlumObject[] pobjArr = new PlumObject[plumMeta.PlumPerDay];
        string toJson = JsonHelper.ToJson<PlumObject>(pobjArr, prettyPrint: true);

        // String filePath = String.Format("{0}PlumData_{1}.json", folderPath, DateTime.Now.ToString("yyyy_MM_dd"));
        // File.WriteAllText(filePath, toJson);
    }
}
