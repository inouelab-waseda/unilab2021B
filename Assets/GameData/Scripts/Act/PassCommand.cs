using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PassCommand : ActionCommand
{

    public PassCommand()
    {
        delaytime = 0.1f;
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