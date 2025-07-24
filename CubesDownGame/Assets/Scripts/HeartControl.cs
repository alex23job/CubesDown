using UnityEngine;

public class HeartControl : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private bool isDown = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDown)
        {
            Vector3 pos = transform.position;
            pos.z -= speed * Time.deltaTime;
            transform.position = pos;
        }
    }

    public void SetDown()
    {
        isDown = true;
    }
}
