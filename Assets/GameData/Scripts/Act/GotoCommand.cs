using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GotoCommand : ActionCommand
{
    private int _loopnum;
    private int current_loop;
    private ActionCommand _nextcommand;

    public GotoCommand(string commandname, int scope, ActionCommand nextcommand,int loopnum) : base(commandname, scope, 0.1f)
    {
        _nextcommand = nextcommand;
        if (loopnum <= 0) loopnum = 99999;
        _loopnum = loopnum;
        current_loop = 0;
    }

    public override void Action()
    {
        Debug.Log("gotoコマンドの実行");
        current_loop += 1;
        if (current_loop < _loopnum) _nextcursor = _nextcommand;
        else
        {
            current_loop = 0;
            _nextcursor = null;
        }

        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {
        current_loop = 0;
    }
}