using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer;

public class HealthBarDisplay : MonoBehaviour
{

    //public float barDisplay; //current progress
    public Vector2 pos = new Vector2(20, 40);
    public Vector2 size = new Vector2(60, 20);
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;
    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }


    void OnGUI()
    {

        //draw the background:
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        //GUI.Box(new Rect(0, 0, size.x, size.y), progressBarEmpty);
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), progressBarEmpty, ScaleMode.StretchToFill);

        //draw the filled-in part:
        GUI.BeginGroup(new Rect(0, 0, size.x, size.y));
        //GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull);
        GUI.DrawTexture(new Rect(0, 0, player.playerHealth, size.y), progressBarFull, ScaleMode.StretchToFill);
        GUI.EndGroup();
        GUI.EndGroup();

    }
}
