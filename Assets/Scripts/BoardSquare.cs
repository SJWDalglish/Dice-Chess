using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour
{
    private GameManager gm;
    public Color startcolor;
    public bool selected;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        startcolor = GetComponent<Renderer>().material.color;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (gm.moveDiceMode)
        {
            if ((Math.Abs(gm.editPos.x - transform.position.x) < .1f) || (Math.Abs(gm.editPos.z - transform.position.z) <.1f)) {    
                GetComponent<Renderer>().material.color = startcolor;
                GameManager.instance.MoveDice(transform.position, this);
            }
        } else if ((getDie(transform.position).isWhite && gm.whiteTurn) || (!getDie(transform.position).isWhite && !gm.whiteTurn))
        {
            GetComponent<Renderer>().material.color = Color.red;
            GameManager.instance.MoveDiceMode(transform.position, this);
            selected = true;
        }
        
    }

    void OnMouseEnter()
    {
        if (gm.moveDiceMode)
        {
            
            Debug.Log("x:" + (gm.editPos.x - transform.position.x).ToString());
            Debug.Log("z:" + (gm.editPos.z - transform.position.z).ToString());
            if ((Math.Abs(gm.editPos.x - transform.position.x) < .1f) || (Math.Abs(gm.editPos.z - transform.position.z) <.1f)) {
                GetComponent<Renderer>().material.color = Color.red;
                gm.CalculatePath(transform.position, this);
            }
        } else if ((getDie(transform.position).isWhite && gm.whiteTurn) || (!getDie(transform.position).isWhite && !gm.whiteTurn))
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        
    }

    void OnMouseExit()
    {
        if (!selected)
            GetComponent<Renderer>().material.color = startcolor;
    }

    public DiceController getDie(Vector3 pos) {
        GameObject[,] dl = gm.diceList;
        GameObject die = null;
        for (int i=0; i<dl.GetLength(0); i++) {
            if ((dl[i,0].transform.position - pos).magnitude <.5f) {
                die = dl[i,0];
                break;
            } else if ((dl[i,1].transform.position - pos).magnitude <.5f) {
                die = dl[i,1];
                break;
            }
        }
        DiceController dc = die.GetComponent<DiceController>();
        return dc;
    }
}