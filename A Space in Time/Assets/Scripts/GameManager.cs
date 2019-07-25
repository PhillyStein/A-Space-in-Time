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
                difficulty = 0,
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

    private bool gameOver,
                canJump;

    public bool winState,
                fogMoving,
                isPaused,
                isTutorial,
                gameStarted;

    public GameObject gameOverPanel,
                        pauseMenu,
                        groundGroup;


    private KeyCode[] keyCodes = {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I,
                                    KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
                                    KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space,
                                    KeyCode.Comma, KeyCode.Period, KeyCode.Minus};

    private char[] charArray = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', ',', '.', '-' };

    public string[,] sentences = {
                                    { "Type the characters you see", "Well done", "You have mastered the controls", "Welcome to A Space in Time", "You may be wondering", "where the gameplay is.", "Patience, young grasshopper.", "You are just getting warmed up.", "The space bar is a special key.", "Whenever you press it" },
                                    { "You will jump.", "But only if you press it", "when it is a part", "of the sentence.", "Just time your jumps.", "To avoid the dangers.", "a a", "b b", "c c", "d d" },
                                    { "Now jump.", "Good, but there is more.", "e e", "f f", "g g", "h h", "i i", "j j", "k k", "l l" },
                                    { "Here comes the darkness.", "It will slowly creep up on you.", "So try not to hesitate.", "Otherwise you will perish.", "That would be bad", "It could be worse.", "It starts off slow", "But it will get faster.", "m m", "n n" },
                                    { "Much faster.", "o o", "p p", "q q", "r r", "s s", "t t", "u u", "v v", "w w"}
    };

    string[,] multiDimensionalArray2 = { { "1", "2", "3" }, { "4", "5", "6" }, {"h", "7", "7"} };

    public string[] levelOne = { "Type the characters you see", "Well done", "You have mastered the controls", "Welcome to A Space in Time", "You may be wondering", "where the gameplay is.", "Patience, young grasshopper.", "You are just getting warmed up.", "The space bar is a special key.", "Whenever you press it" },
                    levelTwo = { "You will jump.", "But only if you press it", "when it comes up." },
                    levelThree = { "Now jump.", "Good, but there is more." },
                    levelFour = { "Here comes the darkness.", "It will slowly creep up on you.", "So try not to hesitate.", "Otherwise you will perish.", "That would be bad", "It could be worse.", "It starts off slow", "But it will get faster." },
                    levelFive = { "Much faster."};

    public static GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        isTutorial = ScoreKeeper.instance.isTutorial;
        gameStarted = false;
        isPaused = false;
        canJump = false;
        fogMoving = false;
        gameOver = false;
        winState = false;

        pauseMenu.SetActive(false);

        if (isTutorial)
        {
            //Start on the first level
            level = 0;
        } else
        {
            //Start on a later level, skipping the tutorial part.
            level = 4;
        }

        /*
        sentences[0] = new string[] { levelOne };
        sentences[1] = new string[levelTwo.Length];
        sentences[2] = new string[levelThree.Length];
        sentences[3] = new string[levelFour.Length];
        sentences[4] = new string[levelFive.Length];
        */

        typedChars = "";
        typedText.text = typedChars;
        sentenceNum = 0;
        untypedChars = sentences[level,0];
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

        if(level >= 1)
        {
            canJump = true;
        }

        if(level >= 2)
        {
            gameStarted = true;
        }

        if(level >= 3)
        {
            fogMoving = true;
        }

        UpdateHearts();

        pointsText.text = points.ToString();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }

        if(isPaused || gameOver)
        {
            if(Input.GetKeyUp(KeyCode.R))
            {
                SceneManager.LoadScene("Scene1");
            }

            if (Input.GetKeyUp(KeyCode.M))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        if(points > ScoreKeeper.GetHighScore())
        {
            ScoreKeeper.SetHighScore(points);
        }
    }


    public void OnGUI()
    {
        Event e = Event.current;
        if (e != null)
        {
            if (e.isKey)
            {
                keyPressed = e.keyCode;

                if (containsKey(keyPressed) && !playerController.isFalling && !gameOver && !isPaused)
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
                            } else if(sentenceNum + 1 < 10)
                            {
                                sentenceNum++;
                                //playerController.playerRB.transform.position = new Vector2(playerController.playerRB.transform.position.x + 5, playerController.playerRB.transform.position.y);
                                playerController.playerRB.transform.Translate(Vector2.right * 50 * Time.deltaTime, Space.World);

                                if (sentenceNum < 10)
                                {
                                    untypedChars = sentences[level, sentenceNum];
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
                                    if(level < 5)
                                    {
                                        sentenceNum = 0;
                                        untypedChars = sentences[level,sentenceNum];

                                        /*
                                        finalText.text = "You scored " + points + " points.";
                                        
                                        gameOverText.text = "Congratulations. You have reached level " + level + "." ;
                                        restartText.text = "Press SPACE to continue.\nPress R to restart.";
                                        gameOverPanel.SetActive(true);
                                        winState = true;*/

                                        if (fogMoving)
                                        {
                                            playerController.playerLag *= 10;
                                        }
                                    } else
                                    {
                                        finalText.text = "You scored " + points + " points.";
                                        gameOverText.text = "Congratulations. You have evaded the darkness.";
                                        restartText.text = "Press SPACE to continue.\nPress R to restart.";
                                        gameOverPanel.SetActive(true);
                                        isPaused = true;
                                        winState = true;
                                    }
                                }
                                typedChars = "";
                            }

                            typedText.text = typedChars;
                            untypedText.text = untypedChars;

                            if(keyPressed == KeyCode.Space && canJump)
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
            gameOverPanel.SetActive(true);
            isPaused = true;
            gameOver = true;
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        isPaused = !isPaused;
    }
}
