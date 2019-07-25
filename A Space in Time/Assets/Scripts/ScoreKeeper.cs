using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    public static int highScore;

    public bool isTutorial = true;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetHighScore(int score)
    {
        highScore = score;
    }

    public static int GetHighScore()
    {
        return highScore;
    }
}
