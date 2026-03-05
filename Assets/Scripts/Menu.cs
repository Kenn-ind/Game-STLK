using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    //public void play(string namescene)
    //{
    //    SceneManager.LoadScene(namescene);
    //}

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject StartFade;

    public void Button(string namescene)
    {
        SceneManager.LoadScene(namescene);
        Time.timeScale = 1;
    }

    public void exit() 
    { 
        Application.Quit();
        Debug.Log("Selesai");
    }

    public void pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void resume()
    {
        PauseMenu?.SetActive(false);
        Time.timeScale = 1;
    }

    public void restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        Time.timeScale = 1;
    }

    public void mulai()
    {
        StartFade.SetActive(true);
        StartFade.GetComponent<Animator>().SetBool("StartFade", true);

        StartCoroutine(LoadSceneAfterFade());
    }

    IEnumerator LoadSceneAfterFade()
    {
        yield return new WaitForSeconds(3.5f); // sesuaikan dengan durasi animasi
        SceneManager.LoadScene("prolog");
    }

}
