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
    /// �R�}���h��(�����R�}���h�ł��p�r���Ⴄ�ꍇ�����邽��)
    /// </summary>
    private string _commandname = null;
    public string commandname { get { return _commandname; } }


    /// <summary>
    /// ���Ɏ��s����R�}���h�̐ݒ�(null�̏ꍇ��1���ɂȂ�)
    /// </summary>
    protected ActionCommand _nextcursor = null;
    public ActionCommand nextcursor { get { return _nextcursor; } }

    /// <summary>
    /// ���̃R�}���h�����s�����܂ł̈ڍs����
    /// </summary>
    private float _delaytime = 1.0f;
    public float delaytime { get { return _delaytime; } }

    /// <summary>
    /// ���̃R�}���h�̌��݂̃X�R�[�v
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
    /// ���̃R�}���h�ōs����A�N�V�����̒��g(ex:Move,Rotate,Check��)
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// ���̃R�}���h�������������ۂ̐ݒ�
    /// </summary>
    public abstract void Initialize();
}
