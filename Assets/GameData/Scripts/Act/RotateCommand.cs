using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RotateCommand : ActionCommand
{
    private PlayerController playercontroller;
    private string direction;

    public RotateCommand(string commandname, int scope, string dir) : base(commandname, scope,0.7f)
    {
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        direction = dir;
    }

    public override void Action()
    {
        Debug.LogFormat("{0}方向へまがる",direction);
        playercontroller.PlayerRotate(direction);
        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {

    }
}
