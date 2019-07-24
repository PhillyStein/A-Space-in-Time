using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text typedText,
                untypedText,
                pointsText,
                finalText,
                gameOverText,
                restartText;

    public Rigidbody2D player;

    public PlayerController playerController;

    public int jumpSpeed,
                points = 0,
                lives = 3;

    private string typedChars,
                    untypedChars,
                    upperCaseText;

    private int typedCharsSize,
                untypedCharsSize,
                keyPos,
                level,
                sentenceNum;

    public KeyCode keyPressed,
                    keyToPress;
    
    private char keyToChar,
                 currentChar;

    public Image[] hearts;

    private bool gameOver;

    public bool winState;

    public GameObject gameOverPanel,
                        groundGroup;


    private KeyCode[] keyCodes = {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I,
                                    KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
                                    KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space,
                                    KeyCode.Comma, KeyCode.Period, KeyCode.Exclaim};

    private char[] charArray = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', ',', '.', '!'};

    private string[][] sentences = new string[2][];

    //private string[] levelOne = { "Level one is for kids", "It has easy words", "Like cat and dog", "If you fail on level one", "You are really bad", "You can not fail on level one", "I will tell everybody" },
    private string[] levelOne = { "Level one is for kids" },
                        levelTwo = { "Oh good", "You have made it", "This is level two", "Level two has full stops.", "Or periods.", "Whatever you call them.", "Level two has them.", "A lot of them.", "This is an ellipsis...", "People use them all the time.", "They use them incorrectly." },
                        levelThree = { "That level was too easy.", "Here comes a comma.", "Commas are cool, I guess.", "I guess level three is about commas.", "We should probably have more.", "Well, pal, the problem is...", "...these sentences are not...", "...very long.", "They have to fit along", "the bottom, here.", "Otherwise, you would not see", "what you should be tpying." };

    public static GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameOver = false;
        winState = false;

        level = 0;

        sentences[0] = levelOne;
        sentences[1] = levelTwo;

        typedChars = "";
        typedText.text = typedChars;
        sentenceNum = 0;
        untypedChars = sentences[0][0];
        untypedText.text = untypedChars;
        typedCharsSize = typedChars.Length;
        untypedCharsSize = untypedChars.Length;

        upperCaseText = untypedText.text.ToUpper();
        currentChar = upperCaseText[0];

        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        upperCaseText = untypedChars.ToUpper();
        if (upperCaseText.Length > 0)
        {
            currentChar = upperCaseText[0];
        }

        typedCharsSize = typedChars.Length;
        untypedCharsSize = untypedChars.Length;

        typedText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, typedCharsSize * 60);
        untypedText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, untypedCharsSize * 60);

        if (Input.anyKey)
        {
            OnGUI();
        }

        if(Input.GetKeyUp(KeyCode.Alpha1) && lives > 0)
        {
            lives--;
        }

        UpdateHearts();

        pointsText.text = points.ToString();
    }


    public void OnGUI()
    {
        Event e = Event.current;
        if (e != null)
        {
            if (e.isKey)
            {
                keyPressed = e.keyCode;

                if (containsKey(keyPressed) && !playerController.isFalling && !gameOver)
                {

                    keyPos = System.Array.IndexOf(keyCodes, keyPressed);
                    keyToChar = charArray[keyPos];

                    if (upperCaseText.Length > 0)
                    {
                        if (keyToChar == upperCaseText[0])
                        {

                            typedChars = typedChars + untypedChars[0];
                            //typedText.text = typedChars;
                            untypedChars = untypedChars.Substring(1, untypedChars.Length - 1);

                            upperCaseText = untypedChars.ToUpper();

                            if (upperCaseText.Length > 0)
                            {
                                currentChar = upperCaseText[0];
                            } else if(sentenceNum + 1 <= sentences[level].Length)
                            {
                                sentenceNum++;
                                //playerController.playerRB.transform.position = new Vector2(playerController.playerRB.transform.position.x + 5, playerController.playerRB.transform.position.y);
                                playerController.playerRB.transform.Translate(Vector2.right * 50 * Time.deltaTime, Space.World);

                                if (sentenceNum < sentences[level].Length)
                                {
                                    untypedChars = sentences[level][sentenceNum];
                                } else
                                {
                                    // LEVEL COMPLETE!
                                    /*
                                    finalText.text = "You scored " + points + " points.";
                                    gameOverText.text = "Congratulations. You have evaded the darkness.";
                                    restartText.text = "Press SPACE to continue.\nPress R to restart.";
                                    gameOverPanel.SetActive(true);
                                    winState = true;
                                    */
                                    level++;
                                    if(level < sentences.Length)
                                    {
                                        sentenceNum = 0;
                                        untypedChars = sentences[level][sentenceNum];

                                        finalText.text = "You scored " + points + " points.";
                                        
                                        gameOverText.text = "Congratulations. You have reached level " + level + "." ;
                                        restartText.text = "Press SPACE to continue.\nPress R to restart.";
                                        gameOverPanel.SetActive(true);
                                        winState = true;

                                        playerController.playerLag *= 10;
                                    } else
                                    {
                                        finalText.text = "You scored " + points + " points.";
                                        gameOverText.text = "Congratulations. You have evaded the darkness.";
                                        restartText.text = "Press SPACE to continue.\nPress R to restart.";
                                        gameOverPanel.SetActive(true);
                                        winState = true;
                                    }
                                }
                                typedChars = "";
                            }

                            typedText.text = typedChars;
                            untypedText.text = untypedChars;

                            if(keyPressed == KeyCode.Space)
                            {
                                playerController.canJump = true;
                                points += 100;
                            } else
                            {
                                points += 10;
                            }
                        }
                    }
                }

                if(gameOver || winState)
                {
                    if(Input.GetKeyUp(KeyCode.R))
                    {
                        winState = false;
                        gameOver = false;
                        SceneManager.LoadScene("Scene1");
                    }
                }

                if(winState)
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        //Go to next level
                        playerController.restartPlatforms();
                        gameOverPanel.SetActive(false);
                        winState = false;
                    }

                }
            }
        }
    }

    public bool containsKey(KeyCode keyToFind)
    {
        for(int i = 0; i < keyCodes.Length; i++)
        {
            if(keyCodes[i] == keyToFind)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateHearts()
    {
        for(int i = 0; i < 3; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        /* Uncomment this to add lives
        for(int i = 0; i < lives; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
        */
    }

    public void GameOver()
    {
        if (!winState)
        {
            finalText.text = "You scored " + points + " points.";
            gameOverText.text = "You have been consumed by the darkness.";
            restartText.text = "Press R to restart.";
            gameOverPanel.SetActive(true);
            gameOver = true;
        }
    }
}
