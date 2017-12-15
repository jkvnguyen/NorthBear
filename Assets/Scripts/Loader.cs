using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loader : MonoBehaviour
{
	public GameObject player;
    private Vector3 playerPos;
    private Vector3 tilePos;

    void Start ()
    {
        playerPos = player.transform.position;
        tilePos = GetComponent<Transform> ().position;
    }

    void Update ()
    {
        playerPos = player.transform.position;
        Debug.Log("Player: " + playerPos.y + ", Tile: " + tilePos.y);
        if (playerPos.y <= tilePos.y)
        {
            Debug.Log("Warp");
            StartCoroutine(SetSnapshot());
            StartCoroutine(Pause());
        }

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
        SceneManager.LoadScene("Room1");
    }

}
