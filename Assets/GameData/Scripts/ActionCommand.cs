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
    /// ���̃R�}���h�ōs����A�N�V�����̒��g(ex:Move,Rotate,Check��)
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// ���̃R�}���h��Commandlist�ǉ����Ɏ��s����ׂ�����(ex:if��->end���ꏏ�ɍ��)
    /// </summary>
    public abstract void Add();

    /// <summary>
    /// ���̃R�}���h��Commandlist�폜���Ɏ��s����ׂ�����(ex:if��->end���ꏏ�ɍ폜)
    /// </summary>
    public abstract void Remove();
}
