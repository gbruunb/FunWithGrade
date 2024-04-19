using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    int page = 0;

    public GameObject[] pageArr;
    public Button previous;
    public Button next;
    public Text pageTxt;

    public Button backHome;

    // Start is called before the first frame update
    void Start()
    {
        previous.onClick.AddListener(GoPreviousPage);
        next.onClick.AddListener(GoNextPage);
        backHome.onClick.AddListener(BackHomeFn);
    }

    private void BackHomeFn()
    {
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        pageTxt.text = (page + 1).ToString("0") +" | "+ (pageArr.Length).ToString("0");
    }

    private void GoPreviousPage()
    {
        page--;
        if(page >= 0)
        {
            pageArr[page + 1].SetActive(false);
            pageArr[page].SetActive(true);

        }
        else
        {
            page = 0;
        }
    }

    private void GoNextPage()
    {
        page++;
        if(page < pageArr.Length)
        {
            pageArr[page - 1].SetActive(false);
            pageArr[page].SetActive(true);

        }
        else
        {
            page = pageArr.Length - 1;
        }

    }
}
