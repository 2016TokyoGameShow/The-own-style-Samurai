using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {

    [SerializeField]
    private Player mPlayer;
    [SerializeField]
    private UIController mUiController;
    [SerializeField]
    private ParticleController mParticleController;

    public Player player { get { return mPlayer; }}
    public UIController uiController { get { return mUiController;}}
    public ParticleController particleController { get { return mParticleController; } }
	void Start () {
	
	}
	

	void Update () {
	
	}
}
