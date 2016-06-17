using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NextDoorOpen : MonoBehaviour {
    [SerializeField]
    private StageController stageController;
    [SerializeField]
    private GameObject[] openDoors;
    [SerializeField]
    private Animator[]openDoorsAnime;
    [SerializeField]
    private int border;
    [SerializeField]
    private GameObject enemyGenerator;



    private EnemyController enemycontroller;
    // Use this for initialization
    void Start()
    {
        enemycontroller = stageController.enemycontroller;
        openDoorsAnime[0] = openDoors[0].GetComponent<Animator>();
        openDoorsAnime[1] = openDoors[1].GetComponent<Animator>();
        openDoorsAnime[2] = openDoors[2].GetComponent<Animator>();
        openDoorsAnime[3] = openDoors[3].GetComponent<Animator>();

    }


	
	// Update is called once per frame
	void Update () {
        if (border <= enemycontroller.enemyDeathCount)
        {
            Debug.Log("go");
            StartCoroutine(OpenDoor());
            

        }
	}
    public IEnumerator OpenDoor()
    {
        Debug.Log("first");
        if(openDoorsAnime[0]!=null)openDoorsAnime[0].SetBool("open", true);
        if (openDoorsAnime[1] != null) openDoorsAnime[1].SetBool("open", true);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("second");
        GameObject.Destroy(openDoors[0]);
        GameObject.Destroy(openDoors[1]);
        if (openDoors[2] != null) openDoorsAnime[2].SetBool("open", true);
        if (openDoors[3] != null) openDoorsAnime[3].SetBool("open", true);
        yield return new WaitForSeconds(1.0f);
        GameObject.Destroy(openDoors[2]);
        GameObject.Destroy(openDoors[3]);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("ok");
    }
}
