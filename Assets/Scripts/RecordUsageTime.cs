using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine;
using Assets.SimpleAndroidNotifications;
using System.Linq;

// 사용 시간을 기록
namespace FinishPlum
{
    public class RecordUsageTime : MonoBehaviour
    {
        enum Mode { None, Running, Paused };
        private Mode TimerMode = Mode.None;

        private GameObject timeTracker;
        Coroutine timeCoroutine = null;
        private List<bool> CoroutineFlagList = new List<bool>();

        public static DateTime StartTime;
        public static DateTime EndTime;
        public static DateTime NowDate;
        public static TimeSpan PauseResTime;

        static TimeSpan timeCal;

        public static bool isUsingPlum = false;

        public int timeCalDay; // = timeCal.Days;
        int timeCalMin;
        public List<GameObject> firstPlumArray = new List<GameObject>();
        public List<GameObject> secondPlumArray = new List<GameObject>();

        public GameObject plumManager;

        NotificationExample newNoti = new NotificationExample();

        public event EventHandler FinishTimerEvnetHandler;

        public DateTime nowDate;
        public DateTime now;
        public DateTime quitTime;

        public float timeGap;

        void Start(){
            Init_Notification();
        }

        void OnApplicationQuit()
        {
            quitTime = DateTime.Now;
            timeGap = (float)((now - quitTime).TotalSeconds);
            PlayerPrefs.SetFloat("사용한시간", timeGap);
        }

        public void Init_Notification()
	    {
            #if UNITY_IOS
			UnityEngine.iOS.NotificationServices.RegisterForNotifications(
				UnityEngine.iOS.NotificationType.Alert |
				UnityEngine.iOS.NotificationType.Badge |
				UnityEngine.iOS.NotificationType.Sound
            );
            #endif
        }
        public void createTimerWSelectedPlumQue(){
        
            PlumManager pm = plumManager.GetComponent<PlumManager>();
            int plumNum = pm.getSelectedPlumNum();
            Init_Notification();
            settingTimerWPlumNum(plumNum, pm.metaData.SecPerPlum);
        }
        public void settingTimerWPlumNum(int plumNum, int plumTime){
            now = DateTime.Now;
            NowDate = System.Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss"));
            timeCal = NowDate - StartTime;

            // 총 시간
            // int timerTime = plumNum * 10;
            int timerTime = plumNum * plumTime;

            StartTime = DateTime.Now;
            EndTime = StartTime.AddSeconds(timerTime);

            TimerMode = Mode.Running;

            // 사용중으로 변경
            PlumManager pm = plumManager.GetComponent<PlumManager>();
			pm.changePlumStatusInDay1OrDay2(plumNum: 1, fromPlumStatus: 2, toPlumStatus: 3);
            pm.addPlumUsingTimeLogInDay1OrDay(StartTime, plumTime);

            Init_Notification();
            // 안드로이드/아이폰 알람 추가
            CreateLocalNotification(timerTime);

            int coroutinIdx = CoroutineFlagList.Count;
            CoroutineFlagList.Add(true);
            StartCoroutine(GetTimerCoroutine(coroutinIdx, timerTime));

            // 플럼 개개인의 시간
            StartCoroutine(GetPlumTimerCoroutine(coroutinIdx, plumNum, plumTime));
        }

        public void SettingTimerWSecond(int plumNum, int plumTime, int timerTime)
        {
            Debug.Log("settingTimerWSecond");
            NowDate = System.Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss"));
            timeCal = NowDate - StartTime;

            // 총 시간
            // int timerTime = plumNum * 10;
            
            StartTime = DateTime.Now;
            EndTime = StartTime.AddSeconds(timerTime);

            TimerMode = Mode.Running;

            // 사용중으로 변경
            PlumManager pm = plumManager.GetComponent<PlumManager>();
            pm.changePlumStatusInDay1OrDay2(plumNum: plumNum, fromPlumStatus: 1, toPlumStatus: 3);
            
            Init_Notification();

            // 안드로이드/아이폰 알람 추가
            CreateLocalNotification(timerTime);

            int coroutinIdx = CoroutineFlagList.Count;
            CoroutineFlagList.Add(true);
            StartCoroutine(GetTimerCoroutine(coroutinIdx, timerTime));

            // 플럼 개개인의 시간
            StartCoroutine(GetPlumTimerCoroutine(coroutinIdx, plumNum, plumTime));
        }

        public void SettingTimerWPlumList(List<PlumObject> plumList)
        {
            DateTime nowTime = DateTime.Now;

            plumList.Sort((PlumObject x, PlumObject y) => DateTime.Parse(x.usingEndDate) > DateTime.Parse(y.usingEndDate) ? 1 : -1);
            TimerMode = Mode.Running;

            List<int> plumTimeList = new List<int>();
            foreach (PlumObject pob in plumList)
            {
                DateTime sTime = DateTime.Parse(pob.usingStartDate);
                if (nowTime > sTime)
                    sTime = nowTime;
                int sec = (int)(DateTime.Parse(pob.usingEndDate) - sTime).TotalSeconds;
                plumTimeList.Add(sec);
            }

            double totalTime = plumTimeList.Sum();

            int coroutinIdx = CoroutineFlagList.Count;
            CoroutineFlagList.Add(true);
            StartCoroutine(GetTimerCoroutine(coroutinIdx, (int) totalTime));

            // 플럼 개개인의 시간
            StartCoroutine(GetPlumTimerCoroutine(coroutinIdx, plumTimeList.ToArray()));
        }

        public void Pause()
        {
            if (TimerMode == Mode.Running)
            {
                Init_Notification();
                TimerMode = Mode.Paused;
                CancelLocalNotification();
                PauseResTime = EndTime - DateTime.Now;

                CancelLocalNotification();
                int lastCoroutineIdx = CoroutineFlagList.Count - 1;
                CoroutineFlagList[lastCoroutineIdx] = false;

            }
        }

        public void Stop()
        {
            if (TimerMode == Mode.Running)
            {
                Init_Notification();
                TimerMode = Mode.None;
                CancelLocalNotification();
                int lastCoroutineIdx = CoroutineFlagList.Count - 1;
                CoroutineFlagList[lastCoroutineIdx] = false;

                PlumManager pm = plumManager.GetComponent<PlumManager>();
                pm.changePlumStatusStopDay1OrDay2();
            }
        }


        public void Restart()
        {
             if(TimerMode == Mode.Paused)
            {
                TimerMode = Mode.Running;
                //PauseResTime.TotalSeconds;
            }
        }

        
        private void CreateLocalNotification(int seconds)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
				newNoti.SchodueCustomWSeconds(seconds);
                //TimeSpan.FromSeconds(seconds);
                // 로컬에 언제 끝나는지 저장해두기
			}
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                #if UNITY_IOS
                    UnityEngine.iOS.LocalNotification noti = new UnityEngine.iOS.LocalNotification();
                    noti.alertAction = "PLUM";
                    noti.alertBody = "자두 사용 시간이 완료되었어요!";
                    noti.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
                    noti.applicationIconBadgeNumber = 1;
                    noti.fireDate = System.DateTime.Now.AddSeconds(seconds);
                    UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(noti);
                #endif
            }
        }

        private void CancelLocalNotification()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                newNoti.CancelAll();
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                #if UNITY_IOS
                    UnityEngine.iOS.LocalNotification noti = new UnityEngine.iOS.LocalNotification();
                    noti.fireDate = System.DateTime.Now;
                    noti.applicationIconBadgeNumber = -1;
                    noti.hasAction = false;
                    UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(noti);
                    
                    UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
                    UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
                #endif
            }
        }

        private IEnumerator GetTimerCoroutine(int coroutinIdx, int time)
        {
            WaitForSeconds waitSec = new WaitForSeconds(time);
            yield return waitSec;
            // 다 끝났으니까 삭제하기
            if (CoroutineFlagList[coroutinIdx])
            {
                PlumManager pm = plumManager.GetComponent<PlumManager>();
                pm.changePlumStatusInDay1OrDay2(plumNum: 1, fromPlumStatus: 3, toPlumStatus: 0);

                if(FinishTimerEvnetHandler != null)
                    FinishTimerEvnetHandler(this, null);
            }
		}

        private IEnumerator GetPlumTimerCoroutine(int coroutinIdx, int plumNum, int plumTime){
            for (int i = 0; i < plumNum; i++)
            {
                WaitForSeconds waitSec = new WaitForSeconds(plumTime);
                yield return waitSec;

                if (!CoroutineFlagList[coroutinIdx]) break;

                PlumManager pm = plumManager.GetComponent<PlumManager>();
                pm.changePlumStatusInDay1OrDay2(plumNum: 1, fromPlumStatus: 3, toPlumStatus: 0);
                pm.changePlumStatusInDay1OrDay2(plumNum: 1, fromPlumStatus: 2, toPlumStatus: 3);
            }
            yield return System.Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss"));
        }

        private IEnumerator GetPlumTimerCoroutine(int coroutinIdx, int[] plumTime)
        {
            foreach(int pTime in plumTime)
            {
                WaitForSeconds waitSec = new WaitForSeconds(pTime);
                yield return waitSec;

                if (!CoroutineFlagList[coroutinIdx]) break;

                PlumManager pm = plumManager.GetComponent<PlumManager>();
                pm.changePlumStatusInDay1OrDay2(plumNum: 1, fromPlumStatus: 3, toPlumStatus: 0);
                pm.changePlumStatusInDay1OrDay2(plumNum: 1, fromPlumStatus: 2, toPlumStatus: 3);
            }
            yield return System.Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss"));
        }
    }
}