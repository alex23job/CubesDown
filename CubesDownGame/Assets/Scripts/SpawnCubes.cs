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

    public void SpawnNewCubes(int count, float speed = 1f)
    {
        if (parent != null) DelParent();
        speedParent = speed;

        parent = new GameObject("Parent");
        parent.transform.position = transform.position;
        for (int i = 0; i < count; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.transform.parent = parent.transform;
            //float z = (i > 0 ? 0.1f : 0) + i;
            cube.transform.localPosition = new Vector3(0, 0, i * 1.1f);
            int numMat1 = Random.Range(0, arrMaters.Length);
            int numMat2 = Random.Range(0, arrMaters.Length);
            cube.GetComponent<CubeControl>().SetColors(arrMaters[numMat1], arrMaters[numMat2], numMat1, numMat2);
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
