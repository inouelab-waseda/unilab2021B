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
    /// コマンド名(同じコマンドでも用途が違う場合があるため)
    /// </summary>
    private string _commandname = null;
    public string commandname { get { return _commandname; } }


    /// <summary>
    /// 次に実行するコマンドの設定(nullの場合は1つ次になる)
    /// </summary>
    protected ActionCommand _nextcursor = null;
    public ActionCommand nextcursor { get { return _nextcursor; } }

    /// <summary>
    /// 次のコマンドが実行されるまでの移行時間
    /// </summary>
    private float _delaytime = 1.0f;
    public float delaytime { get { return _delaytime; } }

    /// <summary>
    /// そのコマンドの現在のスコープ
    /// </summary>
    private int _scope = 0;
    public int scope { get { return _scope; } }
    public int nextscope { get { return getnextscope(); } }

    public void scopeUp() => _scope += 1;
    public void scopeDown()
    {
        if (_scope > 0) _scope -= 1;
    }

    public ActionCommand(string commandname,int scope, float delaytime = 1.0f)
    {
        _commandname = commandname;
        _scope = scope;
        _delaytime = delaytime;
    }

    public int getnextscope()
    {
        if (commandname == "IfEnd" || commandname == "forEnd")
        {
            return scope - 1;
        }
        return scope;
    }

    /// <summary>
    /// そのコマンドで行われるアクションの中身(ex:Move,Rotate,Check等)
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// そのコマンドを初期化した際の設定
    /// </summary>
    public abstract void Initialize();
}
