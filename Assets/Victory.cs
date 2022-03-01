using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{

    public Text VictoryText;

    public GlobalManager gm;

    private int ScoreNum;

    void Awake()
    {
        gm = GameObject.FindObjectOfType<GlobalManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ScoreNum = gm.score;
        VictoryText.text = "Congratulations, You Beat the Game" + '\n' + "Your score was: " + ScoreNum + '\n' + "Total Possible Score: 2000";
    }
}
