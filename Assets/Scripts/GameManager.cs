using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

public class GameManager : MonoBehaviour
{
    public int boardx, boardz, numDice;

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public GameObject whiteSquare;
    public GameObject blackSquare;
    public GameObject whiteDie;
    public GameObject blackDie;
    private Transform boardHolder;
    public bool moveDiceMode = false;							//Boolean to check if a square is being edited.
    public Vector3 editPos;                                 //Location of square to be changed.
    public Vector3 editPos2;                                 //Location of square to be changed.
    public BoardSquare editSquare;
    public BoardSquare editSquare2;
    public DiceController editObject;                        //object to be changed.
    private int[,] boardGrid;
    public GameObject[,] boardSquares;
    public GameObject[,] diceList;
    public GameObject editDie;
    public Text txt;
    private int[] d1Index;
    private int[] d2Index;
    public bool whiteTurn;
    private AudioSource source;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        // DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }
    
    //Initializes the game for each level.
    void InitGame()
    {
        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        // doingSetup = true;
        source = GetComponent<AudioSource>();

        // Reposition Camera
        GameObject.Find("Main Camera").transform.position = new Vector3(-3, 3, 3);
        GameObject.Find("Main Camera").transform.localEulerAngles = new Vector3(30, 90, 0);

        GameObject toInstantiate;
        GameObject inst;

        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;
        // boardGrid = new int[boardx, boardz];
        boardSquares = new GameObject[boardx, boardz];
        diceList = new GameObject[numDice,2];
        // boardHolder.gameObject.tag = "Board";

        for (int i=0; i< boardx; i++)
        {
            for (int j=0; j< boardz; j++) 
            {
                if ((i + j) % 2 == 0) {toInstantiate = whiteSquare;}
                else {toInstantiate = blackSquare;}

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                inst = Instantiate(toInstantiate, new Vector3(i, 0f, j), Quaternion.identity) as GameObject;
                inst.name = "boardSquare" + i + j;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                inst.transform.SetParent(boardHolder);

                // Initialize grid values
                boardSquares[i, j] = inst;
                // boardGrid[i, j] = 0;
            }
        }

        // Place Dice
        for (int i=0; i<numDice; i++)
        {
            // Set random values for dice -- needs vectors corresponding to dice values
            // int randomIndex = Random.Range(1, 7);

            //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
            inst = Instantiate(whiteDie, new Vector3(i, .25f, boardz-1), Quaternion.identity) as GameObject;
            inst.name = "whiteDie" + i;
            // boardGrid[i, 0] = inst.GetComponent<DiceController>().getFace();
            diceList[i,0] = inst;

            //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
            inst = Instantiate(blackDie, new Vector3(i, .25f, 0f), Quaternion.identity) as GameObject;
            inst.name = "blackDie" + i;
            // boardGrid[i, boardz-1] = inst.GetComponent<DiceController>().getFace();
            diceList[i,1] = inst;
        }

        whiteTurn = true;
    }

    public void MoveDiceMode(Vector3 position, BoardSquare bs)
    {
        moveDiceMode = true;
        editPos = position;
        editSquare = bs;
        
        editDie = null;
        // int[] d1Index;
        for (int i=0; i< numDice; i++) {
            if ((position - diceList[i,0].transform.position).magnitude < 1)
            {
                editDie = diceList[i,0];
                d1Index = new int[] {i,0};
                break;
            }
            else if ((position - diceList[i,1].transform.position).magnitude < 1)
            {
                editDie = diceList[i,1];
                d1Index = new int[] {i,1};
                break;
            }
        }
        editDie.GetComponent<DiceController>().destination += new Vector3(0, 1, 0);;
        // editObject = clicker;
    }

    public void MoveDice(Vector3 position, BoardSquare bs)
    {
        moveDiceMode = false;
        editPos2 = position;
        editSquare2 = bs;
        editSquare.selected = false;
        for (int i=0; i< boardx; i++) {
            for (int j=0; j< boardz; j++) {
                boardSquares[i,j].GetComponent<BoardSquare>().selected = false;
                boardSquares[i,j].GetComponent<Renderer>().material.color = boardSquares[i,j].GetComponent<BoardSquare>().startcolor;
            }
        }
        GameObject editDie2 = null;
        // int[] d2Index;
        for (int i=0; i< numDice; i++) {
            if ((position - diceList[i,0].transform.position).magnitude < 1)
            {
                editDie2 = diceList[i,0];
                d2Index = new int[] {i,0};
                break;
            }
            else if ((position - diceList[i,1].transform.position).magnitude < 1)
            {
                editDie2 = diceList[i,1];
                d2Index = new int[] {i,1};
                break;
            }
        }

        DiceController dc = editDie.GetComponent<DiceController>();
        if (editDie2 != null) 
        {
            int dieNum1 = dc.getFace();
            int dieNum2 = editDie2.GetComponent<DiceController>().getFace();
            if (dieNum1 > dieNum2) {
                editDie2.GetComponent<DiceController>().destination = new Vector3(d2Index[0],0,-2 + (d2Index[1] * 9));
                editDie2.GetComponent<DiceController>().inPlay = false;
                // GameObject.Find("TestText").GetComponent<TextMeshProUGUI>().text = (editSquare2.transform.position - editSquare.transform.position).ToString();
                Quaternion rot = dc.getRotationVector(position-editPos);
                dc.destination = new Vector3(bs.transform.position.x, .25f, bs.transform.position.z);
                dc.rotation = rot;
            } else {
                dc.destination = new Vector3(d1Index[0],0,-2 + (d1Index[1] * 9));
                dc.inPlay = false;
            }
        } else {
            // GameObject.Find("TestText").GetComponent<TextMeshProUGUI>().text = (editSquare2.transform.position - editSquare.transform.position).ToString();
            Quaternion rot = dc.getRotationVector(position-editPos);
            dc.destination = new Vector3(bs.transform.position.x, .25f, bs.transform.position.z);
            dc.rotation = rot;
        }

        source.Play();
        whiteTurn = !whiteTurn;

        // editDie.transform.position = new Vector3(bs.transform.position.x, .25f, bs.transform.position.z);
        // editDie.transform.rotation = Quaternion.Euler(rot);
        // editObject = clicker;
    }
    
    public void CalculatePath(Vector3 pos, BoardSquare bs)
    {
        for (int i=0; i< boardx; i++) {
            for (int j=0; j< boardz; j++) {
                Vector3 squareRelPos = (new Vector3(i,0,j) - editPos);
                Vector3 pointRelPos = (pos - editPos);

                if ((boardSquares[i,j].GetComponent<BoardSquare>() == editSquare) || (boardSquares[i,j].GetComponent<BoardSquare>() == bs))
                {
                    boardSquares[i,j].GetComponent<Renderer>().material.color = Color.red;
                    boardSquares[i,j].GetComponent<BoardSquare>().selected = true;
                } 
                else if (squareRelPos.magnitude < pointRelPos.magnitude && Vector3.Cross(squareRelPos, pointRelPos).magnitude < 1f && Vector3.Dot(squareRelPos, pointRelPos) > 0)
                {
                    
                    boardSquares[i,j].GetComponent<Renderer>().material.color = Color.red;
                    boardSquares[i,j].GetComponent<BoardSquare>().selected = true;
                } 
                else 
                {
                    boardSquares[i,j].GetComponent<Renderer>().material.color = boardSquares[i,j].GetComponent<BoardSquare>().startcolor;
                    boardSquares[i,j].GetComponent<BoardSquare>().selected = false;
                }
            }
        }
    }



    public void Update()
    {
        int whiteDice = 0;
        int blackDice = 0;
        for (int i=0; i<numDice; i++) {
            if (diceList[i,0].GetComponent<DiceController>().inPlay) whiteDice += 1;
            if (diceList[i,1].GetComponent<DiceController>().inPlay) blackDice += 1;
        }
        if(whiteDice == 0)
            GameObject.Find("WinText").GetComponent<TextMeshProUGUI>().text = "Black Dice Win!";
        if(blackDice == 0)
            GameObject.Find("WinText").GetComponent<TextMeshProUGUI>().text = "White Dice Win!";
    }


}
