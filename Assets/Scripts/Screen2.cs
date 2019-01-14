using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen2 : MonoBehaviour
{
    public GameObject screen2;

    private void Start()
    {
        screen2.SetActive(false);
    }

    private void OnMouseDown()
    {
        screen2.SetActive(true);
        StartCoroutine(OffPanel());
    }

    private IEnumerator OffPanel()
    {
        yield return new WaitForSeconds(5f);
        screen2.SetActive(false);
    }
}