using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public float sceneLoadDelay = 3f;
    public CanvasGroup sceneFadeOverlay;
    public float sceneFadeDuration = 0f;

    public void Start()
    {
        if (sceneFadeOverlay != null) sceneFadeOverlay.alpha = 0f;
    }

    public void LoadScene (string scene)
    {
        StartCoroutine(LoadSceneDelay(scene));
    }

    public void RestartScene ()
    {
        StartCoroutine(LoadSceneDelay(SceneManager.GetActiveScene().name));
    }

    IEnumerator LoadSceneDelay (string sceneName)
    {
        yield return new WaitForSeconds(sceneLoadDelay - sceneFadeDuration);

        float timer = 0f;

        while (timer < 1f)
        {
            if (sceneFadeOverlay != null) sceneFadeOverlay.alpha = Mathf.Lerp(0f, 1f, timer / sceneFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        if (sceneFadeOverlay != null) sceneFadeOverlay.alpha = 1f;

        SceneManager.LoadScene(sceneName);
    }
}
