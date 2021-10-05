using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class ActManager : MonoBehaviour
{
    public GameManager gameManager;
    public PlayUIManager UIManager;
    [SerializeField]
    private List<string> commandlist_string = new List<string> { };

    [SerializeReference]
    private List<ActionCommand> commandlist = new List<ActionCommand> { };

    private List<IDisposable> disposableeventlist = new List<IDisposable> { };
    [SerializeField]
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

        commandlist_string.Insert(editcursor,UIManager.GetcommandfromUI());
        //現在指定されているActionCommandをnewで作り、その結果出来たものを格納する
        if (UIManager.GetcommandfromUI() == "movefront")
        {
            ActionCommand action = new MoveCommand();
            commandlist.Insert(editcursor,action);
        } else if (UIManager.GetcommandfromUI() == "turnright")
        {
            ActionCommand action = new RotateCommand("right");
            commandlist.Insert(editcursor, action);
        } else if (UIManager.GetcommandfromUI() == "turnleft")
        {
            ActionCommand action = new RotateCommand("left");
            commandlist.Insert(editcursor, action);
        } else if (UIManager.GetcommandfromUI() == "3times")
        {
            ActionCommand foraction = new PassCommand();
            ActionCommand gotoaction = new GotoCommand(foraction, 3);
            commandlist.Insert(editcursor, foraction);
            CursorUp();
            commandlist.Insert(editcursor, gotoaction);
            commandlist_string.Insert(editcursor, "ForloopEnd");
        } else if (UIManager.GetcommandfromUI() == "checkfrontwall")
        {
            ActionCommand passaction = new PassCommand();
            ActionCommand ifaction = new FrontCheckCommand(passaction, "wall", true);
            commandlist.Insert(editcursor, ifaction);
            CursorUp();
            commandlist.Insert(editcursor, passaction);
            commandlist_string.Insert(editcursor, "IFEnd");
        } else
        {
            Debug.LogAssertionFormat("{}というコマンドはないまたは実装されてません", UIManager.GetcommandfromUI());
        }
        CursorUp();

        UpdateUI();
    }

    public void RemoveAct()
    {
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        UpdateUI();
        //Removeact
    }

    private void ExecuteNextAction(ActionCommand action)
    {
        //今さっき実行したアクションによってexecutecursorを決める
        if (action.nextcursor != null)
        {
            executecursor = commandlist.IndexOf(action.nextcursor);
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
        Debug.Log(commandlist.Count);
        if (commandlist.Count > editcursor) editcursor += 1;
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
