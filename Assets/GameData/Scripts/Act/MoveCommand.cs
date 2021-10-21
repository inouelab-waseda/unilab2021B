using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MoveCommand : ActionCommand
{
    private PlayerController playercontroller;

    public MoveCommand(string commandname, int scope) : base(commandname, scope,0.5f)
    {
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void Action()
    {
        Debug.Log("ëOï˚Ç…êiÇﬁ");
        playercontroller.PlayerMove();
        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {
        
    }
}
