using UnityEngine;

public class CubeSelect : MonoBehaviour
{
    private int numPos = 0;
    private LevelControl levelControl;

    public int NumberPosition { get { return numPos; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetParams(LevelControl lc, int num)
    {
        levelControl = lc;
        numPos = num;
    }

    private void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (levelControl != null) levelControl.CubeSelect(gameObject);
        }
    }
}
