using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{

    public GameObject winkcow;
    public GameObject nowinkcow;
    public AudioSource wink;

    void Start()
    {

        StartCoroutine(SplashAnimation());

    }

    IEnumerator SplashAnimation()
    {
        nowinkcow.SetActive(true);
        winkcow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        wink.Play();
        nowinkcow.SetActive(false);
        winkcow.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        nowinkcow.SetActive(true);
        winkcow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("InGame");

    }




}
