using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

public class DiceLogic : MonoBehaviour
{
    public int[,][] absDice;
    
    private GameManager gm; 

    void GetBoardState() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        absDice = new int[gm.boardx, gm.boardz][];
        for (int i=0; i< gm.boardx; i++) {
            for (int j=0; j < gm.boardz; j++) {
                BoardSquare bs = gm.boardSquares[i,j].GetComponent<BoardSquare>();
                if (bs.getDie(bs.transform.position) == null) {
                    absDice[i,j] = new int[] {0, 0, 0, 0};
                } else {
                    DiceController dc = bs.getDie(bs.transform.position).GetComponent<DiceController>();
                    absDice[i,j] = GetDiceState(dc);
                }
            }
        }
    }

    int[] GetDiceState(DiceController dc) {
        // Find which axis is up
        bool yUp = (Vector3.Cross(Vector3.up, dc.transform.up).magnitude < 0.5f) && (Vector3.Dot(Vector3.up, dc.transform.up) > 0);
        bool yDown = (Vector3.Cross(Vector3.up, dc.transform.up).magnitude < 0.5f) && !(Vector3.Dot(Vector3.up, dc.transform.up) > 0);
        bool xUp = (Vector3.Cross(Vector3.up, dc.transform.right).magnitude < 0.5f) && (Vector3.Dot(Vector3.up, dc.transform.right) > 0);
        bool xDown = (Vector3.Cross(Vector3.up, dc.transform.right).magnitude < 0.5f) && !(Vector3.Dot(Vector3.up, dc.transform.right) > 0);
        bool zUp = (Vector3.Cross(Vector3.up, dc.transform.forward).magnitude < 0.5f) && (Vector3.Dot(Vector3.up, dc.transform.forward) > 0);
        bool zDown = (Vector3.Cross(Vector3.up, dc.transform.forward).magnitude < 0.5f) && !(Vector3.Dot(Vector3.up, dc.transform.forward) > 0);
        
        
        // Find which axis is oriented right
        bool yRight = (Vector3.Cross(Vector3.right, dc.transform.up).magnitude <0.5f)  && (Vector3.Dot(Vector3.right, dc.transform.up) > 0);
        bool yLeft = (Vector3.Cross(Vector3.right, dc.transform.up).magnitude <0.5f)  && !(Vector3.Dot(Vector3.right, dc.transform.up) > 0);
        bool xRight = (Vector3.Cross(Vector3.right, dc.transform.right).magnitude <0.5f) && (Vector3.Dot(Vector3.right, dc.transform.right) > 0);
        bool xLeft = (Vector3.Cross(Vector3.right, dc.transform.right).magnitude <0.5f) && !(Vector3.Dot(Vector3.right, dc.transform.right) > 0);
        bool zRight = (Vector3.Cross(Vector3.right, dc.transform.forward).magnitude <0.5f) && (Vector3.Dot(Vector3.right, dc.transform.forward) > 0);
        bool zLeft = (Vector3.Cross(Vector3.right, dc.transform.forward).magnitude <0.5f) && !(Vector3.Dot(Vector3.right, dc.transform.forward) > 0);

        int wd;
        if (dc.isWhite) {
            wd = 1;
        } else {
            wd = -1;
        }

        // Not finished!
        if(yUp && xRight) {return new int[] {3,5,6,wd};}
        if(yUp && xLeft) {return new int[] {3,2,1,wd};}
        if(yUp && zRight) {return new int[] {3,6,2,wd};}
        if(yUp && zLeft) {return new int[] {3,1,5,wd};}
        if(yDown && xRight) {return new int[] {4,5,1,wd};}
        if(yDown && xLeft) {return new int[] {4,2,6,wd};}
        if(yDown && zRight) {return new int[] {4,6,2,wd};}
        if(yDown && zLeft) {return new int[] {4,1,5,wd};}
        if(xUp && yRight) {return new int[] {5,3,1,wd};}
        if(xUp && yLeft) {return new int[] {5,4,6,wd};}
        if(xUp && zRight) {return new int[] {5,6,3,wd};}
        if(xUp && zLeft) {return new int[] {5,1,4,wd};}
        if(xDown && yRight) {return new int[] {2,3,6,wd};}
        if(xDown && yLeft) {return new int[] {2,4,1,wd};}
        if(xDown && zRight) {return new int[] {2,6,4,wd};}
        if(xDown && zLeft) {return new int[] {2,1,3,wd};}
        if(zUp && xRight) {return new int[] {6,5,4,wd};}
        if(zUp && xLeft) {return new int[] {6,2,3,wd};}
        if(zUp && yRight) {return new int[] {6,3,5,wd};}
        if(zUp && yLeft) {return new int[] {6,4,2,wd};}
        if(zDown && xRight) {return new int[] {1,5,3,wd};}
        if(zDown && xLeft) {return new int[] {1,2,4,wd};}
        if(zDown && yRight) {return new int[] {1,3,2,wd};}
        if(zDown && yLeft) {return new int[] {1,4,5,wd};}
        return new int[] {0,0,0,0};
    }

    // public int[] RollDice(int[] ad, Vector3 roll) 
    // {
    //     int[] rd = ad;
        
    //     int xRoll = Mathf.RoundToInt(roll.x);
    //     for (int i=0; i<Math.Abs(xRoll); i++) {
    //         rd = GetUnitFaceRoll(rd, new Vector3(Math.Sign(xRoll) * 1f,0f,0f))
    //     }

    //     int yRoll = Mathf.RoundToInt(roll.y);
    //     for (int i=0; i<Math.Abs(xRoll); i++) {
    //         rd = GetUnitFaceRoll(rd, new Vector3(Math.Sign(xRoll) * 1f,0f,0f))
    //     }

    //     int zRoll = Mathf.RoundToInt(roll.z);
    //     for (int i=0; i<Math.Abs(xRoll); i++) {
    //         rd = GetUnitFaceRoll(rd, new Vector3(Math.Sign(xRoll) * 1f,0f,0f))
    //     }
    // }

    // public int[] GetUnitFaceRoll(int[] ad, Vector3 unitRot)
    // {
    //     if (unitRot == new Vector3(1,0,0)) 
    //     {
    //         return new int[] {7 - ad[2], ad[1], ad[0], ad[3]};
    //     } 
    //     else if (unitRot == new Vector3(-1,0,0)) 
    //     {
    //         return new int[] {ad[2], ad[1], 7 - ad[0], ad[3]};
    //     } 
    //     else if (unitRot == new Vector3(0,1,0)) 
    //     {
    //         return new int[] {ad[0], ad[2], 7 - ad[1], ad[3]};
    //     } 
    //     else if (unitRot == new Vector3(0,-1,0)) 
    //     {
    //         return new int[] {ad[0], 7 - ad[2], ad[1], ad[3]};
    //     } 
    //     else if (unitRot == new Vector3(0,0,1)) 
    //     {
    //         return new int[] {7 - ad[1], ad[0], ad[2], ad[3]};
    //     } 
    //     else if (unitRot == new Vector3(0,0,-1)) 
    //     {
    //         return new int[] {ad[1], 7 - ad[0], ad[2], ad[3]};
    //     }
    // }
}