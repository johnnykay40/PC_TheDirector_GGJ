using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public event Action OnFinalScore;
    public Image[] carpetScore;
    public int maxScore;
    private int actualScore;
    public Sprite activeSprite;
    public Sprite desactiveSprite;

    private void Start()
    {
        maxScore = carpetScore.Length;
        Reset();
    }
    public void Score()
    {
        AddScore();
    }
    private void AddScore()
    {
        if (actualScore < maxScore)
        {
            carpetScore[actualScore].sprite = activeSprite;
            actualScore++;
        }
        if (actualScore >= maxScore)
        {
            OnFinalScore?.Invoke();
        }
    } 

    public void Reset()
    {
        foreach (var file in carpetScore)
        {
            file.sprite = desactiveSprite;
        }

        actualScore = 0;
    }
}
