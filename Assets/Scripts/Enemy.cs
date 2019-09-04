using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 1f;
    Rigidbody rb;
    CharacterController cc;
    public int hp = 200;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Die();
        SetOnY();
    }

    void Die(){
        if(hp <= 0){
            Destroy(gameObject);
            Values.enemyCount++;
            // Values.player.SpawnEnemy();
        }
    }

    void SetOnY(){
        Vector3 pos = transform.position;
        if(pos.y != 1){
            transform.position = new Vector3(pos.x, 1, pos.z);
        }
    }

    void Move(){
        MoveTowardsTarget(Values.playerPosition);
    }

    void MoveTowardsTarget(Vector3 target) {
        var offset = target - transform.position;
        if(offset.magnitude > .1f) {
            offset = offset.normalized * speed;
            cc.Move(offset * Time.deltaTime);
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Player"){
            //Destroy(col.gameObject);
            Vector3 dir = col.contacts[0].point - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody>().AddForce(dir*10);
        }
        if(col.gameObject.tag == "Enemy"){
            Vector3 dir = col.contacts[0].point - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody>().AddForce(dir*10);
        }
    }
}
