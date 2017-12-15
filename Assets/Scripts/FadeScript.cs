using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour
{
    public AudioClip link;
    private GUITexture gText;
    private Texture2D fadeTexture = Data.snapshot;
    private float fadeSpeed = 0.3f;
    private int fadeDir = -1;
    private float alpha = 1.0f;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(link);
        gText = GetComponent<GUITexture> ();
        gText.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        gText.texture = fadeTexture;
        gText.pixelInset = (new Rect(0, 0, -525, -850));
    }

    void Update()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        gText.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
    }
}
