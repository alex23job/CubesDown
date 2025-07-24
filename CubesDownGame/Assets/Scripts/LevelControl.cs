using NUnit.Framework;
using System.Collections.Generic;
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

    private float timer = 2f;
    private int countLive = 3;
    private int spavnLeft = 0;
    private int spavnRight = 0;

    private int level = 1;
    private int score = 0;

    private GameObject cubesLeft = null;
    private GameObject cubesRight = null;
    private List<GameObject> cubes = new List<GameObject>();

    private List<LevelInfo> listLevels = new List<LevelInfo>();
    private List<Vector3> listPositions = new List<Vector3>();

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
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            timer = 2f;
            LiveMinus();
            spavnLeft++;
            spavnRight++;
            if (spavnLeft == 4)
            {
                spavnLeft = 0;                
            }
            if (spavnRight == 8)
            {
                spavnRight = 0;
            }
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
        listLevels.Add(new LevelInfo(17, 550, 8, 7, 7, true));
        listLevels.Add(new LevelInfo(18, 600, 8, 8, 7, true));
        listLevels.Add(new LevelInfo(19, 700, 8, 8, 8, true));
        listLevels.Add(new LevelInfo(20, 800, 9, 8, 8, true));
    }

    private void CreatePositions()
    {
        listPositions.Clear();
        listPositions.Add(new Vector3(0, 1f, 0));
        listPositions.Add(new Vector3(0, 1f, 1.1f));
        listPositions.Add(new Vector3(0, 1f, -1.1f));
        listPositions.Add(new Vector3(-2.4f, 1f, 1.1f));
        listPositions.Add(new Vector3(2.4f, 1f, -1.1f));
        listPositions.Add(new Vector3(2.4f, 1f, 1.1f));
        listPositions.Add(new Vector3(-2.4f, 1f, -1.1f));
        listPositions.Add(new Vector3(-2.4f, 1f, 0));
        listPositions.Add(new Vector3(2.4f, 1f, 0));
    }

    private void LiveMinus()
    {
        if (countLive > 0)
        {
            countLive--;
            GameObject heartLive = Instantiate(heartPrefab, new Vector3(6f, 1f, 4.5f), Quaternion.identity);
            heartLive.GetComponent<HeartControl>().SetDown();
            Destroy(heartLive, 3f);
        }
        else
        {
            heart.SetActive(false);
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
                break;
            }
        }
        ui_Control.ViewLevel(li.Level);

        if (spavnLeft == 0)
        {
            spLeft.SpawnNewCubes(li.CountDownCubesLeft, 2f + (level - 1) * 0.1f, level > 10);
        }
        if (spavnRight == 0 && li.IsRight)
        {
            spRight.SpawnNewCubes(li.CountDownCubesRight, 2f + (level - 6) * 0.1f, level > 10);
        }
    }

    private void GenerateSelectedCubes()
    {
        LevelInfo li = listLevels[level - 1];
        if (cubes.Count < li.CountSelectedCubes)
        {
            for (int i = 0; i < li.CountSelectedCubes; i++) 
            {
                CubeSelect cs = null;
                if (i < cubes.Count) cs = cubes[i].GetComponent<CubeSelect>();
                if (cs == null)
                {
                    GenerateCube(i);
                }
                else if (cs.NumberPosition != i)
                {
                    GenerateCube(i);
                }
            }
        }
        if (cubes.Count == 0) 
        {
            GenerateCube(0);
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
            cube.GetComponent<CubeControl>().SetColors(arrMaters[numMat1], arrMaters[numMat2], numMat1, numMat2);
        }
        else
        {
            cube.GetComponent<CubeControl>().SetColors(arrMaters[numMat1], arrMaters[numMat1], numMat1, numMat1);
        }

        cubes.Insert(numPos, cube);
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
        if (cubesLeft != null)
        {
            float cy = cube.transform.position.y;
            for (int i = 0; i < cubesLeft.transform.childCount; i++)
            {
                GameObject go = cubesLeft.transform.GetChild(i).gameObject;
                CubeControl cc = go.GetComponent<CubeControl>();
                if (Mathf.Abs(go.transform.position.y - cy) < 0.4f)
                {
                    if (cc.CubeColor == cube.GetComponent<CubeControl>().CubeColor)
                    {
                        score++;
                        ui_Control.ViewScore(score);
                        GameObject effect = Instantiate(effectPrefab, cube.transform.position, Quaternion.identity);
                        Destroy(effect, 0.5f);
                        cubes.RemoveAt(cube.GetComponent<CubeSelect>().NumberPosition);
                        Destroy(cube);
                        Invoke("GenerateSelectedCubes", 1f);

                        break;
                    }
                }
            }
        }
        //print(cube.name);
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
