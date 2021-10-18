using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FrontCheckCommand : ActionCommand
{
    private PlayerController playercontroller;
    private ActionCommand _failedcommand;
    private string _obj;
    private bool _existsflag;

    public FrontCheckCommand(string commandname, int scope, ActionCommand failedcommand, string obj, bool existsflag) : base(commandname, scope, 0.1f)
    {
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _failedcommand = failedcommand;
        _obj = obj;
        _existsflag = existsflag;

    }

    public override void Action()
    {
        Debug.LogFormat("前方に{0}があるかチェックし、{1}のとき通す",_obj,_existsflag);
        if (_obj == "wall")
        {
            if (playercontroller.FrontWallExists() != _existsflag) _nextcursor = _failedcommand;
            else _nextcursor = null;
        } else if (_obj == "enemy")
        {
            if (playercontroller.FrontEnemyExists() != _existsflag) _nextcursor = _failedcommand;
            else _nextcursor = null;
        }
        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {
        _nextcursor = null;
        
    }

    public string Getobjname()
    {
        return _obj;
    }
}
