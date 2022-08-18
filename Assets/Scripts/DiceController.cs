using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    private Camera mainCamera;
    private float CameraZDistance;
    private int[] gridSquare;
    private int[] newGridSquare;
    public float speed;
    public Vector3 destination;
    public Quaternion rotation;
    public bool isWhite;
    public bool inPlay;

    void Start()
    {
        // mainCamera = Camera.main;
        // CameraZDistance =
        //     mainCamera.WorldToScreenPoint(transform.position).z; //z axis of the game object for screen view
        destination = transform.position;
        rotation = transform.rotation;
        gridSquare = new int[] {(int) this.transform.position.x, (int) this.transform.position.z};
        speed = 5;
        if (this.name.Substring(0,1) == "w") {
            isWhite = true;
        } else {
            isWhite = false;
        }
        inPlay = true;
        // Debug.Log("gridSquare set to " + gridSquare[0] + ',' + gridSquare[1]);
    }

    // void OnMouseDrag()
    // {
    //     Vector3 ScreenPosition =
    //         new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance); //z axis added to screen point 
    //     Vector3 NewWorldPosition =
    //         mainCamera.ScreenToWorldPoint(ScreenPosition); //Screen point converted to world point

    //     transform.position = NewWorldPosition;
    // }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }
        // if ((int) this.transform.position.x != gridSquare[0]) 
        // {
        //     newGridSquare = new int[] {(int) this.transform.position.x, (int) this.transform.position.z};
        //     this.transform.rotation = Quaternion.Euler(this.getRotationVector(new Vector3 (newGridSquare[0]-gridSquare[0],0,newGridSquare[1]-gridSquare[1])));
        //     gridSquare = newGridSquare;
        //     if (this.name=="whiteDie1") Debug.Log("gridSquare set to " + gridSquare[0] + ',' + gridSquare[1]);
        // } 
        // if ((int) this.transform.position.z != gridSquare[1]) 
        // {
        //     newGridSquare = new int[] {(int) this.transform.position.x, (int) this.transform.position.z};
        //     this.transform.rotation = Quaternion.Euler(this.getRotationVector(new Vector3 (newGridSquare[0]-gridSquare[0],0,newGridSquare[1]-gridSquare[1])));
        //     gridSquare = newGridSquare;
        //     if (this.name=="whiteDie1") Debug.Log("gridSquare set to " + gridSquare[0] + ',' + gridSquare[1]);
        // }

    void OnMouseDown()
    {
        //Highlight();
        // Debug.Log("Editing:" + transform.position, this);
        // GameManager.instance.EditSquareMode(transform.position, this);
    }

    public int getFace() {
        if (  Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f) //x axis a.b.sin theta <45
        {
            if (Vector3.Dot(Vector3.up, transform.right) > 0)
            {
                return 5;
            }
            else
            {
                return 2;
            }
        }
        else if ( Vector3.Cross(Vector3.up, transform.up).magnitude <0.5f) //y axis
        {
            if (Vector3.Dot(Vector3.up, transform.up) > 0)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else if ( Vector3.Cross(Vector3.up, transform.forward).magnitude <0.5f) //z axis
        {
            if (Vector3.Dot(Vector3.up, transform.forward) > 0)
            {
                return 6;
            }
            else
            {
                return 1;
            }
        }
        else return 0;
    }

    // Work out rotation direction as die moves over the grid
    public Quaternion getRotationVector(Vector3 dir) {
        // Find which axis is up
        bool xUp = Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f;
        bool yUp = Vector3.Cross(Vector3.up, transform.up).magnitude < 0.5f;
        // public bool zUp = Vector3.Cross(Vector3.up, transform.forward).magnitude < 0.5f;
        
        // Find which axis is in direction of movement
        bool xTo = Vector3.Cross(dir, transform.right).magnitude <0.5f;
        bool yTo = Vector3.Cross(dir, transform.up).magnitude <0.5f;
        // public bool zTo = Vector3.Cross(dir, transform.forward).magnitude <0.5f

        Vector3 rot;

        if (xUp) //x axis a.b.sin theta <45
        {
            if (yTo) //check y for direction of travel
            {
                if (Vector3.Dot(Vector3.up, transform.right) * Vector3.Dot(dir, transform.up) > 0)
                {                
                    rot = new Vector3 (0f,0f,90f);
                }
                else rot = new Vector3 (0f,0f,-90f);
            }
            else // Must be z direction for travel
            {
                if (Vector3.Dot(Vector3.up, transform.right) * Vector3.Dot(dir, transform.forward) > 0)
                {                
                    rot = new Vector3 (0f,-90f,0f);
                }
                else rot = new Vector3 (0f,90f,0f);
            }
        }
        else if (yUp) //y axis
        {
            if (xTo) //check x for direction of travel
            {
                if (Vector3.Dot(Vector3.up, transform.up) * Vector3.Dot(dir, transform.right) > 0)
                {                
                    rot = new Vector3 (0f,0f,-90f);
                }
                else rot = new Vector3 (0f,0f,90f);
            }
            else // Must be z direction for travel
            {
                if (Vector3.Dot(Vector3.up, transform.up) * Vector3.Dot(dir, transform.forward) > 0)
                {                
                    rot = new Vector3 (90f,0f,0f);
                }
                else rot = new Vector3 (-90f,0f,0f);
            }
        }
        else 
        {
            if (xTo) //check x for direction of travel
            {
                if (Vector3.Dot(Vector3.up, transform.forward) * Vector3.Dot(dir, transform.right) > 0)
                {                
                    rot = new Vector3 (0f,90f,0f);
                }
                else rot = new Vector3 (0f,-90f,0f);
            }
            else // Must be y direction for travel
            {
                if (Vector3.Dot(Vector3.up, transform.forward) * Vector3.Dot(dir, transform.up) > 0)
                {                
                    rot = new Vector3 (-90f,0f,0f);
                }
                else rot = new Vector3 (90f,0f,0f);
            }
        }

        return this.transform.rotation * Quaternion.Euler(dir.magnitude * rot);
    }
}