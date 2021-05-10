using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    private float lifetime = 1.0f;      // 生存時間
    private GameObject effect;          // エフェクト
    private bool       isBomb = false;  // 爆発したか

    public PhysicsMaterial2D BombMat;

    private AudioSource snd;
    public AudioClip se_Bomb;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var v = new Vector2(0, -1);
        rb.AddForce(Quaternion.Euler(0, 0, Random.Range(-40.0f, 40.0f)) * v * 100.0f);

        snd = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isBomb) { return; }
        lifetime -= Time.deltaTime;     // 生存時間を減らしている
        if (lifetime < 0)
        {
            isBomb = true;

            Destroy(gameObject, 0.5f);  // タイムオーバーの時の消去

            snd.PlayOneShot(se_Bomb);

            CircleCollider2D cir = this.GetComponent<CircleCollider2D>();
            cir.radius *= 2.0f;
            cir.sharedMaterial = BombMat;

            // エフェクトを表示
            effect = this.transform.GetChild(0).gameObject;
            GameObject p = (GameObject)Instantiate(effect);
            p.SetActive(true);
            p.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.1f);
        }
    }
}
