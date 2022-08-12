using UnityEngine;

public class test_mono : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) print("up");
        if (Input.GetKey(KeyCode.DownArrow)) print("down");

        if (Input.GetKey(KeyCode.RightArrow)) print("right");
        if (Input.GetKey(KeyCode.LeftArrow)) print("left");

    }
}
