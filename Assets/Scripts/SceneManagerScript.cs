using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
     public void PlayGame()
    {
        Debug.Log("Start...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("Exit...");
        Application.Quit();
    }

    public void EndMenu()
    {
        Debug.Log("Game Over...");
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("EndScene");
    }
    public void MainMenu()
    {
        Debug.Log("Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }
}
