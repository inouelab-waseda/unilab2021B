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
    /// ���Ɏ��s����R�}���h�̐ݒ�(null�̏ꍇ��1���ɂȂ�)
    /// </summary>
    public ActionCommand nextcursor = null;

    /// <summary>
    /// ���̃R�}���h�����s�����܂ł̈ڍs����
    /// </summary>
    public float delaytime = 1.0f;

    /// <summary>
    /// ���̃R�}���h�ōs����A�N�V�����̒��g(ex:Move,Rotate,Check��)
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// ���̃R�}���h�������������ۂ̐ݒ�
    /// </summary>
    public abstract void Initialize();
}
