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

    /// <summary>
    /// 次に実行するコマンドの設定(nullの場合は1つ次になる)
    /// </summary>
    public ActionCommand nextcursor = null;

    /// <summary>
    /// 次のコマンドが実行されるまでの移行時間
    /// </summary>
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
