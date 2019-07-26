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
                gameOverText;

    public Rigidbody2D player;

    public PlayerController playerController;

    public int jumpSpeed,
                points = 0,
                difficulty = 0,
                level,
                lives = 3;

    private string typedChars,
                    untypedChars,
                    upperCaseText;

    private int typedCharsSize,
                untypedCharsSize,
                keyPos,
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
                        groundGroup,
                        highlightedText;

    public RectTransform highlight;

    public Vector2 highlightPos;


    private KeyCode[] keyCodes = {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I,
                                    KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
                                    KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space,
                                    KeyCode.Comma, KeyCode.Period, KeyCode.Minus};

    private char[] charArray = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', ',', '.', '-' };

    public string[,] sentences = {
                                    { "Type the highlighted characters.", "Well done.", "You have mastered the controls.", "Welcome to A Space in Time.", "You may be wondering", "where the gameplay is.", "Patience, young grasshopper.", "We are just getting started.", "The space bar is a special key.", "Whenever you press it" },
                                    { "You will jump.", "But only if you press it", "while it is highlighted.", "Just time your jumps.", "To avoid the dangers.", "Yes, there are dangers ahead.", "There will be pitfalls.", "And blocks that are slightly", "too large to step over.", "Cue the dangers." },
                                    { "You should time your jumps.", "Patience is a virtue, after all.", "Take it all in your stride.", "There is no need to rush.", "Slow and steady wins the race.", "Well it would, were it not for", "The Creeping Darkness.", "Oh, I neglected to mention", "The darkness will chase you", "Right about..." },
                                    { "Now.", "It is quite slow to begin with.", "But it will indeed get faster", "as time goes on.", "So try to stay away from it.", "You may have already noticed", "that every sentence you type", "gives you a slight boost.", "With skill, you may survive.", "But you will likely perish." },
                                    { "Sorry about that.", "We are almost at the end", "of the tutorial section.", "Just be good at typing.", "And also time your jumps.", "I give great advice.", "I shall now leave you", "to type out the lyrics", "to Space Oddity by David Bowie.", "Good luck."},
                                    {"Ground Control to Major Tom.", "Ground Control to Major Tom.", "Take your protein pills", "and put your helmet on.", "Ground Control to Major Tom.", "Commencing countdown", "engines on.", "Check ignition and may", "Gods love be with you.", "Ten, Nine, Eight, Seven, Six,"},
                                    {"Five, Four, Three, Two, One,", "Lift off.", "This is Ground Control", "to Major Tom.", "You really made the grade.", "And the papers want to know", "whose shirts you wear.", "Now it is time to leave", "the capsule if you dare.", "This is Major Tom"},
                                    {"to Ground Control.", "I am stepping through the door.", "And I am floating", "in a most peculiar way.", "And the stars look", "very different today.", "For here", "Am I sitting in a tin can.", "Far above the world.", "Planet Earth is blue"},
                                    {"And there is nothing I can do.", "Though I am past one hundred", "thousand miles.", "I am feeling very still.", "And I think my spaceship", "knows which way to go.", "Tell my wife", "I love her very much", "she knows.", "Ground Control to Major Tom."},
                                    {"Your circuit is dead", "there is something wrong.", "Can you hear me, Major Tom.", "Can you hear me, Major Tom.", "Can you hear me, Major Tom.", "Can you hear", "Am I floating round my tin can.", "Far above the Moon.", "Planet Earth is blue", "And there is nothing I can do."}
    };

    public static GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        //isTutorial = ScoreKeeper.instance.isTutorial;
        gameStarted = false;
        isPaused = false;
        canJump = false;
        fogMoving = false;
        gameOver = false;
        winState = false;

        pauseMenu.SetActive(false);

        level = ScoreKeeper.instance.startLevel;

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

        highlightPos = highlight.position;

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

        //highlightedText.transform.position = untypedText.transform.position;
        //highlightedText.transform.localScale = new Vector3(1, 1, 1);

        typedCharsSize = typedChars.Length;
        untypedCharsSize = untypedChars.Length;

        typedText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, typedCharsSize * 30);
        untypedText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, untypedCharsSize * 30);

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
                ScoreKeeper.instance.startLevel = level;
                SceneManager.LoadScene("Scene1");
            }

            if (Input.GetKeyUp(KeyCode.M))
            {
                ScoreKeeper.instance.isFromPause = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
        
        if(level == 10)
        {
            if (points > ScoreKeeper.instance.highScore)
            {
                ScoreKeeper.instance.highScore = points;
            }

            finalText.text = "You scored " + points + " points.";
            gameOverText.text = "Congratulations. You have evaded the darkness.";
            gameOverPanel.SetActive(true);
            isPaused = true;
            winState = true;
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

                            highlightedText.transform.position = new Vector2(highlightedText.transform.position.x + 30, highlightedText.transform.position.y);

                            upperCaseText = untypedChars.ToUpper();

                            if (upperCaseText.Length > 0)
                            {
                                currentChar = upperCaseText[0];
                            } else //if(sentenceNum + 1 < 10)
                            {
                                sentenceNum++;
                                playerController.playerRB.transform.Translate(Vector2.right * 50 * Time.deltaTime, Space.World);

                                if (sentenceNum < 10)
                                {
                                    untypedChars = sentences[level, sentenceNum];
                                }
                                else
                                {
                                    level++;
                                    if(level < 10)
                                    {
                                        sentenceNum = 0;
                                        untypedChars = sentences[level,sentenceNum];

                                        //highlightedText.transform.position = new Vector2(highlightedText.transform.position.x - (typedChars.Length * 30), highlightedText.transform.position.y);
                                        //highlight.position = new Vector2(16f, highlight.position.y);

                                        /*
                                        if (fogMoving)
                                        {
                                            playerController.playerLag *= 10;
                                        }
                                        */
                                    } 
                                }
                                highlightedText.transform.position = new Vector2(highlightedText.transform.position.x - (typedChars.Length * 30), highlightedText.transform.position.y);
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
            if (points > ScoreKeeper.instance.highScore)
            {
                ScoreKeeper.instance.highScore = points;
            }
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
