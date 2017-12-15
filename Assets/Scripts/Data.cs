using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour
{
    public const int EASY = 0;
    public const int NORMAL = 1;
    public const bool CROUCHED = true;
    public const bool UNCROUCHED = false;
    public const int HARD = 2;
    public static int difficulty;
    public static bool crouched;
    public static Texture2D snapshot;
}
