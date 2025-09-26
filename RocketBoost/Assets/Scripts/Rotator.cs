using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Variables for rotation
    [SerializeField] float xRotation = 0f;
    [SerializeField] float yRotation = 0f;
    [SerializeField] float zRotation = 0f;

    void Update()
    {
        Rotate();
    }

    // This method rotates the gameObject by a frame-rate independent amount
    private void Rotate()
    {
        float x = xRotation * Time.deltaTime;
        float y = yRotation * Time.deltaTime;
        float z = zRotation * Time.deltaTime;
        gameObject.transform.Rotate(x, y, z);
    }
}
