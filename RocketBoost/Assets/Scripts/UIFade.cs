using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    [SerializeField] float fadeSpeed = 1f;

    IEnumerator fadeRoutine;

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
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            // Now that our value is changed, we have to actually change the alpha of our fadescreen 
            // based on the new alpha that's being calculated every frame. 
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }
}
