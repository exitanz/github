using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject item, bomb;

    private float ItemInterval = 1.0f; // アイテムの出現時間
    private float itemTime = 0;        // アイテムの出現計算用

    private GameObject  title, over;
    private AudioSource snd;
    public  AudioClip   se_start, se_over;

    public int Mode = 0;
    public int Score;

    private float PlayTime = 0;
    private Text  ScoreText;
    private Text Key;
    private Text Info;


    void Start()
    {
        title = GameObject.Find("Title");
        over = GameObject.Find("Over");
        over.SetActive(false);
        snd = gameObject.AddComponent<AudioSource>();
        ScoreText = GameObject.Find("Text").GetComponent<Text>();
        Key = GameObject.Find("Text2").GetComponent<Text>();
        Info = GameObject.Find("Text3").GetComponent<Text>();
    }

    void Update()
    {
        switch (Mode)
        {
            case 0:
                Title();
                break;
            case 1:
                Game();
                break;
            case 2:
                Over();
                break;
        }
         ScoreText.text = "Time:" + Mathf.FloorToInt(PlayTime).ToString() + ", Score: " + Score.ToString();
    }

    private void Title()
    {
        Key.text = "ジャンプ[ Space ] / 移動[ WASD or ↑←↓→ ]" ;
        Info.text = "より多くのリンゴをゲットしよう！爆弾に当たると時間が減るぞ！";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            title.SetActive(false);
            snd.PlayOneShot(se_start);
            Mode++;

            Score    = 0;
            PlayTime = 30.0f;
            Key.text = "" ;
            Info.text = "";
        }
    }

    private void Over()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Mode = 0;
            over.SetActive(false);
            title.SetActive(true);
        }
    }


    private void Game()
    {
        PlayTime -= Time.deltaTime;
        if (PlayTime <= 0)
        {
            snd.PlayOneShot(se_over);
            over.SetActive(true);
            PlayTime = 0;
            Mode++;
            return;
        }

        itemTime -= Time.deltaTime;
        if (itemTime <= 0)                  // アイテムの出現時間が来たか?
        {                                   // アイテムを生成
            GameObject a = item;

            int b = Random.Range(0, 3);     // 0～2
            if (b <= 1) { a = bomb; }

            var obj = Instantiate(a, new Vector3(Random.Range(-3.28f, 3.97f), 5.8f, 0), Quaternion.identity);

            ItemInterval -= 0.01f;
            if (ItemInterval <= 0.2f) { ItemInterval = 0.2f; }

            itemTime = ItemInterval;
        }
    }
}
