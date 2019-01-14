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
    public Image misePanel;
    public Image choPanel;
    public Sprite[] miseSprite;
    public Sprite[] choSprite;
    public Text miseTxt;
    public Text choTxt;

    private string pm10Grade1H;
    private string pm25Grade1H;
    private string pm10Value;
    private string pm25Value;

    private int count;
    private bool isSetData;

    private void Start()
    {
        inforPanel.SetActive(false);
        isSetData = false;
        count = 0;
        gm.gameLevel = GameLevel.Default;
        StartCoroutine(RequestData());
    }

    private IEnumerator RequestData()
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
            gm.gameLevel = GameLevel.Easy;
            misePanel.sprite = miseSprite[0];
        }
        else if (form.list[2].pm10Grade1h.Equals("2"))
        {
            gm.gameLevel = GameLevel.Normal;
            misePanel.sprite = miseSprite[1];
        }
        else if (form.list[2].pm10Grade1h.Equals("3"))
        {
            gm.gameLevel = GameLevel.Hard;
            misePanel.sprite = miseSprite[2];
        }
        else if (form.list[2].pm10Grade1h.Equals("4"))
        {
            gm.gameLevel = GameLevel.Hard;
            misePanel.sprite = miseSprite[3];
        }

        if (form.list[2].pm25Grade1h.Equals("1"))
        {
            choPanel.sprite = choSprite[0];
        }
        else if (form.list[2].pm25Grade1h.Equals("2"))
        {
            choPanel.sprite = choSprite[1];
        }
        else if (form.list[2].pm25Grade1h.Equals("3"))
        {
            choPanel.sprite = choSprite[2];
        }
        else if (form.list[2].pm25Grade1h.Equals("4"))
        {
            choPanel.sprite = choSprite[3];
        }

        pm10Value = form.list[2].pm10Value;
        pm25Value = form.list[2].pm25Value;

        miseTxt.text = pm10Value;
        choTxt.text = pm25Value;
    }

    public void OnMouseDown()
    {
        inforPanel.SetActive(true);

        StartCoroutine(OffPanel());
    }

    private IEnumerator OffPanel()
    {
        yield return new WaitForSeconds(5f);
        inforPanel.SetActive(false);

        yield return StartCoroutine(RequestData());
    }
}