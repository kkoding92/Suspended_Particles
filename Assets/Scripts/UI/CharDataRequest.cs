using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharDataRequest : MonoBehaviour
{
    public GameManager gm;
    public GameObject inforPanel;
    public GameObject dataPanel;
    public Text dataText1;
    public Text dataText2;

    private int clickCount = 0;
    private bool isClicked;

    void Start()
    {
        inforPanel.SetActive(false);
        dataPanel.SetActive(true);

        dataText1.text = "내부 미세먼지 수치";
        dataText2.text = gm.FineDustLevel.ToString();

        clickCount = 0;
        isClicked = false;
        StartCoroutine(CheckChangeValue());
    }

    private void OnMouseDown()
    {
        isClicked = true;

        if (clickCount == 0)
        {
            dataText1.text = "획득한 돈";
            dataText2.text = gm.Coin.ToString();
            clickCount++;
        }
        else if (clickCount == 1)
        {
            dataText1.text = "내부 미세먼지 수치";
            dataText2.text = gm.FineDustLevel.ToString();
            clickCount=0;
        }
    }

    IEnumerator CheckChangeValue()
    {
        while (true)
        {
            if (!isClicked)
                dataText2.text = gm.FineDustLevel.ToString();
            else
            {
                if (clickCount == 0)
                    dataText2.text = gm.Coin.ToString();
                else if (clickCount == 1)
                    dataText2.text = gm.FineDustLevel.ToString();
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
