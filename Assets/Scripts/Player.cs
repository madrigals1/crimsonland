using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bullet;
    public Enemy enemy;
    public float speed = 1f;
    public float playerBulletCooldown = 0.25f;
    Rigidbody rb;
    CharacterController cc;
    Transform head, gun1, gun2, bulletHolder, enemyHolder;
    Camera playerCamera;
    float rotationAngle;
    float shootTimedelta = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        head = transform.GetChild(0).transform;
        gun1 = head.GetChild(0).transform;
        gun2 = head.GetChild(1).transform;
        playerCamera = transform.GetChild(1).GetComponent<Camera>();
        bulletHolder = GameObject.Find("Bullet Holder").transform;
        enemyHolder = GameObject.Find("Enemy Holder").transform;
        Values.player = GetComponent<Player>();
        Physics.IgnoreLayerCollision(9, 9);
        SpawnEnemies();
    }

    void Update()
    {
        Move();
        RotateHead();
        Shoot();
        SetPlayerPosition();
        SetOnY();
        if(Values.enemyCount > 0){
            SpawnEnemy();
        }
    }

    void FixedUpdate(){
        ButtonPress();
    }

    void SetOnY(){
        Vector3 pos = transform.position;
        if(pos.y != 1){
            transform.position = new Vector3(pos.x, 1, pos.z);
        }
    }

    void SetPlayerPosition(){
        Values.playerPosition = transform.position;
    }

    public void SpawnEnemies(){
        while(Values.enemyCount > 0){
            SpawnEnemy();
        }
    }

    void ButtonPress(){
        if( Input.GetKeyDown(KeyCode.X) ) {
            playerBulletCooldown = 0.05f;
        }
    }

    public void SpawnEnemy(){
        bool enposReady = false;
        Vector3 enpos = new Vector3(0,0,0);
        while(!enposReady){
            enpos = new Vector3(Random.Range(-50,50), 1, Random.Range(-50, 50));
            if(Vector3.Distance(enpos, transform.position) <= 15){
                enposReady = false;
            } else {
                enposReady = true;
            }
        }
        if(enposReady){
            Debug.Log(enpos);
            Enemy enemyIns = Instantiate(enemy, enemyHolder);
            enemyIns.transform.position = enpos;
            Values.enemyCount--;
        }
    }

    void Move(){
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        cc.Move(moveDirection * speed);
    }

    void Shoot(){
        shootTimedelta += Time.deltaTime;
        if(Input.GetButton("Fire1") && shootTimedelta >= playerBulletCooldown){
            Bullet bulletIns = Instantiate(bullet, bulletHolder);
            Bullet bulletIns2 = Instantiate(bullet, bulletHolder);

            bulletIns.rotationAngle = rotationAngle;
            bulletIns2.rotationAngle = rotationAngle;

            bulletIns.initPos = gun1.position;
            bulletIns2.initPos = gun2.position;

            bulletIns.Launch();
            bulletIns2.Launch();

            shootTimedelta = 0;
        }
    }

    void RotateHead() {
		Vector2 positionOnScreen = new Vector2 (head.position.x, head.position.z);
        Vector3 mouseOnScreen = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseOnScreen2D = new Vector2 (mouseOnScreen.x, mouseOnScreen.z);
         
        rotationAngle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen2D);
 
        head.rotation = Quaternion.Euler (new Vector3(0f,270-rotationAngle,0f));
	}

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
