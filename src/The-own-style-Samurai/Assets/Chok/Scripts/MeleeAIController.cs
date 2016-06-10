using UnityEngine;
using System.Collections;

public class MeleeAIController : MonoBehaviour {
    public static MeleeAIController singleton;

    int angle;

    public int Angle
    {
        get
        {
            angle += 2; angle %= 5;
            return angle;
        }
    }

    public void OnEnable()
    {
        singleton = this;
    }
}
