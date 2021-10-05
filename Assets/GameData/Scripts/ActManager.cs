using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class ActManager : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField]
    private List<string> commandlist_string = new List<string> { };

    [SerializeReference]
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
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        ActionCommand action = new PassCommand();
        ActionCommand action8 = new PassCommand();
        ActionCommand action9 = new GotoCommand(action8, 2);
        ActionCommand action2 = new GotoCommand(action, 3);
        ActionCommand action4 = new RotateCommand("right");
        ActionCommand action3 = new MoveCommand();
        commandlist.Add(action);
        commandlist.Add(action8);
        commandlist.Add(action3);
        commandlist.Add(action9);
        commandlist.Add(action4);
        commandlist.Add(action2);
        //現在指定されているActionCommandをnewで作り、その結果出来たものを格納する
    }

    public void RemoveAct()
    {
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        //Removeact
    }

    public void ExecuteNextAction(ActionCommand action)
    {
        //今さっき実行したアクションによってexecutecursorを決める
        if (action.nextcursor != null)
        {
            executecursor = commandlist.IndexOf(action.nextcursor);
            Debug.Log(executecursor);
        }
        else executecursor += 1;

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
            command.Initialize();
            var disposableevent = command.ActionFinished.Delay(System.TimeSpan.FromSeconds(command.delaytime)).Subscribe(x => ExecuteNextAction(x));
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
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        commandlist.Clear();
        UpdateUI();
    }

    public void CursorUp()
    {
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        if (commandlist.Count < editcursor) editcursor += 1;
        UpdateUI();
    }

    public void CursorDown()
    {
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        if (editcursor > 0) editcursor -= 1;
        UpdateUI();
    }

    public void UpdateUI()
    {
        //UpdateUI
    }
}
