using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame ()
    {
        StartCoroutine(LoadSceneWithDelay());
    }

    public void QuitGame () 
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
