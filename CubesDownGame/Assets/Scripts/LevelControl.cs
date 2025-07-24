using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject cubePrefab;
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
    private GameObject[] cubes = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (spavnLeft == 0)
        {
            spLeft.SpawnNewCubes(5,3f);
        }
        if (spavnRight == 0)
        {
            spRight.SpawnNewCubes(5, 2f);
        }
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
}
