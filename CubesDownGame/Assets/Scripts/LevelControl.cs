using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private Material[] arrMaters;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject cubeSelPrefab;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject heart;
    [SerializeField] private SpawnCubes spLeft;
    [SerializeField] private SpawnCubes spRight;
    [SerializeField] private UI_Control ui_Control;

    private bool isPause = true;

    private float timer = 2f;
    private int countLive = 3;
    private int spavnLeft = 0;
    private int spavnRight = 0;

    private int oldLevel = 1;
    private int level = 1;
    private int score = 0;

    private GameObject cubesLeft = null;
    private GameObject cubesRight = null;
    private List<GameObject> cubes = new List<GameObject>();

    private List<LevelInfo> listLevels = new List<LevelInfo>();
    private List<Vector3> listPositions = new List<Vector3>();
    private int[] znCols = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateLevels();
        CreatePositions();
        GenerateSelectedCubes();
        SpawnCubes();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause) return;
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            timer = 1.5f;
            //LiveMinus();
            spavnLeft++;
            spavnRight++;
            if (spavnLeft == 3)
            {
                spavnLeft = 0;                
            }
            if (spavnRight == 3)
            {
                spavnRight = 0;
            }
            SpawnCubes();
        }
    }

    public void SetPause(bool pause)
    {
        isPause = pause;
    }

    private void CreateLevels()
    {
        listLevels.Clear();
        listLevels.Add(new LevelInfo(1, 5, 1));
        listLevels.Add(new LevelInfo(2, 10, 2));
        listLevels.Add(new LevelInfo(3, 20, 2, 4));
        listLevels.Add(new LevelInfo(4, 30, 2, 5));
        listLevels.Add(new LevelInfo(5, 50, 3, 5));
        listLevels.Add(new LevelInfo(6, 70, 3, 5, 3, true));
        listLevels.Add(new LevelInfo(7, 100, 3, 5, 4, true));
        listLevels.Add(new LevelInfo(8, 150, 3, 5, 5, true));
        listLevels.Add(new LevelInfo(9, 200, 4, 5, 5, true));
        listLevels.Add(new LevelInfo(10, 250, 4, 6, 5, true));
        listLevels.Add(new LevelInfo(11, 300, 4, 7, 5, true));
        listLevels.Add(new LevelInfo(12, 350, 5, 7, 5, true));
        listLevels.Add(new LevelInfo(13, 400, 5, 7, 6, true));
        listLevels.Add(new LevelInfo(14, 450, 6, 7, 6, true));
        listLevels.Add(new LevelInfo(15, 450, 6, 7, 7, true));
        listLevels.Add(new LevelInfo(16, 500, 7, 7, 7, true));
        listLevels.Add(new LevelInfo(17, 550, 8, 7, 7, true));
        listLevels.Add(new LevelInfo(18, 600, 8, 8, 7, true));
        listLevels.Add(new LevelInfo(19, 700, 8, 8, 8, true));
        listLevels.Add(new LevelInfo(20, 800, 9, 8, 8, true));
    }

    private void CreatePositions()
    {
        listPositions.Clear();
        listPositions.Add(new Vector3(-2.4f, 1f, 1.1f));
        listPositions.Add(new Vector3(-2.4f, 1f, -1.1f));
        listPositions.Add(new Vector3(-2.4f, 1f, 0));
        listPositions.Add(new Vector3(0, 1f, 0));
        listPositions.Add(new Vector3(0, 1f, 1.1f));
        listPositions.Add(new Vector3(0, 1f, -1.1f));
        listPositions.Add(new Vector3(2.4f, 1f, -1.1f));
        listPositions.Add(new Vector3(2.4f, 1f, 1.1f));
        listPositions.Add(new Vector3(2.4f, 1f, 0));
    }

    private void LiveMinus()
    {
        countLive--;
        if (countLive > 0)
        {            
            GameObject heartLive = Instantiate(heartPrefab, heart.transform.position, Quaternion.identity);
            heartLive.GetComponent<HeartControl>().SetDown();
            Destroy(heartLive, 3f);
        }
        else
        {
            ui_Control.ViewLossPanel(level, score);
            heart.SetActive(false);
        }
        ui_Control.ViewLive(countLive);
    }

    public void LivePlus(int cnt)
    {
        countLive += cnt;
        heart.SetActive(true);
        ui_Control.ViewLive(countLive);
    }

    public void Restart()
    {
        score = 0; 
        ui_Control.ViewScore(score);

        level = 1;
        oldLevel = level;
        ui_Control.ViewLevel(level);

        countLive = 3;
        ui_Control.ViewLive(countLive);
        heart.SetActive(true);

        foreach(GameObject cube in cubes)
        {
            GameObject effect = Instantiate(effectPrefab, cube.transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            cubes.Remove(cube);
            Destroy(cube);
        }
        for (int i = 0; i < 9; i++) znCols[i] = -1;
        GenerateSelectedCubes();
    }

    private void SpawnCubes()
    {
        LevelInfo li = listLevels[0];
        for (int i = 0; i < listLevels.Count; i++)
        {
            if (listLevels[i].Score >= score)
            {
                li = listLevels[i];
                level = li.Level;
                if (level != oldLevel)
                {
                    oldLevel = level;
                    LivePlus(1);
                }
                break;
            }
        }
        ui_Control.ViewLevel(li.Level);

        if (spavnLeft == 0)
        {
            spLeft.SpawnNewCubes(li.CountDownCubesLeft, znCols, 3f + (level - 1) * 0.1f, level > 10);
        }
        if (spavnRight == 0 && li.IsRight)
        {
            spRight.SpawnNewCubes(li.CountDownCubesRight, znCols, 3f + (level - 6) * 0.1f, level > 10);
        }
    }

    private void GenerateSelectedCubes()
    {
        LevelInfo li = listLevels[level - 1];
        List<int> nums = new List<int>();
        int i, cntNums = (level < 6) ? 6 : 9;
        int cntOkNums = 0;
        for (i = 0; i < cntNums; i++)
        {
            if (znCols[i] == -1) nums.Add(i);
            else cntOkNums++;
        }
        while (cntOkNums < li.CountSelectedCubes)
        {
            int numPos = Random.Range(0, nums.Count);
            GenerateCube(nums[numPos]);
            nums.Remove(nums[numPos]);
            cntOkNums++;
        }

        /*if (cubes.Count < li.CountSelectedCubes)
        {
            for (i = 0; i < li.CountSelectedCubes; i++) 
            {
                if (znCols[i] == -1)
                {
                    GenerateCube(i);
                }
            }
        }*/
        if (cubes.Count == 0) 
        {
            GenerateCube(4);
        }
    }

    private void GenerateCube(int numPos)
    {
        GameObject cube = Instantiate(cubeSelPrefab, listPositions[numPos], Quaternion.identity);
        cube.GetComponent<CubeSelect>().SetParams(gameObject.GetComponent<LevelControl>(), numPos);
        CubeControl cc = cube.GetComponent<CubeControl>();
        int numMat1 = Random.Range(0, arrMaters.Length);
        int numMat2 = Random.Range(0, arrMaters.Length);
        if (level > 10)
        {
            cc.SetColors(arrMaters[numMat1], arrMaters[numMat2], numMat1, numMat2);
        }
        else
        {
            cc.SetColors(arrMaters[numMat1], arrMaters[numMat1], numMat1, numMat1);
        }
        znCols[numPos] = cc.CubeColor;
        cubes.Add(cube);
        //cubes.Insert(numPos, cube);
    }

    public void SendParent(GameObject parent, bool isLeft = true)
    {
        if (isLeft) 
        {
            cubesLeft = parent;
        }
        else
        {
            cubesRight = parent;
        }
    }

    public void CubeSelect(GameObject cube)
    {
        bool isSelectedOk = TestColorComparison(cube);
        if (isSelectedOk)
        {
            List<GameObject> delCubes = new List<GameObject>();
            List<int> delNumPositions = new List<int>();
            int i, j, numPos = cube.GetComponent<CubeSelect>().NumberPosition;
            
            //delNumPositions.Add(numPos);
            for (i = 0; i < cubes.Count; i++)
            {
                GameObject cb = cubes[i];
                float cz = cb.transform.position.z, cx = cb.transform.position.x;
                bool isOK = false;
                if (cubesLeft != null && cx < 1f)
                {
                    for (j = 0; j < cubesLeft.transform.childCount; j++)
                    {
                        GameObject go = cubesLeft.transform.GetChild(j).gameObject;
                        //sb.Append($"<left_{i+1}={go.transform.position}> ");
                        CubeControl cc = go.GetComponent<CubeControl>();
                        if (Mathf.Abs(go.transform.position.z - cz) < 0.5f)
                        {
                            if (cc.CubeColor == cb.GetComponent<CubeControl>().CubeColor)
                            {
                                delCubes.Add(go);
                                isOK = true;
                                break;
                            }
                        }
                    }
                }
                if (cubesRight != null && cx > -1)
                {
                    for (j = 0; j < cubesRight.transform.childCount; j++)
                    {
                        GameObject go = cubesRight.transform.GetChild(j).gameObject;
                        //sb.Append($"<right_{i + 1}={go.transform.position}> ");
                        CubeControl cc = go.GetComponent<CubeControl>();
                        if (Mathf.Abs(go.transform.position.z - cz) < 0.5f)
                        {
                            if (cc.CubeColor == cb.GetComponent<CubeControl>().CubeColor)
                            {
                                delCubes.Add(go);
                                isOK = true;
                                break;
                            }
                        }
                    }
                }
                if (isOK)
                {
                    delNumPositions.Add(cb.GetComponent<CubeSelect>().NumberPosition);
                }
            }
            DeletingCubes(delCubes, delNumPositions);
            Invoke("GenerateSelectedCubes", 1f);
        }
        else
        {   //  Минус одна попытка
            LiveMinus();
        }


        //print(sb.ToString());
    }

    private bool TestColorComparison(GameObject cube)
    {
        bool isSelectedOk = false;
        float cz = cube.transform.position.z, cx = cube.transform.position.x;
        if (cubesLeft != null && cx < 1f)
        {
            for (int i = 0; i < cubesLeft.transform.childCount; i++)
            {
                GameObject go = cubesLeft.transform.GetChild(i).gameObject;
                CubeControl cc = go.GetComponent<CubeControl>();
                if (Mathf.Abs(go.transform.position.z - cz) < 0.5f)
                {
                    if (cc.CubeColor == cube.GetComponent<CubeControl>().CubeColor)
                    {
                        isSelectedOk = true;
                        break;
                    }
                }
            }
        }
        if (cubesRight != null && cx > -1)
        {
            for (int i = 0; i < cubesRight.transform.childCount; i++)
            {
                GameObject go = cubesRight.transform.GetChild(i).gameObject;
                CubeControl cc = go.GetComponent<CubeControl>();
                if (Mathf.Abs(go.transform.position.z - cz) < 0.5f)
                {
                    if (cc.CubeColor == cube.GetComponent<CubeControl>().CubeColor)
                    {
                        isSelectedOk = true;
                        break;
                    }
                }
            }
        }
        return isSelectedOk;
    }

    private void DeletingCubes(List<GameObject> listCubes, List<int> listNumbers) 
    {
        print($"paramCubes={listCubes.Count} numbers={listNumbers.Count}");
        int i;
        score += listNumbers.Count;
        if (listNumbers.Count > 1) score += listNumbers.Count - 1;
        
        for (i = 0; i < listCubes.Count; i++)
        {
            GameObject go = listCubes[i];
            GameObject downEffect = Instantiate(effectPrefab, go.transform.position, Quaternion.identity);
            Destroy(downEffect, 0.5f);
            Destroy(go);
        }
        for (i = 0; i < listNumbers.Count; i++)
        {
            int numPos = listNumbers[i];
            for (int j = 0; j < cubes.Count; j++)
            {
                GameObject cube = cubes[j];
                if (cube.GetComponent<CubeSelect>().NumberPosition == numPos)
                {
                    if (cube.GetComponent<CubeControl>().CubeColor >= 10) score++;
                    GameObject effect = Instantiate(effectPrefab, cube.transform.position, Quaternion.identity);
                    Destroy(effect, 0.5f);
                    cubes.Remove(cube);
                    znCols[numPos] = -1;
                    Destroy(cube);
                    break;
                }                
            }
        }
        ui_Control.ViewScore(score);
    }
}

public class LevelInfo
{
    public int Level { get; set; }
    public int Score { get; set; }
    public int CountSelectedCubes { get; set; }
    public int CountDownCubesLeft { get; set; }
    public int CountDownCubesRight { get; set; }
    public bool IsRight { get; set; }

    public LevelInfo() { }
    public LevelInfo(int lev, int sc, int cntSel, int cntDCLeft = 3, int cntDCRight = 3, bool isR = false)
    {
        Level = lev;
        Score = sc;
        CountSelectedCubes = cntSel;
        CountDownCubesLeft = cntDCLeft;
        CountDownCubesRight = cntDCRight;
        IsRight = isR;
    }
}
