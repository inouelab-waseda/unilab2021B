using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public abstract class ActionCommand
{
    protected Subject<ActionCommand> actionsubject = new Subject<ActionCommand>();
    public IObservable<ActionCommand> ActionFinished
    {
        get { return actionsubject; }
    }

    public ActionCommand nextcursor = null;
    public int scope = 0;
    public float delaytime = 1.0f;

    /// <summary>
    /// そのコマンドで行われるアクションの中身(ex:Move,Rotate,Check等)
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// そのコマンドを初期化した際の設定
    /// </summary>
    public abstract void Initialize();
}
