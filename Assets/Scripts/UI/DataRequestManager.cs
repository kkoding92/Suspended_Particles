using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class Form
{
    public List<Data> list;
}

[System.Serializable]
public class Data
{
    public string _returnType;
    public string coGrade;
    public string coValue;
    public string dataTerm;
    public string dataTime;
    public string khaiGrade;
    public string khaiValue;
    public string mangName;
    public string no2Grade;
    public string no2Value;
    public string numOfRows;
    public string o3Grade;
    public string o3Value;
    public string pageNo;
    public string pm10Grade;
    public string pm10Grade1h;
    public string pm10Value;
    public string pm10Value24;
    public string pm25Grade;
    public string pm25Grade1h;
    public string pm25Value;
    public string pm25Value24;
    public string resultCode;
    public string resultMsg;
    public string rnum;
    public string serviceKey;
    public string sidoName;
    public string so2Grade;
    public string so2Value;
    public string stationCode;
    public string stationName;
    public string totalCount;
    public string ver;
}

public class DataRequestManager : MonoBehaviour
{
    public GameManager gm;
    public GameObject inforPanel;
    public GameObject dataPanel;
    public Text inforText;
    public Text dataText1;
    public Text dataText2;

    private string pm10Grade1H;
    private string pm25Grade1H;
    private string pm10Value;
    private string pm25Value;

    private int count;
    private int clickCount = 0;
    private bool isSetData;

    void Start()
    {
        inforPanel.SetActive(true);
        dataPanel.SetActive(false);
        isSetData = false;
        clickCount = 0;
        count = 0;
        inforText.text = "외부 미세먼지 정보";
        gm.gameLevel = GameLevel.Default;
        StartCoroutine(RequestData());
    }

    IEnumerator RequestData()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://openapi.airkorea.or.kr/openapi/services/rest/ArpltnInforInqireSvc/getCtprvnRltmMesureDnsty?sidoName=경기&pageNo=1&numOfRows=10&ServiceKey=zLp2tLOTP9bJF2vIfVzdf%2Bdj0loKSEqd2sO8mwCgIaEVplrCzarGyK%2FPA%2BNG8BcIxAGuR4z9aeVFiwmERpMawg%3D%3D&ver=1.3&_returnType=json");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            count++;
            if (count < 5)
                StartCoroutine(RequestData());
            else
            {
                isSetData = false;
            }
        }
        else
        {
            count = 0;
            isSetData = true;
            Debug.Log(www.downloadHandler.text);
            ParsingJson(www.downloadHandler.text);
        }
    }

    private void ParsingJson(string receiveData)
    {
        Form form = JsonUtility.FromJson<Form>(receiveData);

        if (form.list[2].pm10Grade1h.Equals("1"))
        {
            pm10Grade1H = "좋음";
            gm.gameLevel = GameLevel.Easy;
        }
        else if (form.list[2].pm10Grade1h.Equals("2"))
        {
            pm10Grade1H = "보통";
            gm.gameLevel = GameLevel.Normal;
        }
        else if (form.list[2].pm10Grade1h.Equals("3"))
        {
            pm10Grade1H = "나쁨";
            gm.gameLevel = GameLevel.Hard;
        }
        else if (form.list[2].pm10Grade1h.Equals("4"))
        {
            pm10Grade1H = "매우나쁨";
            gm.gameLevel = GameLevel.Hard;
        }

        if (form.list[2].pm25Grade1h.Equals("1"))
            pm25Grade1H = "좋음";
        else if (form.list[2].pm25Grade1h.Equals("2"))
            pm25Grade1H = "보통";
        else if (form.list[2].pm25Grade1h.Equals("3"))
            pm25Grade1H = "나쁨";
        else if (form.list[2].pm25Grade1h.Equals("4"))
            pm25Grade1H = "매우나쁨";

        pm10Value = form.list[2].pm10Value;
        pm25Value = form.list[2].pm25Value;
    }

    public void OnMouseDown()
    {
        if(clickCount == 0)
        {
            inforPanel.SetActive(false);
            dataPanel.SetActive(true);

            dataText1.text = "미세먼지 등급";
            dataText2.text = pm10Grade1H;

            clickCount++;
        }
        else if (clickCount == 1)
        {
            dataText1.text = "초미세먼지 등급";
            dataText2.text = pm25Grade1H;
            clickCount++;
        }
        else if (clickCount == 2)
        {
            dataText1.text = "미세먼지 수치";
            dataText2.text = pm10Value;
            clickCount++;
        }
        else if(clickCount == 3)
        {
            dataText1.text = "초미세먼지 수치";
            dataText2.text = pm25Value;
            clickCount++;
        }
        else if (clickCount == 4)
        {
            inforPanel.SetActive(true);
            dataPanel.SetActive(false);
            clickCount = 0;
        }
    }
}
