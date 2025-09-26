using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float immuneTime = 1f;
    [SerializeField] int health = 5;
    [SerializeField] TextMeshProUGUI tMPro;
    float timer = 0f;
    float waitTime = 2f;
    IEnumerator loadNextLevel;
    IEnumerator restartLevel;

    void Start()
    {
        loadNextLevel = LoadNextLevel(waitTime);
        restartLevel = ReloadLevel(waitTime);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        tMPro.text = $"Health {health}";
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
                    StartCoroutine(loadNextLevel);
                    break;
                case "Fuel":
                    print("This is a fuel object");
                    break;
                default:
                    print("You crashed!");
                    if (health > 1)
                    {
                        health--;
                    }
                    else
                    {
                        StartCoroutine(restartLevel);
                    }

                    if (timer <= 0)
                    {
                        timer = immuneTime;
                    }
                    break;
            }
        }
    }

    private IEnumerator LoadNextLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator ReloadLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
