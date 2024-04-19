using System;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 0.8f;

    public static int height = 20;
    public static int width = 30;

    private static Transform[,] grid = new Transform[width, height];

    public static bool getAllGrade;
    public static bool getGradeF;

    public static int gameOver = 0; //-1 is lose | 0 is playing | 1 is win

    // Update is called once per frame
    void Update()
    {
        if(gameOver == 0 && !Panel.PausePanel.activeSelf)
            SpawnTetromino.targetTime -= Time.deltaTime;

        if (!Panel.AJPanel.activeSelf && !Panel.GameOverPanel.activeSelf && gameOver == 0 && !Panel.PausePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidMove())
                    transform.position -= new Vector3(-1, 0, 0);

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidMove())
                    transform.position -= new Vector3(1, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!ValidMove())
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }

            if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    AddToGrid();
                    CheckForLines();
                    checkOverSpawnPoint(15);
                    Check10BallotHorizontal();
                    Check10BallotVertical();
                    checkPoo();
                    checkCovid();
                    checkGrade();
                    checkStone();
                    checkWaterMelon();
                    checkAJ();
                    checkFloatBlock();
                    this.enabled = false;

                    if (checkWaterMelon() || SpawnTetromino.haveWatermelon)
                    {
                        FindObjectOfType<SpawnTetromino>().NewAJMo();
                    }
                    else if(getAllGrade == true || getGradeF == true)
                    {
                        FindObjectOfType<SpawnTetromino>().NewGrade();
                        getGradeF = false;
                    }
                    else
                    {
                        FindObjectOfType<SpawnTetromino>().NewTeTromino();
                    }
                    
                    
                }
                previousTime = Time.time;
            }
        }
    }

    private void checkOverSpawnPoint(int maxHeight) 
    {
        for (int i = 0; i < width; i++)
        {
                try
                {
                    if(grid[i, maxHeight] != null)
                    {
                    SpawnTetromino.descriptionStr = "You lose! Your block is over height limit!";
                    SpawnTetromino.winLoseStr = "Lose !";
                    Debug.Log(gameOver);

                    Panel.GameOverPanel.SetActive(true);
                    gameOver = -1;
                    }
                }
                catch (Exception e)
                {
                    
                }
        }
    }

    private void checkRedWTML()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (grid[i, j] != null
                        && grid[i, j].parent.name == "Watermelon-red(Clone)"
                        && grid[i + 1, j].parent.name == "Watermelon-red(Clone)"
                        && grid[i, j - 1].parent.name == "Watermelon-red(Clone)"
                        && grid[i + 1, j - 1].parent.name == "Watermelon-red(Clone)")
                    {
                        Destroy(grid[i, j].gameObject);
                        grid[i, j] = null;

                        Destroy(grid[i + 1, j].gameObject);
                        grid[i + 1, j] = null;

                        Destroy(grid[i, j - 1].gameObject);
                        grid[i, j - 1] = null;

                        Destroy(grid[i + 1, j - 1].gameObject);
                        grid[i + 1, j - 1] = null;
                        SpawnTetromino.haveWatermelon = true;
                        
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
    }

    private void checkAJ()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (grid[i, j] != null && (grid[i, j].parent.name == "AJMo(Clone)" || grid[i, j].parent.name == "AJPan(Clone)" || grid[i, j].parent.name == "AJMoBody(Clone)")
                        && grid[i, j - 1].transform.name == "B11"
                        && grid[i - 1, j - 1].transform.name == "B10"
                        && grid[i + 1, j - 1].transform.name == "B12"
                        && grid[i, j - 2].transform.name == "B01"
                        && grid[i - 1, j - 2].transform.name == "B00"
                        && grid[i + 1, j - 2].transform.name == "B02"
                        )
                    {
                        Destroy(grid[i-1, j-1].gameObject);
                        grid[i-1, j-1] = null;
                        Destroy(grid[i - 1, j - 2].gameObject);
                        grid[i - 1, j - 2] = null;

                        Destroy(grid[i + 1, j - 1].gameObject);
                        grid[i + 1, j - 1] = null;
                        Destroy(grid[i + 1, j - 2].gameObject);
                        grid[i + 1, j - 2] = null;

                        Panel.randQ = UnityEngine.Random.Range(0, Panel.question.Length);

                        Panel.AJPanel.SetActive(true);
                    }

                }
                catch (Exception e)
                {

                }
            }
        }
    }

    private void checkGrade()
    {
        float totalChangeScore = 0;
        float totalChangeTime = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (grid[i, j].parent.name == "Grade A(Clone)")
                    {
                        totalChangeScore += 30;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else if (grid[i, j].parent.name == "Grade B+(Clone)")
                    {
                        totalChangeScore += 10;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else if (grid[i, j].parent.name == "Grade B(Clone)")
                    {
                        totalChangeScore += 10;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else if (grid[i, j].parent.name == "Grade C+(Clone)")
                    {
                        totalChangeTime += 7.5f;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else if (grid[i, j].parent.name == "Grade C(Clone)")
                    {
                        totalChangeTime += 10;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else if (grid[i, j].parent.name == "Grade D+(Clone)")
                    {
                        totalChangeTime += 2.5f;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else if (grid[i, j].parent.name == "Grade D(Clone)")
                    {
                        totalChangeTime += 2;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else if (grid[i, j].parent.name == "Grade F(Clone)")
                    {
                        totalChangeScore -= 30;
                        totalChangeTime -= 10;
                        increseScoreByGrade(totalChangeScore, totalChangeTime, i, j);

                    }
                    else
                    {
                    }

                }
                catch (Exception e)
                {

                }
                
            }
        }
        
    }

    private void increseScoreByGrade(float scoreParam, float timeParam, int i, int j)
    {
        SpawnTetromino.score += scoreParam;
        SpawnTetromino.targetTime += timeParam;
        getAllGrade = false;
        Destroy(grid[i, j].gameObject);
        grid[i, j] = null;
    }

    private void checkPoo()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (grid[i, j] != null && grid[i, j].transform.name == "Poo")
                    {
                        SpawnTetromino.score--;
                    }
                }
                catch (Exception e)
                {

                }

            }
        }
    }

    private void checkCovid()
    {
        int covid = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (grid[i, j] != null && grid[i, j].parent.name == "Covid-19(Clone)")
                    {
                        covid++;
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
        if(covid > 3)
        {
            SpawnTetromino.score -= 10;
        }
    }

    private bool checkWaterMelon()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (grid[i, j] != null &&
                        //red => right bottom
                        (grid[i, j].parent.name == "Watermelon-green(Clone)"
                        && grid[i, j + 1].parent.name == "Watermelon-green(Clone)"
                        && grid[i, j + 2].parent.name == "Watermelon-green(Clone)"
                        && grid[i + 1, j + 2].parent.name == "Watermelon-green(Clone)"
                        && grid[i + 2, j + 2].parent.name == "Watermelon-green(Clone)"

                        && grid[i + 1, j].parent.name == "Watermelon-red(Clone)"
                        && grid[i + 1, j + 1].parent.name == "Watermelon-red(Clone)"
                        && grid[i + 2, j].parent.name == "Watermelon-red(Clone)"
                        && grid[i + 2, j + 1].parent.name == "Watermelon-red(Clone)")
                        ||
                        //red => right top

                        (grid[i, j].parent.name == "Watermelon-green(Clone)"
                             && grid[i, j + 1].parent.name == "Watermelon-green(Clone)"
                             && grid[i, j + 2].parent.name == "Watermelon-green(Clone)"
                             && grid[i + 1, j].parent.name == "Watermelon-green(Clone)"
                             && grid[i + 2, j].parent.name == "Watermelon-green(Clone)"

                             && grid[i + 1, j + 1].parent.name == "Watermelon-red(Clone)"
                             && grid[i + 1, j + 2].parent.name == "Watermelon-red(Clone)"
                             && grid[i + 2, j + 1].parent.name == "Watermelon-red(Clone)"
                             && grid[i + 2, j + 2].parent.name == "Watermelon-red(Clone)")
                        ||
                        //red => left top

                         (grid[i, j].parent.name == "Watermelon-green(Clone)"
                             && grid[i+1, j].parent.name == "Watermelon-green(Clone)"
                             && grid[i+2, j].parent.name == "Watermelon-green(Clone)"
                             && grid[i + 2, j+1].parent.name == "Watermelon-green(Clone)"
                             && grid[i + 2, j+2].parent.name == "Watermelon-green(Clone)"

                             && grid[i, j + 1].parent.name == "Watermelon-red(Clone)"
                             && grid[i, j + 2].parent.name == "Watermelon-red(Clone)"
                             && grid[i + 1, j + 1].parent.name == "Watermelon-red(Clone)"
                             && grid[i + 1, j + 2].parent.name == "Watermelon-red(Clone)")

                         ||
                         //red => left bottom

                         (grid[i+2, j].parent.name == "Watermelon-green(Clone)"
                             && grid[i + 2, j+1].parent.name == "Watermelon-green(Clone)"
                             && grid[i + 2, j+2].parent.name == "Watermelon-green(Clone)"
                             && grid[i + 1, j + 2].parent.name == "Watermelon-green(Clone)"
                             && grid[i, j + 2].parent.name == "Watermelon-green(Clone)"

                             && grid[i, j].parent.name == "Watermelon-red(Clone)"
                             && grid[i+1, j].parent.name == "Watermelon-red(Clone)"
                             && grid[i, j + 1].parent.name == "Watermelon-red(Clone)"
                             && grid[i + 1, j + 1].parent.name == "Watermelon-red(Clone)")


                        )
                    {
                        checkRedWTML();
                        return true;
                    }
                    

                }catch(Exception e)
                {

                }
            }
        }
        return false;
    }


    private void checkStone()
    {
        int posI = -1;
        int posJ = -1;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j] != null && grid[i, j].transform.name == "sqb0")
                {
                    posI = i;
                    posJ = j;
                    for (int k = j - 1; k >= 0; k--)
                    {
                        for (int l = i; l < i + 3; l++)
                        {
                            try
                            {
                                if (grid[l, k].parent.name == "I Tetromino(Clone)"
                                    || grid[l, k].parent.name == "J Tetromino(Clone)"
                                    || grid[l, k].parent.name == "L Tetromino(Clone)"
                                    || grid[l, k].parent.name == "O Tetromino(Clone)"
                                    || grid[l, k].parent.name == "S Tetromino(Clone)"
                                    || grid[l, k].parent.name == "T Tetromino(Clone)"
                                    || grid[l, k].parent.name == "Z Tetromino(Clone)"
                                    )
                                {
                                    SpawnTetromino.score += 5;
                                }
                                else if (grid[l, k].parent.name == "Covid-19(Clone)")
                                {
                                    SpawnTetromino.score -= 10;
                                }
                                else if (grid[l, k].parent.name == "Ballot(Clone)")
                                {
                                    SpawnTetromino.score += 2;
                                }
                                else if (grid[l, k].parent.name == "AJMo(Clone)" || grid[l, k].parent.name == "AJPan(Clone)" || grid[l, k].parent.name == "AJBody(Clone)")
                                {
                                    SpawnTetromino.score += 20;
                                }
                                else if (grid[l, k].parent.name == "Grade A(Clone)")
                                {
                                    SpawnTetromino.score += 30;
                                }
                                else if (grid[l, k].parent.name == "Grade B+(Clone)")
                                {
                                    SpawnTetromino.score += 10;
                                }
                                else if (grid[l, k].parent.name == "Grade B(Clone)")
                                {
                                    SpawnTetromino.score += 10;
                                }
                                else if (grid[l, k].parent.name == "Grade C+(Clone)")
                                {
                                    SpawnTetromino.targetTime += 7.5f;
                                }
                                else if (grid[l, k].parent.name == "Grade C(Clone)")
                                {
                                    SpawnTetromino.targetTime += 10;
                                }
                                else if (grid[l, k].parent.name == "Grade D+(Clone)")
                                {
                                    SpawnTetromino.targetTime += 2.5f;
                                }
                                else if (grid[l, k].parent.name == "Grade D(Clone)")
                                {
                                    SpawnTetromino.targetTime += 2;
                                }
                                else if (grid[l, k].parent.name == "Grade F(Clone)")
                                {
                                    SpawnTetromino.score -= 30;
                                    SpawnTetromino.targetTime -= 10;
                                }
                                else if (grid[l, k].parent.name == "Watermelon-red(Clone)")
                                {
                                    SpawnTetromino.targetTime += 10;
                                }
                                else if (grid[l, k].parent.name == "Watermelon-green(Clone)")
                                {
                                    SpawnTetromino.targetTime += 10;
                                }
                                else if (grid[l, k].parent.name == "Poo(Clone)")
                                {
                                    SpawnTetromino.score -= 10;
                                }
                                else if (grid[l, k].parent.name == "AJMoBody(Clone)")
                                {
                                    SpawnTetromino.score += 30;
                                }
                                Destroy(grid[l, k].gameObject);
                                grid[l, k] = null;
                            }
                            catch (Exception e)
                            {

                            }


                        }
                    }
                }
            }
        }

        if(posI != -1 && posJ != -1)
        {
            removeStone(posI, posJ);
        }
    }

    private void removeStone(int posI, int posJ)
    {

        //remove all stone in map
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j] != null && grid[i, j].parent.name == "Stone(Clone)")
                {
                    try
                    {
                        Destroy(grid[i, j].gameObject);
                        grid[i, j] = null;
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }


    private void Check10BallotHorizontal()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            Has10BallotHorizontal(i);
        }
    }

    private void Has10BallotHorizontal(int i)
    {
        int Ballot = 0;
        int start = -1;
        int stop = -1;
        for (int j = 0; j < width - 5; j++) 
        {
            Ballot = 0;
            
            for(int k = j; k < j + 5; k++)
            {
                try
                {
                    if (grid[k, i].parent.name == "Ballot(Clone)")
                    {
                        Ballot++;
                    }
                    
                }catch(Exception e)
                {

                }
                
            }

            if (Ballot == 5)
            {
                start = j;
                stop = j + 4;
                DeleteInRange(i, start, stop, "horizontal");
                RowDownInRange(i, start, stop);
                RemoveCovid();
            }
        }
    }


    private void Check10BallotVertical()
    {
        for (int i = width - 1; i >= 0; i--)
        {
            Has10BallotVertical(i);
        }
    }

    private void Has10BallotVertical(int i)
    {
        int Ballot = 0;
        int start = -1;
        int stop = -1;
        for (int j = 0; j < height - 5; j++)
        {
            Ballot = 0;

            for (int k = j; k < j + 5; k++)
            {
                try
                {
                    if (grid[i, k].parent.name == "Ballot(Clone)")
                    {
                        Ballot++;
                    }

                }
                catch (Exception e)
                {

                }

            }

            if (Ballot == 5)
            {
                start = j;
                stop = j + 4;
                DeleteInRange(i, start, stop, "vertical");
                RemoveCovid();
            }
        }
    }


    private void DeleteInRange(int i, int start, int stop, String direction)
    {
        if(direction == "vertical")
        {
            for (int j = start; j <= stop; j++)
            {
                Destroy(grid[i, j].gameObject);
                grid[i, j] = null;
            }
        }
        else
        {
            for (int j = start; j <= stop; j++)
            {
                try
                {
                    Destroy(grid[j, i].gameObject);
                    grid[j, i] = null;
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
        
    }

    private void RowDownInRange(int i, int start, int stop)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = start; j <= stop; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void CheckForLines()
    {
        for(int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
                SpawnTetromino.score += 50; //score with line
            }
        }
    }



    private bool HasLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }

        return true;
    }

    private void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    private void RowDown(int i)
    {
        for(int y  = i; y < height; y++)
        {
            for(int j = 0; j < width; j++)
            {
                if (grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }


    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
            
        }
    }

    private bool ValidMove()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }



    private void RemoveCovid()
    {
        
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                try       
                {
                    if (grid[j, i].parent.name == "Covid-19(Clone)")
                    {
                        Destroy(grid[j, i].gameObject);
                        grid[j, i] = null;
                    }
                }
                catch (Exception e)
                {

                }
            }
            }
        
    }

    private void checkFloatBlock()
    {
        for(int i = 0; i < width; i++)
        {
            for(int h = 0; h < height; h++)
            {
                for (int j = 1; j < height; j++)
                {
                    if (grid[i, j] != null && grid[i, j - 1] == null)
                    {
                        grid[i, j - 1] = grid[i, j];
                        grid[i, j] = null;
                        grid[i, j - 1].transform.position -= new Vector3(0, 1, 0);
                    }
                }
            }
            
        }
    }

}
