using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoButtonManager : MonoBehaviour
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

        firstWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = true;
    }

    void Update()
    {
        firstWeekButtons.transform.GetChild(0).GetComponent<Button>().interactable = true; // 사용 전 or 첫날엔 첫번째 버튼 활성화    
    }


    public void ButtonClicked()
    {
        // DateTime.Now.ToString("yyyy-MM-dd")
        // 클릭할 때마다 List에 클릭한 시점 날짜 추가
        // 일단 두번째 주로 안 넘어가도 되는지 확인
        // 첫번째 List 길이가 사용횟수랑 같아지면 두번째 List로 넘김
        string clickedDate; 
        int freqPerWeek = metaPlum.frequency;
        clickedDate = DateTime.Now.ToString("yyyy_MM_dd");

        if(ButtonData.usedDays == 0) // 한 번도 안 썼으면
        {   
            ButtonData.usedDays++;
            ButtonData.firstWeekDateStr.Add(clickedDate); // 첫날 추가
            ButtonData.startDate = DateTime.Now; // 첫주에만 세이브해줘도 됨
            ButtonData.Save();
        }

        else if(DateTime.Now.DayOfYear < DateTime.ParseExact(ButtonData.firstWeekDateStr[0], "yyyy_MM_dd", null).DayOfYear+7) // && 시작일로부터 일주일 이내임
        {
//            foreach(String str in ButtonData.firstWeekDateStr)
            if(DateTime.ParseExact(ButtonData.firstWeekDateStr[ButtonData.firstWeekDateStr.Count-1], "yyyy_MM_dd", null).DayOfYear < DateTime.Now.DayOfYear)
            {
                //Debug.Log(str);
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
        else
        {
            clickedDate = DateTime.Now.ToString("yyyy_MM_dd");
            if(ButtonData.secondWeekDateStr.Count == 0)
            {
                ButtonData.usedDays++;
                ButtonData.secondWeekDateStr.Add(clickedDate);
                ButtonData.Save();
            }
            if(DateTime.ParseExact(ButtonData.secondWeekDateStr[ButtonData.secondWeekDateStr.Count-1], "yyyy_MM_dd", null).DayOfYear < DateTime.Now.DayOfYear)
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

    private void ButtonEnabler() // 버튼 활성화 및 비활성화
    {
        // 현재 날짜, 배열의 count랑 주당 사용횟수 받아서
        // count째의 버튼을 활성화하고
        // 현재날짜랑 비교해서 
        //사용 횟수가 홀수일 때: 첫째주 n-1회까지 다 쓰면 그 다음은 n회랑 2주차 1회
        //사용 횟수가 짝수일 때: 첫째주 다 쓰면 둘째주 열림
        if(ButtonData.firstWeekDateStr.Count < metaPlum.frequency) // 첫째주
        {
            this.gameObject.transform.GetChild(ButtonData.firstWeekDateStr.Count).gameObject.GetComponent<Button>().interactable = true;// getchild buttondata.firstweekdatestr.count 첫째주
        }
        else if((DateTime.Now-DateTime.Parse(ButtonData.firstWeekDateStr[0])).Days >= 7) // 둘째주 ButtonData.firstWeekDateStr.Count == metaPlum.frequency 
        {
            this.gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;// getchild 둘째주 첫번째
        }
        else if(ButtonData.secondWeekDateStr.Count > 0)
        {  
            this.gameObject.transform.GetChild(ButtonData.secondWeekDateStr.Count).gameObject.GetComponent<Button>().interactable = true; // getchild 둘째주 buttondata.secondweekdatestr.count
        }
    }

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

    private void ButtonDisabler()
    {
        // 함수를 하나 따로 만들어서 리스트날짜마다 터치한 날 11시 59분이 지나면 대응하는 아이콘 비활성화하고 그 다음거 활성화하도록?
        // 사용횟수 홀짝에 따라 다름
        // 나머지 다 꺼줘야 함
        // 자정 지나고 나면 꺼야 함
    }
    //buttondata의 각 배열 count를 받아서... 그거 보고 언제거를 활성화시킬 지 (child 번호로)
    //매번 앱을 실행할 때마다 첫째날로부터 일주일이 지났는지 확인하고
    // - 일주일이 지났고 2주 1번째일 경우: 2주 1번째로 넘겨버린다
    // - 일주일이 지나지 않았을 경우: 그 다음 버튼을 활성화한다


}
