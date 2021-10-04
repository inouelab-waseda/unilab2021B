using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MoveCommand : ActionCommand
{
    private PlayerController playercontroller;

    public MoveCommand()
    {
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Add();
    }

    public override void Action()
    {
        Debug.Log("ëOï˚Ç…êiÇﬁ");
        playercontroller.PlayerMove();
        actionsubject.OnNext(this);
    }

    public override void Add()
    {
        
    }

    public override void Remove()
    {
        
    }
}
