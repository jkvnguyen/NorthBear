using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Done : GameButton
{
    public GameButton playEasy;
    public GameButton playNormal;
    public GameButton playHard;
    public GameButton instructions;
    public GameObject instructionsText;
    public AudioClip click;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        GetComponent<Image>().enabled = false;
    }

    public override void Press()
    {
        source.PlayOneShot(click);
        playEasy.GetComponent<Image>().enabled = true;
        playNormal.GetComponent<Image>().enabled = true;
        playHard.GetComponent<Image>().enabled = true;
        instructions.GetComponent<Image>().enabled = true;
        GetComponent<Image>().enabled = false;
        instructionsText.SetActive(false);
    }

}
