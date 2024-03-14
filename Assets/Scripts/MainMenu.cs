using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevel;
    [SerializeField] private string twoPlayer;
    
   public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void TwoplayerMode()
    {
        SceneManager.LoadScene(twoPlayer);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
