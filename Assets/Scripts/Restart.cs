using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restart : GameButton
{
    private string scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene ().name;   
    }

    public override void Press()
    {
        StartCoroutine(SetSnapshot());
        StartCoroutine(Pause());
    }

    IEnumerator SetSnapshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        Data.snapshot = texture;
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.3F);
        SceneManager.LoadScene(scene);
    }
}
