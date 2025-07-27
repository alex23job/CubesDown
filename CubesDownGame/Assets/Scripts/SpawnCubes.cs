using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    [SerializeField] private Material[] arrMaters;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private LevelControl levelControl;
    [SerializeField] private bool isLeft = true;

    private GameObject parent = null;
    private float speedParent = 1f;
    private float timeLive = 50f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (parent != null) 
        {
            Vector3 pos = parent.transform.position;
            pos.z -= speedParent * Time.deltaTime;
            parent.transform.position = pos;
        }
    }

    public void SpawnNewCubes(int count, int[] arr, float speed = 1f, bool isTwoColors = false)
    {
        if (parent != null) DelParent();
        speedParent = speed;
        List<int> list = new List<int>();
        int i, countFromArr = 0;
        for (i = 0; i < arr.Length; i++)
        {
            if (arr[i] != -1)
            {
                if (isLeft)
                {
                    if (i < 6) list.Add(arr[i]);
                    //if (i < 4 || i == 6 || i == 7) list.Add(arr[i]);
                }
                else
                {
                    if (i > 2) list.Add(arr[i]);
                    //if (i < 3 || i == 4 || i == 5 || i == 8) list.Add(arr[i]);
                }
            }
        }
        parent = new GameObject("Parent");
        parent.transform.position = transform.position;
        for (i = 0; i < count; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.transform.parent = parent.transform;
            //float z = (i > 0 ? 0.1f : 0) + i;
            cube.transform.localPosition = new Vector3(0, 0, i * 1.1f);
            int numMat1 = Random.Range(0, arrMaters.Length);
            int numMat2 = Random.Range(0, arrMaters.Length);
            int rnd = Random.Range(0, 2);
            if (rnd == 0 || (rnd == 1 && i > 1))
            {
                if (countFromArr < 2 && list.Count > 0)
                {
                    if (list[0] > 10)
                    {
                        numMat1 = (list[0] / 10) - 1;
                        numMat2 = (list[0] % 10) - 1;
                    }
                    else
                    {
                        numMat1 = list[0] - 1;
                        numMat2 = numMat1;
                    }
                    list.RemoveAt(0);
                    countFromArr++;
                }
            }
            if (isTwoColors)
            {
                cube.GetComponent<CubeControl>().SetColors(arrMaters[numMat1], arrMaters[numMat2], numMat1, numMat2);
            }
            else
            {
                cube.GetComponent<CubeControl>().SetColors(arrMaters[numMat1], arrMaters[numMat1], numMat1, numMat1);
            }
            Destroy(cube, timeLive - 1);
        }
        Destroy(parent, timeLive);
        if (levelControl != null)
        {
            levelControl.SendParent(parent, isLeft);
        }
    }

    private void DelParent()
    {
        int count = parent.transform.childCount;
        for (int i = 0;i < count;i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
        Destroy(parent);
    }
}
