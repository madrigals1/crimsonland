using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public Sprite[] animation;
    private SpriteRenderer spr;
    public float wait_time = 0.1f;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        StartCoroutine("Animate");
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < animation.Length; i++)
        {
            spr.sprite = animation[i];
            yield return new WaitForSeconds(wait_time);
        }

        Destroy(gameObject);
    }
}
