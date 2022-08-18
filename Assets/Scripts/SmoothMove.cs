 using UnityEngine;
 using System.Collections;
 
 public class SmoothMove : MonoBehaviour {
     public Vector3 destination;
     public float speed = 0.1f;
 
     void Start () {
         destination = transform.position;
     }
     
     void Update () {
         transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
     }
 }