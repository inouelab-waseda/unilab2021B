using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AttackCommand : ActionCommand
{
    private PlayerController playercontroller;
    private string direction;

    public AttackCommand(string commandname, int scope) : base(commandname, scope,0.9f)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playercontroller = player.GetComponent<PlayerController>();
    }

    public override void Action()
    {
        Debug.LogFormat("çUåÇÇ∑ÇÈ");
        playercontroller.PlayerAttack();
        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {

    }
}
