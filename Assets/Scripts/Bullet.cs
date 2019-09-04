using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public float speed = 3f;
    public Vector3 initPos;
    public int damage = 10;
    public float rotationAngle = 0;
    public float maxDistance = 100;

    void Start()
    {
        
    }

    public void Launch(){
        transform.position = initPos;
        transform.rotation = Quaternion.Euler(new Vector3(90f, 270 - rotationAngle,0f));
        GetComponent<Rigidbody>().velocity = transform.up * speed;
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime(){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Enemy"){
            col.gameObject.GetComponent<Enemy>().hp -= damage;
            Destroy(gameObject);
        }
    }
}
