using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PassCommand : ActionCommand
{
    private int _memodata;
    public int memodata { get { return _memodata; } }

    public PassCommand(string commandname, int scope, int memo = 0) : base(commandname,scope,0.1f)
    {
        _memodata = memo;
    }

    public override void Action()
    {
        Debug.Log("passコマンドの実行");
        actionsubject.OnNext(this);
    }

    public override void Initialize()
    {

    }
}