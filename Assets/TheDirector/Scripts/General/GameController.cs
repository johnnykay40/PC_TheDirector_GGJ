using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    
    public ScoreController scoreController;
    public static event Action<bool> OnStartGame;
    public static event Action<bool> OnPauseGame;
    public Image finishGameImg;
    public Sprite winSprite;
    public Sprite loseSprite;
    public GameObject finalUI;
    public GameObject startUI;


    public bool gamePause;

    public void StartGame()
    {
        //Boton UI
        
        gamePause = false;
        OnStartGame?.Invoke(true);
    }

    public void PauseGame()
    {
        gamePause = !gamePause;
        OnPauseGame?.Invoke(gamePause);
    }

    public void Win()
    {
        PauseGame();
        finalUI.SetActive(true);
        //finishGameImg.sprite = winSprite;
        Debug.Log("Ganaste");
    }

    public void GameOver()
    {
        PauseGame();
        finalUI.SetActive(true);
        //finishGameImg.sprite = loseSprite;
        Debug.Log("Perdiste");
    }

    public void PlayGame()
    {
        PauseGame();
        startUI.SetActive(true);
        //finishGameImg.sprite = loseSprite;
        Debug.Log("Perdiste");
    }

    private void ProcessTime()
    {
        GameOver();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You Left");
    }

    public void ResetScene()
    {
        finalUI.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
    
     scoreController.OnFinalScore += Win;
    }

    private void OnDisable()
    {
     scoreController.OnFinalScore -= Win;
    }
}
