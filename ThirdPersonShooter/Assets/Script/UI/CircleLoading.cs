using UnityEngine;

public class CircleLoading : MonoBehaviour
{
    RectTransform loadingTransform;
    public float rotateSpeed = 200f;
    // Start is called before the first frame update
    void Start()
    {
        loadingTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        loadingTransform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
