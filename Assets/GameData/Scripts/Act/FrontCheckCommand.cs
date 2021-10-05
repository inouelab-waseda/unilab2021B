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

    public FrontCheckCommand(ActionCommand failedcommand, string obj, bool existsflag)
    {
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _failedcommand = failedcommand;
        _obj = obj;
        _existsflag = existsflag;
        delaytime = 0.1f;

    }

    public override void Action()
    {
        Debug.LogFormat("前方に{0}があるかチェックし、{1}のとき通す",_obj,_existsflag);
        if (_obj == "wall")
        {
            if (playercontroller.FrontWallExists() != _existsflag) nextcursor = _failedcommand;
            else nextcursor = null;
        } else if (_obj == "enemy")
        {

        }
        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {
        nextcursor = null;
        
    }
}
