using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private Material[] arrBonusMaters;
    [SerializeField] private Material[] arrMaters;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject cubeSelPrefab;
    [SerializeField] private GameObject cubeBonusPrefab;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject heart;
    [SerializeField] private SpawnCubes spLeft;
    [SerializeField] private SpawnCubes spRight;
    [SerializeField] private UI_Control ui_Control;
    [SerializeField] private int spawnBonus = 20;
    [SerializeField] private int slowSpeed = 2;

    private bool isPause = true;

    private float timer = 2f;
    private int countLive = 3;
    private int countBonus = 0;
    private int spavnLeft = 0;
    private int spavnRight = 0;

    private int oldLevel = 1;
    private int level = 1;
    private int score = 0;

    private int slowCount = 0;

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
            if (slowCount > 0) timer += 2f;
            //LiveMinus();
            spavnLeft++;
            spavnRight++;
            if (spavnLeft >= 3)
            {
                spavnLeft = 0;                
            }
            if (spavnRight >= 3)
            {
                spavnRight = 0;
            }
            SpawnCubes();
        }
    }

    public void SetPause(bool pause)
    {
        isPause = pause;
        ui_Control.ViewLevel(level);
        ui_Control.ViewScore(score);
        if (isPause == false)
        {
            spavnLeft = 0;
            spavnRight = 0;
            SpawnCubes();
        }
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
        listLevels.Add(new LevelInfo(17, 550, 7, 7, 7, true));
        listLevels.Add(new LevelInfo(18, 600, 8, 7, 7, true));
        listLevels.Add(new LevelInfo(19, 700, 8, 8, 7, true));
        listLevels.Add(new LevelInfo(20, 800, 8, 8, 8, true));
        listLevels.Add(new LevelInfo(21, 900, 9, 8, 8, true));
        listLevels.Add(new LevelInfo(22, 1000, 9, 9, 8, true));
        listLevels.Add(new LevelInfo(23, 1100, 9, 9, 8, true));
        listLevels.Add(new LevelInfo(24, 1200, 9, 9, 9, true));
        listLevels.Add(new LevelInfo(25, 1300, 9, 9, 9, true));
        listLevels.Add(new LevelInfo(26, 1400, 9, 9, 9, true));
        listLevels.Add(new LevelInfo(27, 1500, 9, 9, 9, true));
        listLevels.Add(new LevelInfo(28, 1600, 9, 9, 9, true));
        listLevels.Add(new LevelInfo(29, 1700, 9, 9, 9, true));
        listLevels.Add(new LevelInfo(30, 1800, 9, 9, 9, true));
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
            SaveProgress();
        }
        ui_Control.ViewLive(countLive);
    }

    public void LivePlus(int cnt)
    {
        countLive += cnt;
        if (countLive > 7) countLive = 7;
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

    private void SaveProgress()
    {
        if (score > GameManager.Instance.currentPlayer.totalScore)
        {
            GameManager.Instance.currentPlayer.totalScore = score;
            GameManager.Instance.currentPlayer.maxLevel = level;
            GameManager.Instance.SaveGame();
        }        
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
                    SaveProgress();
                }
                break;
            }
        }
        ui_Control.ViewLevel(li.Level);
        int slow = (slowCount > 0) ? slowSpeed : 0;
        if (spavnLeft == 0)
        {
            if (slowCount > 0)
            {
                slowCount--;
                if (slowCount == 0)
                {
                    timer += 4f;
                    ui_Control.ViewScherepacha(false);
                }
            }
            spLeft.SpawnNewCubes(li.CountDownCubesLeft, znCols, 3f + (level - 1) * 0.1f - slow, level > 10);
        }
        if (spavnRight == 0 && li.IsRight)
        {
            spRight.SpawnNewCubes(li.CountDownCubesRight, znCols, 3f + (level - 6) * 0.1f - slow, level > 10);
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
        if (cubes.Count == 0) 
        {
            GenerateCube(4);
        }
    }

    private void GenerateCube(int numPos)
    {
        countBonus++;
        GameObject cube;
        if (countBonus == spawnBonus)
        {
            cube = Instantiate(cubeBonusPrefab, listPositions[numPos], Quaternion.Euler(0, 0, 180));
        }
        else
        {
            cube = Instantiate(cubeSelPrefab, listPositions[numPos], Quaternion.identity);
        }        
        cube.GetComponent<CubeSelect>().SetParams(gameObject.GetComponent<LevelControl>(), numPos);
        CubeControl cc = cube.GetComponent<CubeControl>();
        if (countBonus == spawnBonus)
        {
            //int numBonusMat = Random.Range(0, arrBonusMaters.Length);
            int numBonusMat = Random.Range(0, 20);
            if (numBonusMat > 0) 
            {
                if (numBonusMat < 14) numBonusMat = 1;
                else numBonusMat = 2;
            }
            cc.SetColors(arrBonusMaters[numBonusMat], arrBonusMaters[numBonusMat], 6 + numBonusMat, 6 + numBonusMat, true);
        }
        else
        {
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
        }
        znCols[numPos] = cc.CubeColor;
        /*StringBuilder sb = new StringBuilder();
        for (int i = 0; i < znCols.Length; i++) sb.Append($"<{znCols[i]}>, ");
        print(sb.ToString());*/
        cubes.Add(cube);
        countBonus %= spawnBonus;
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
        bool isBomb = false;
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
                            //if (cc.CubeColor == cb.GetComponent<CubeControl>().CubeColor)
                            if (cc.CmpColor(cb.GetComponent<CubeControl>().CubeColor))
                            {
                                if (cc.CubeColor == 7) LivePlus(1);
                                if (cc.CubeColor == 8) isBomb = true;
                                if (cc.CubeColor == 9) { slowCount = 3; ui_Control.ViewScherepacha(true); }
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
                            //if (cc.CubeColor == cb.GetComponent<CubeControl>().CubeColor)
                            if (cc.CmpColor(cb.GetComponent<CubeControl>().CubeColor))
                            {
                                if (cc.CubeColor == 7) LivePlus(1);
                                if (cc.CubeColor == 8) isBomb = true;
                                if (cc.CubeColor == 9) { slowCount = 3; ui_Control.ViewScherepacha(true); }
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
                    if (isBomb) break;
                }
            }
            if (isBomb)
            { 
                delCubes.Clear();
                delNumPositions.Clear();
                if (cubesLeft != null)
                {
                    for (j = 0; j < cubesLeft.transform.childCount; j++)
                    {
                        GameObject go = cubesLeft.transform.GetChild(j).gameObject;
                        delCubes.Add(go);
                    }
                }
                if (cubesRight != null)
                {
                    for (j = 0; j < cubesRight.transform.childCount; j++)
                    {
                        GameObject go = cubesRight.transform.GetChild(j).gameObject;
                        delCubes.Add(go);
                    }
                }
                for (i = 0; i < cubes.Count; i++)
                {
                    GameObject cb = cubes[i];
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
                    //if (cc.CubeColor == cube.GetComponent<CubeControl>().CubeColor)
                    if (cc.CmpColor(cube.GetComponent<CubeControl>().CubeColor))
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
                    //if (cc.CubeColor == cube.GetComponent<CubeControl>().CubeColor)
                    if (cc.CmpColor(cube.GetComponent<CubeControl>().CubeColor))
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
        //print($"paramCubes={listCubes.Count} numbers={listNumbers.Count}");
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
        SaveProgress();
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
