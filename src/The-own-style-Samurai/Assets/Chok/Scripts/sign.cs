using UnityEngine;
using System.Collections;

public class sign : MonoBehaviour {

    public float lifeTime;
    private Transform m_camera;     //メインカメラー
    private Animator animator;

	// Use this for initialization
	void Start () {
        StartCoroutine(Dead());
        animator = this.GetComponent<Animator>();
        m_camera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Animation();
    }

    private void Rotate()
    {
        //カメラに向くように
        float dirX = m_camera.position.x - transform.position.x;
        float dirZ = m_camera.position.z - transform.position.z;
        float angle = Mathf.Atan2(dirX, dirZ);
        angle *= 180.0f / Mathf.PI;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    private void Animation()
    {
        //アニメーションのスピード調整
        animator.speed = 0.5f;
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyObject(gameObject);
    }
}
