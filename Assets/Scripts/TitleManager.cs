using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject Title;
    public GameObject ep1;
    public GameObject ep2;
    public GameObject ep3;

    public float time = 5f;

    private void Start()
    {
        ep1.SetActive(false);
        ep2.SetActive(false);
        ep3.SetActive(false);

        StartCoroutine(ShowTitle());
    }

    private IEnumerator ShowTitle()
    {
        yield return new WaitForSeconds(time);
        ep1.SetActive(true);
        yield return new WaitForSeconds(time);
        ep2.SetActive(true);
        yield return new WaitForSeconds(time);
        ep3.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}