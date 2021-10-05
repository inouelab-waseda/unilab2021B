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
    /// ���̃R�}���h�ōs����A�N�V�����̒��g(ex:Move,Rotate,Check��)
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// ���̃R�}���h�������������ۂ̐ݒ�
    /// </summary>
    public abstract void Initialize();
}
