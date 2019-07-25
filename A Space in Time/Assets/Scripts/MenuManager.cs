using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private int highScore;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        highScore = ScoreKeeper.GetHighScore();
        scoreText.text = "High score: " + highScore;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyUp(KeyCode.T))
        {
            LoadGame(true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            LoadGame(false);

        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    public void LoadGame(bool isTut)
    {
        ScoreKeeper.instance.isTutorial = isTut;
        SceneManager.LoadScene("Scene1");
    }

}
