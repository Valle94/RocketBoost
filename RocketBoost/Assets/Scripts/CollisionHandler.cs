using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float immuneTime = 1f;
    [SerializeField] int maxHealth = 5;
    [SerializeField] float loadDelay = 2f;
    [SerializeField] TextMeshProUGUI tMPro;
    [SerializeField] AudioClip impactSound;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip healthPickupSound;
    [SerializeField] ParticleSystem crashFX;
    [SerializeField] ParticleSystem finishFX;
    [SerializeField] Image fadeScreen;

    AudioSource audioSource;
    IEnumerator fadeRoutine;
    Image loadScreen;

    float timer = 0f;
    int health;
    bool isControllable = true;
    bool isCollidable = true;


    void Start()
    {
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1f);
        FadeToClear();
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        tMPro.text = $"Health: {health}"; // UI Health display

        RespondToDebugKeys();
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable) { return; }

        if (timer <= 0) // If the timer is <= 0, we aren't "immune"
        {
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    print("Keep going!");
                    break;
                case "Finish": // We've reached the end, start next level
                    print("You've reached the end!");
                    FinishSequence();
                    break;
                case "Health": // Restore health
                    if (health <= 3) { health += 2; }
                    else { health = maxHealth; }
                    audioSource.PlayOneShot(healthPickupSound);
                    Destroy(other.gameObject);
                    break;
                default: // We hit an obstacle
                    print("You crashed!");
                    if (health > 1) {
                        audioSource.PlayOneShot(impactSound);
                        health--; } // Reduce health
                    else
                    {
                        health--;
                        StartCrashSequence();
                    } // Restart level if health would be 0

                    if (timer <= 0) { timer = immuneTime; }
                    break;
            }
        }
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            StartCoroutine(LoadNextLevel(0));
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }

    private void FinishSequence()
    {
        FadeToBlack();
        isControllable = false;
        finishFX.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        GetComponent<Movement>().enabled = false;
        StartCoroutine(LoadNextLevel(loadDelay)); ;
    }

    private void StartCrashSequence()
    {
        FadeToBlack();
        isControllable = false;
        crashFX.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        GetComponent<Movement>().enabled = false;
        StartCoroutine(ReloadLevel(loadDelay));
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
    
        // The following two methods stop our fade coroutine if it's already running
    // and start it with a value of 0 or 1 depending on if we're fading in or out.
    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }

    // This coroutine will handle the fading effect when we transition scenes by 
    // adjusting the alpha of our fade screen image. 
    public IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            // This like will move our alpha value of the fade screen towards the target alpha value 
            // at the specified rate. The target will be 1 for fading out, and 0 for fading in. 
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, loadDelay * Time.deltaTime);
            // Now that our value is changed, we have to actually change the alpha of our fadescreen 
            // based on the new alpha that's being calculated every frame. 
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }
}
