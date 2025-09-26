using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float immuneTime = 1f;
    float timer = 0f;

    void Update()
    {
        timer -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (timer <= 0)
        {
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    print("Keep going!");
                    break;
                case "Finish":
                    print("You've reached the end!");
                    break;
                case "Fuel":
                    print("This is a fuel object");
                    break;
                default:
                    print("You crashed!");
                    timer += immuneTime;
                    break;
            }
        }
    }
}
