using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{

    public Button submit;
    public Text questionGen;
    public InputField answerToAJ;

    public static int randQ = 0;
    public static string[] question = new string[5] {   "System.out.println(\"ComedySci\");",
                                                        "int[] a = { 5, 2, 1 };\nSystem.out.println(Arrays.toString(a));",
                                                        "int a = 2;\nSystem.out.println(a + 2);",
                                                        "System.out.println(( 2 % 3 % 4 ) + ( 54 % 7 ));",
                                                        "System.out.println(!true && false || (!false && !true));" };

    private readonly string[] correctAnswer = new string[5] {  "ComedySci",
                                                               "[5, 2, 1]",
                                                               "4",
                                                               "7",
                                                               "false"};

    public static GameObject AJPanel;
    public static GameObject GameOverPanel;
    public static GameObject PausePanel;

    GameObject[] inactiveGameObjects;
    public static string answer;

    public Button backToHome;
    public Button pause;

    //pause panel
    public Button resume;
    public Button pauseAndBackToHome;


    // Start is called before the first frame update
    void Start()
    {
        submit.onClick.AddListener(ReadAnswer);
        backToHome.onClick.AddListener(backToHomeFn);
        pause.onClick.AddListener(pauseFn);
        resume.onClick.AddListener(resumeFn);
        pauseAndBackToHome.onClick.AddListener(pauseAndBackToHomeFn);

        inactiveGameObjects = FindObjectsOfType<GameObject>(true);
        foreach (GameObject gameObject in inactiveGameObjects)
        {
            if (gameObject.tag == "AJPanel")
            {
                AJPanel = gameObject;
            }
            else if (gameObject.tag == "GameOver")
            {
                GameOverPanel = gameObject;
            }
            else if (gameObject.tag == "Pause")
            {
                PausePanel = gameObject;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        questionGen.text = question[randQ];

    }

    private void ReadAnswer()
    {
        answer = answerToAJ.text;
        if (answer == correctAnswer[randQ])
        {
            TetrisBlock.getAllGrade = true;
        }
        else
        {
            TetrisBlock.getGradeF = true;
        }
        AJPanel.SetActive(false);
    }

    private void backToHomeFn()
    {
        
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }

    private void pauseFn()
    {
        PausePanel.SetActive(true);
    }

    private void resumeFn()
    {
        PausePanel.SetActive(false);
    }

    private void pauseAndBackToHomeFn()
    {
        SpawnTetromino.targetTime = 4 * 60.0f;
        SpawnTetromino.score = 0;
        
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }
}
