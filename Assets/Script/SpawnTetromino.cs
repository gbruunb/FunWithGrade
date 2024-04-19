using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    public GameObject[] Grade;
    public GameObject AJMoBody;
    public Text timer;
    public Text scoreText;

    public static float targetTime; 
    public static float score;

    public static bool haveCovid;
    public static bool havePoo;
    public static bool haveWatermelon;
    
    public Text winLose;
    public Text scoreResult;
    public Text description;

    public static string winLoseStr = "";
    public static string descriptionStr = "";

    // Start is called before the first frame update
    void Start()
    {
            NewTeTromino();
    }

    void Update()
    {
        winLose.text = winLoseStr;
        scoreResult.text = "Score " + score.ToString("0") + " / 500";
        description.text = descriptionStr;

        if (targetTime > 240) targetTime = 240; //max add time = 4 min
        
        string scoreStr = score.ToString("0");
        scoreText.text = "Score: " + scoreStr;

        if(targetTime >= 0)
        {
            string second = (targetTime % 60).ToString("00");
            string timeStr = System.Math.Floor(targetTime / 60) + " : " + second;
            timer.text = "Time => " + timeStr;
        }
        else
        {
            timer.text = "Time Out!!!!";

            if (score >= 500)
            {
                descriptionStr = "Time Out! But you reach 500 scores!";
                winLoseStr = "WIN !";
                TetrisBlock.gameOver = 1;
            }
            else
            {
                descriptionStr = "Time Out!";
                winLoseStr = "Lose !";
                TetrisBlock.gameOver = -1;
            }
            Panel.GameOverPanel.SetActive(true);
        }

        if(TetrisBlock.gameOver == 0 && Panel.GameOverPanel.activeSelf)
        {
            Debug.Log("Panel.GameOverPanel.activeSelf");
            Panel.GameOverPanel.SetActive(false);
        }

        if(score >= 500)
        {
            descriptionStr = "You reach 500 scores!";
            winLoseStr = "WIN !";
            TetrisBlock.gameOver = 1;
            Panel.GameOverPanel.SetActive(true);
        }


    }
    public void NewTeTromino()
    {
            Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
    }

    public void NewAJMo()
    {
        haveWatermelon = false;

        if (!Panel.AJPanel.activeSelf)
        {
            Instantiate(AJMoBody, transform.position, Quaternion.identity);
        }

    }

    public void NewGrade()
    {
        if (TetrisBlock.getAllGrade == true)
        {
            Instantiate(Grade[Random.Range(0, Grade.Length)], transform.position, Quaternion.identity);
        }
        else if(TetrisBlock.getGradeF == true)
        {
            Instantiate(Grade[Grade.Length-1], transform.position, Quaternion.identity);
        }
    }
}
