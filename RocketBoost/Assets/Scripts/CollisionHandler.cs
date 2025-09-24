using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
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
                break;
        }
    }
}
