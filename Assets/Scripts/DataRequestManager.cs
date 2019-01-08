using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml;

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
    private string pm10Grade1H;
    private string pm25Grade1H;
    private string pm10Value;
    private string pm25Value;

    private int count;

    void Start()
    {
        count = 0;
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://openapi.airkorea.or.kr/openapi/services/rest/ArpltnInforInqireSvc/getCtprvnRltmMesureDnsty?sidoName=경기&pageNo=1&numOfRows=10&ServiceKey=zLp2tLOTP9bJF2vIfVzdf%2Bdj0loKSEqd2sO8mwCgIaEVplrCzarGyK%2FPA%2BNG8BcIxAGuR4z9aeVFiwmERpMawg%3D%3D&ver=1.3&_returnType=json");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            count++;
            if(count < 5)
                StartCoroutine(GetText());
            else
            {
                string title = "알림";
                string message = "인터넷 연결이 불안정합니다.";
                AlertViewController.Show(title, message);
            }
        }
        else
        {
            count = 0;
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            ParsingJson(www.downloadHandler.text);
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    private void ParsingJson(string receiveData)
    {
        Form form = JsonUtility.FromJson<Form>(receiveData);
        Debug.Log(form.list[2].stationName);

        pm10Grade1H = form.list[2].pm10Grade1h;
        pm25Grade1H = form.list[2].pm25Grade1h;
        pm10Value = form.list[2].pm10Value;
        pm25Value = form.list[2].pm25Value;
    }
}
