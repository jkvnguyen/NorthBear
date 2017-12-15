using UnityEngine;
using System.Collections;

public class FadeScriptBackground : MonoBehaviour
{
    public Texture2D fadeTexture;
    private GUITexture gText;
    private float fadeSpeed = 0.3f;
    private int fadeDir = -1;
    private float alpha = 1.0f;

    void Awake()
    {
        gText = GetComponent<GUITexture> ();
        gText.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        gText.texture = fadeTexture;
    }

    void Update()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        gText.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
    }
}
