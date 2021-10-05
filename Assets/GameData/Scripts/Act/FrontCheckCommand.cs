using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FrontCheckCommand : ActionCommand
{
    private PlayerController playercontroller;
    private ActionCommand _failedcommand;
    private string _obj;

    public FrontCheckCommand(ActionCommand failedcommand, string obj)
    {
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _failedcommand = failedcommand;
        _obj = obj;

    }

    public override void Action()
    {
        Debug.LogFormat("前方に{0}があるかチェック",_obj);
        nextcursor = _failedcommand;
        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {
        
    }
}
