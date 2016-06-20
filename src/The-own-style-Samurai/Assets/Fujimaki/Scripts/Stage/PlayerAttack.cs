﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject enemyTarget;
    [SerializeField]
    private GameObject spawnAttackArea;

    public GameObject cameraBase;

    public GameObject mainCamera;

    public GameObject[] zoomPositions;

    public bool playerAttacking;
    public bool playerAttackingOnce;
    public int reciveRightLeft;
    public Vector3 playerAttackingVelocity;

    private List<GameObject> targets = new List<GameObject>();

    void Update()
    {

        /*  if (Input.GetKeyDown(KeyCode.UpArrow))
          {
              StartCoroutine(Attack(player.GetCameraRig().transform.forward,Vector3.up));
          }
          if (Input.GetKeyDown(KeyCode.DownArrow))
          {
              StartCoroutine(Attack(-player.GetCameraRig().transform.forward,-Vector3.up));
          }*/


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Attack(player.GetCameraRig().transform.right, Vector3.right));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(Attack(-player.GetCameraRig().transform.right, -Vector3.right));
        }
    }

    //流し攻撃
    private IEnumerator Attack(Vector3 velocity, Vector3 localVelocity)
    {



        if ((!playerAttacking) && (enemyTarget != null))
        {

            player.UpFinisherGage(0.1f);
            StartCoroutine(CameraMove());

           // player.ChangeColor(Color.red);
            player.nonMove = true;
            playerAttacking = true;
            playerAttackingOnce = true;

            playerAttackingVelocity = localVelocity;

            Vector3 enemyTargetPositon = enemyTarget.transform.position;
            player.transform.position = new Vector3(player.transform.position.x, enemyTarget.transform.position.y, player.transform.position.z);

            player.GetAnimator().SetInteger("katana", localVelocity.x > 0 ? 1 : 2);
            mainCamera.GetComponent<DepthOfField>().focalTransform = player.gameObject.transform;

            Instantiate(spawnAttackArea, (new Vector3(velocity.x * 2, 0.5f, 0) + player.transform.position), transform.rotation);

            while (Vector3.Angle(player.transform.forward, enemyTargetPositon - player.transform.position) != 0)
            {

                Quaternion rotation = Quaternion.LookRotation(enemyTargetPositon - player.transform.position);
                rotation.x = 0;
                rotation.z = 0;
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, 0.5f);
                yield return new WaitForEndOfFrame();
            }
           


            if (enemyTarget != null) Attack();

            yield return new WaitForSeconds(0.5f);

            playerAttacking = false;
            player.ChangeColor(Color.white);
            player.nonMove = false;
            playerAttackingOnce = false;
            playerAttackingVelocity = Vector3.zero;

            player.GetAnimator().SetInteger("katana", 0);
        }
    }

    //カメラ演出
    private IEnumerator CameraMove()
    {
        float counter = 0;
        float speed = 5;

        int zoomObjectNum = Random.Range(0, zoomPositions.Length);

        while (counter < 1)
        {
            counter += Time.deltaTime * speed;
            mainCamera.transform.localPosition = Vector3.Lerp(Vector3.zero, zoomPositions[zoomObjectNum].transform.localPosition, counter);
            mainCamera.transform.localRotation = Quaternion.Lerp(Quaternion.identity, zoomPositions[zoomObjectNum].transform.localRotation, counter);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.8f);


        while (counter > 0)
        {
            counter -= Time.deltaTime * speed;
            mainCamera.transform.localPosition = Vector3.Lerp(Vector3.zero, zoomPositions[zoomObjectNum].transform.localPosition, counter);
            mainCamera.transform.localRotation = Quaternion.Lerp(Quaternion.identity, zoomPositions[zoomObjectNum].transform.localRotation, counter);
            yield return new WaitForEndOfFrame();
        }
        mainCamera.GetComponent<DepthOfField>().focalTransform = null;
    }

    //ダメージを受ける
    public void Hit(int damage)
    {
        enemyTarget = null;
        print("PlayerDamage");
    }

    //攻撃してくるターゲットを指定
    public void SetEnemyTarget(GameObject g)
    {
        enemyTarget = g;
    }

    public GameObject GetEnemyTarget()
    {
        return enemyTarget;
    }


    public bool getEnemyTarget()
    {
        return enemyTarget == null;
    }

    private void Attack()
    {

        foreach(var e in targets)
        {
            ExecuteEvents.Execute<WeaponHitHandler>(
            e,
            null,
            (_object, _event) => { _object.OnWeaponHit(1, this.gameObject); }
            );
        }

        targets.Clear();

        enemyTarget = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Enemy")&&(playerAttacking))
        {
            targets.Add(other.gameObject);
        }
        print("Targets By "+other.gameObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag == "Enemy") && (playerAttacking))
        {
            targets.Remove(other.gameObject);
        }
    }
}
        
