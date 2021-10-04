using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class ActManager : MonoBehaviour
{
    [SerializeField]
    private List<ActionCommand> commandlist = new List<ActionCommand> { };

    private List<IDisposable> disposableeventlist = new List<IDisposable> { };
    private int editcursor = 0;
    private int executecursor = 0;

    private Subject<Unit> executesubject = new Subject<Unit>();
    public IObservable<Unit> ExecuteFinished
    {
        get { return executesubject; }
    }

    public void AddAct()
    {
        ActionCommand action = new MoveCommand();
        commandlist.Add(action);
        //現在指定されているActionCommandをnewで作り、その結果出来たものを格納する
    }

    public void RemoveAct()
    {
        //Removeact
    }

    public void ExecuteNextAction(ActionCommand action)
    {
        //今さっき実行したアクションによってexecutecursorを決める
        executecursor += 1;

        if (executecursor == commandlist.Count)
        {
            FinishExecute();
            return;
        }

        print("Movetonextaction");
        commandlist[executecursor].Action();
    }

    public void StartExecute()
    {
        if (commandlist.Count == 0)
        {
            print("Commandが一切ないのでパス");
            executesubject.OnNext(Unit.Default);
            return;
        }

        //現在ある全コマンドのactionfinishedイベントを購読する
        foreach (ActionCommand command in commandlist)
        {
            var disposableevent = command.ActionFinished.Delay(System.TimeSpan.FromSeconds(1.0f)).Subscribe(x => ExecuteNextAction(x));
            disposableeventlist.Add(disposableevent);
        }

        executecursor = 0;
        commandlist[0].Action();
    }

    public void FinishExecute()
    {
        //現在ある全コマンドのactionfinishedイベントの購読を停止する
        foreach (IDisposable events in disposableeventlist)
        {
            events.Dispose();
        }
        disposableeventlist.Clear();
        executesubject.OnNext(Unit.Default);
    }

    public void ResetCommand()
    {
        commandlist.Clear();
        UpdateUI();
    }

    public void CursorUp()
    {
        if (commandlist.Count < editcursor) editcursor += 1;
        UpdateUI();
    }

    public void CursorDown()
    {
        if (editcursor > 0) editcursor -= 1;
        UpdateUI();
    }

    public void UpdateUI()
    {
        //UpdateUI
    }
}
