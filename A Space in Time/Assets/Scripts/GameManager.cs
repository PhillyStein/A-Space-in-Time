using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text typedText,
                untypedText,
                pointsText;

    public Rigidbody2D player;

    public PlayerController playerController;

    public int jumpSpeed,
                points = 0;

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


    private KeyCode[] keyCodes = {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I,
                                    KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
                                    KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space,
                                    KeyCode.Comma, KeyCode.Period, KeyCode.Exclaim};

    private char[] charArray = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', ',', '.', '!'};

    private string[] sentences = { "And so, the adventure begins.", "It was a dark and stormy night.", "There were pigeons and shit.", "Not today, Satan. Not today!" };

    // Start is called before the first frame update
    void Start()
    {
        typedChars = "";
        typedText.text = typedChars;
        sentenceNum = 0;
        untypedChars = sentences[sentenceNum];
        untypedText.text = untypedChars;
        typedCharsSize = typedChars.Length;
        untypedCharsSize = untypedChars.Length;

        upperCaseText = untypedText.text.ToUpper();
        currentChar = upperCaseText[0];
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

                if (containsKey(keyPressed) && !playerController.isFalling)
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
                            } else if(sentenceNum + 1 <= sentences.Length)
                            {
                                sentenceNum++;
                                untypedChars = sentences[sentenceNum];
                                typedChars = "";
                            }

                            typedText.text = typedChars;
                            untypedText.text = untypedChars;

                            if(keyToChar == ' ')
                            {

                                //player.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                                playerController.canJump = true;
                                points += 100;
                            } else
                            {
                                points += 10;
                            }
                        }
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
}
