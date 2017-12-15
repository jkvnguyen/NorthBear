using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OnLoad : MonoBehaviour
{
    private Texture2D fadeTexture = Data.snapshot;
    private float fadeSpeed = 0.3f;
    private int drawDepth = -1000;
    private int fadeDir = -1;
    private float alpha = 1.0f;

    void OnGUI ()
    {
        Fade ();
    }

    private void Fade ()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        GUI.Label(new Rect(200, 100, 1000, 1000), "Test");
    }

    void Awake()
    {
        SceneManager.sceneLoaded += onLoad;
    }

    void onLoad (Scene scene, LoadSceneMode mode)
    {
        BeginFade();
    }

    public float BeginFade()
    {
        return (fadeSpeed);
    }

}
