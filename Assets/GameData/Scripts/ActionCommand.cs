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
    /// そのコマンドで行われるアクションの中身(ex:Move,Rotate,Check等)
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// そのコマンドがCommandlist追加時に実行するべきもの(ex:if文->endも一緒に作る)
    /// </summary>
    public abstract void Add();

    /// <summary>
    /// そのコマンドがCommandlist削除時に実行するべきもの(ex:if文->endも一緒に削除)
    /// </summary>
    public abstract void Remove();
}
