using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public Button howToPlay;
    public Button credit;
    public Button start;
    public Button exit;

    // Start is called before the first frame update
    void Start()
    {
        howToPlay.onClick.AddListener(HowToPlayFn);
        credit.onClick.AddListener(CreditFn);
        start.onClick.AddListener(StartGameFn);
        exit.onClick.AddListener(ExitFn);
    }

    private void HowToPlayFn()
    {
        SceneManager.LoadScene("HowToPlay", LoadSceneMode.Single);
    }

    private void CreditFn()
    {
        SceneManager.LoadScene("Credit", LoadSceneMode.Single);
    }

    private void StartGameFn()
    {
        TetrisBlock.gameOver = 0;
        SpawnTetromino.targetTime = 4 * 60.0f; //4 min * 60 seconds
        SpawnTetromino.score = 0;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);

    }

    private void ExitFn()
    {
        Application.Quit();
    }
}
