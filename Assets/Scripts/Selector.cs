using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public AudioClip select;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    private AudioSource source;
    public GameButton easy;
    private Sprite playEasy;
    public Sprite playEasyHighlighted;
    public GameButton normal;
    private Sprite playNormal;
    public Sprite playNormalHighlighted;
    public GameButton hard;
    private Sprite playHard;
    public Sprite playHardHighlighted;
    public GameButton instr;
    private Sprite instructions;
    public Sprite instructionsHighlighted;
    public GameButton done;
    private GameButton[] buttons;
    private Sprite[] sprites;
    private Sprite[] spritesHighlighted;
    private int i;
    private bool onInstr;

	void Start ()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        source = GetComponent<AudioSource>();
        playEasy = easy.GetComponent<Image> ().sprite;
        playNormal = normal.GetComponent<Image> ().sprite;
        playHard = hard.GetComponent<Image> ().sprite;
        instructions = instr.GetComponent<Image> ().sprite;
        buttons = new GameButton[] {easy, normal, hard, instr};
        sprites = new Sprite[] {playEasy, playNormal, playHard, instructions};
        spritesHighlighted = new Sprite[] { playEasyHighlighted, playNormalHighlighted, playHardHighlighted, instructionsHighlighted };
        i = 0;
        onInstr = false;

        easy.GetComponent<Image> ().sprite = playEasyHighlighted;
    }

    void Update ()
    {
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (!onInstr)
        {
            if (controller.GetPressDown(touchPad))
            {
                Vector2 press = controller.GetAxis();

                if (press.y >= 0)
                {
                    source.PlayOneShot(select);

                    buttons[i].GetComponent<Image>().sprite = sprites[i];

                    i--;

                    if (i == -1)
                        i = 3;

                    buttons[i].GetComponent<Image>().sprite = spritesHighlighted[i];
                }
                else
                {
                    source.PlayOneShot(select);

                    buttons[i].GetComponent<Image>().sprite = sprites[i];

                    i = (i + 1) % 4;

                    buttons[i].GetComponent<Image>().sprite = spritesHighlighted[i];
                }

            }
            
        }

        if (controller.GetPressDown(triggerButton))
        {
            if (!onInstr)
            {
                buttons[i].Press();

                if (i == 3)
                    onInstr = true;

            }
            else
            {
                done.Press();

                onInstr = false;
            }

        }

    }

}