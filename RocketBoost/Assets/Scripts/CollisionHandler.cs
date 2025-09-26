using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float immuneTime = 1f;
    [SerializeField] int maxHealth = 5;
    [SerializeField] TextMeshProUGUI tMPro;
    float timer = 0f;
    float waitTime = 2f;
    int health;
    IEnumerator loadNextLevel;
    IEnumerator restartLevel;

    void Start()
    {
        health = maxHealth;
        loadNextLevel = LoadNextLevel(waitTime);
        restartLevel = ReloadLevel(waitTime);
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        tMPro.text = $"Health {health}"; // UI Health display
    }

    void OnCollisionEnter(Collision other)
    {
        if (timer <= 0) // If the timer is <= 0, we aren't "immune"
        {
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    print("Keep going!");
                    break;
                case "Finish": // We've reached the end, start next level
                    print("You've reached the end!");
                    StartCoroutine(loadNextLevel); 
                    break;
                case "Health": // Restore health
                    if (health <= 3) { health += 2; }
                    else { health = maxHealth; }
                    Destroy(other.gameObject);
                    break;
                default: // We hit an obstacle
                    print("You crashed!");
                    if (health > 1) { health--; } // Reduce health
                    else { StartCoroutine(restartLevel); } // Restart level if health would be 0

                    if (timer <= 0) { timer = immuneTime; }
                    break;
            }
        }
    }

    // This IEnumerator loads the next level after a specified wait time
    private IEnumerator LoadNextLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // Initialize build indices for current and next scene
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        // If we are in the last scene, make the next scene the 0th index in scene library
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        // Load next scene
        SceneManager.LoadScene(nextScene);
    }

    // This IEnumerator reloads the current level after a specified wait time
    private IEnumerator ReloadLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
