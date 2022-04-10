 using UnityEngine;
 using System.Collections;
 
 public class CameraUpdater : MonoBehaviour {
 
     private Transform player;
     
     void Start () {
         player = GameObject.Find ("Player").transform;
     }
 
     void Update () {
         transform.position = new Vector3 (player.position.x, player.position.y, player.position.z - 10);
     }
 }