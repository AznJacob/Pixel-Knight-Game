using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public Text MyscoreText;
    public int ScoreNum;

    public GlobalManager gm;
    private LevelLoader levelLoader;

    void Awake()
    {
        gm = GameObject.FindObjectOfType<GlobalManager>();
        //Get LevelLoader
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ScoreNum = gm.score;
        MyscoreText.text = "Score: " + ScoreNum;
    }

    private void OnTriggerEnter2D(Collider2D Collectable)
    {
        if(Collectable.tag == "MyCoin")
        {
            ScoreNum += 20;
            Destroy(Collectable.gameObject);
            changeScore();
            FindObjectOfType<AudioManager>().Play("SmallCoin");
        }
        else if(Collectable.tag == "Treasure")
        {
            ScoreNum += 100;
            Destroy(Collectable.gameObject);
            changeScore();
            FindObjectOfType<AudioManager>().Play("Treasure");
        }
        else if (Collectable.tag == "EndLevel")
        {
            FindObjectOfType<AudioManager>().Play("levelComplete");
            saveScore();
            nextLevel();
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "EndLevel")
        {
            saveScore();
            nextLevel();
        }
    }*/

    private void changeScore()
    {
        MyscoreText.text = "Score: " + ScoreNum;
    }

    public void saveScore()
    {
        gm.score = ScoreNum;
    }

    public void nextLevel()
    {
        levelLoader.LoadNextLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
