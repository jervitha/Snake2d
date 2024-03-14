using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private string mainMenu;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text score2;
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private GameObject snakewinDisplay;
    [SerializeField] private TextMeshProUGUI playerWintext;
  
    public Snake snake;

    private void Awake()
    {
        instance = this;
    }
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }
    public void PauseUnpause()
    {
        if(pauseScreen.activeSelf==false)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        SoundManager.Instance.PlaySound(Sounds.PlayButtonClick);

        SceneManager.LoadScene(mainMenu);
    }
    public void QuitGame()
    {
        SoundManager.Instance.PlaySound(Sounds.PlayButtonClick);
        Application.Quit();
    }
    public void Restart()
    {
        SoundManager.Instance.PlaySound(Sounds.PlayButtonClick);
        SceneManager.LoadScene(1);

        Time.timeScale = 1f;
    }
    public void UpdateScore(int total,SnakePlayer snake)

    {
        if (snake == SnakePlayer.Snake1)
        {
            score.text = total.ToString();
        }
      else if (snake == SnakePlayer.Snake2)
        {
            score2.text = total.ToString();
        }


    }

    public void ShowgameOver()
    {
        SoundManager.Instance.PlaySound(Sounds.GameOver);
        gameoverScreen.SetActive(true);
    }


    public void SnakeWin(SnakePlayer snake,int maxScore)
    {
        SoundManager.Instance.PlayMusic(Sounds.SnakeWin);
      
        snakewinDisplay.SetActive(true);
        if(snake==SnakePlayer.Snake1)

        {
            playerWintext.text = "SNAKE1 WINS";
            Time.timeScale = 0f;

        }
        else if(snake==SnakePlayer.Snake2)
        {
            
            playerWintext.text = "SNAKE2 WINS";
            Time.timeScale = 0f;
        }

    }



}
