using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D    rb;
    private SpriteRenderer sp;

    private readonly float MoveSpeed = 5.0f;
    private readonly float JumpPower = 9.0f;

    private bool isJump = false;

    private Animator an;

    private AudioSource snd;
    public  AudioClip   se_ItemGet;

    private GameManager game;


    // 初期化
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();

        snd = gameObject.AddComponent<AudioSource>();
        game = GameObject.Find("Main").GetComponent<GameManager>();
    }

    // 更新
    void Update()
    {
        if (game.Mode != 1) { return; }


        float x = Input.GetAxisRaw("Horizontal");

        if(x > 0)
		{   // 右
            rb.velocity = new Vector2(MoveSpeed, rb.velocity.y);
            sp.flipX = false;
            an.Play("Walk");
        }
        else if( x < 0)
        {   // 左
            rb.velocity = new Vector2(-MoveSpeed, rb.velocity.y);
            sp.flipX = true;
            an.Play("Walk");
        }
        else
		{
            an.Play("Stop");
        }

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
		{
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            isJump      = true;
        }

        // 壁処理
        CheckWall();
    }

    public LayerMask groundLayer;

    void CheckWall()
    {
        // 右
        Vector2 RPos  = new Vector2(transform.position.x + 0.5f, transform.position.y);
        Vector2 RArea = new Vector2(0.05f, 0.5f);
        bool isRWall  = Physics2D.Linecast(RPos + RArea, RPos - RArea, groundLayer);
        if (isRWall && rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        // 左
        Vector2 LPos  = new Vector2(transform.position.x - 0.5f, transform.position.y);
        Vector2 LArea = new Vector2(0.05f, 0.5f);
        bool isLWall  = Physics2D.Linecast(LPos + LArea, LPos - LArea, groundLayer);
        if (isLWall && rb.velocity.x < 0)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }


    // 当たり判定
    void OnCollisionEnter2D(Collision2D col)
	{
        // 地面
        if (col.gameObject.tag == "Ground")
        {
            Vector2 RPos  = new Vector2(transform.position.x, transform.position.y);
            Vector2 LArea = new Vector2(-0.3f, -0.7f);
            Vector2 RArea = new Vector2( 0.3f, -0.7f);
            bool isFloor  = Physics2D.Linecast(RPos + LArea, RPos + RArea, groundLayer);

            if (isFloor)
            {
                isJump = false;
            }
        }

        // アイテム
        if (col.gameObject.tag == "Item")
        {
            snd.PlayOneShot(se_ItemGet);
            game.Score++;
            col.gameObject.GetComponent<ItemManager>().Kill();
        }
    }
}



