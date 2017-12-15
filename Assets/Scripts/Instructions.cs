using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Instructions : GameButton
{
    public GameButton playEasy;
    public GameButton playNormal;
    public GameButton playHard;
    public GameButton done;
    public GameObject instructionsText;
    public AudioClip click;
    private AudioSource source;

    void Start ()
    {
        source = GetComponent<AudioSource> ();
    }

    public override void Press()
    {
        source.PlayOneShot (click);
        playEasy.GetComponent<Image> ().enabled = false;
        playNormal.GetComponent<Image>().enabled = false;
        playHard.GetComponent<Image>().enabled = false;
        GetComponent<Image>().enabled = false;
        done.GetComponent<Image>().enabled = true;
        instructionsText.SetActive (true);
    }

}
