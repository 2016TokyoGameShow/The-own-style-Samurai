using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {

    [SerializeField]
    private Player mPlayer;
    [SerializeField]
    private UIController mUiController;

    public Player player { get { return mPlayer; }}
    public UIController uiController { get { return mUiController;}}

	void Start () {
	
	}
	

	void Update () {
	
	}
}
