using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float xRotation = 0f;
    [SerializeField] float yRotation = 0f;
    [SerializeField] float zRotation = 0f;

    void Update()
    {
        gameObject.transform.Rotate(xRotation * Time.deltaTime,
                 yRotation * Time.deltaTime, zRotation * Time.deltaTime);
    }
}
