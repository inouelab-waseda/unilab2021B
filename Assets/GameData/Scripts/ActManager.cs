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

        int scope;
        if (editcursor <= 0) scope = 0;
        else scope = commandlist[editcursor - 1].nextscope;

        //現在指定されているActionCommandをnewで作り、その結果出来たものを格納する
        if (UIManager.GetcommandfromUI() == "movefront")
        {
            ActionCommand action = new MoveCommand(UIManager.GetcommandfromUI(),scope);
            commandlist.Insert(editcursor,action);
        } else if (UIManager.GetcommandfromUI() == "turnright")
        {
            ActionCommand action = new RotateCommand(UIManager.GetcommandfromUI(), scope, "right");
            commandlist.Insert(editcursor, action);
        } else if (UIManager.GetcommandfromUI() == "turnleft")
        {
            ActionCommand action = new RotateCommand(UIManager.GetcommandfromUI(), scope, "left");
            commandlist.Insert(editcursor, action);
        } else if (UIManager.GetcommandfromUI() == "forloop")
        {
            ActionCommand foraction = new PassCommand("forStart", scope + 1, int.Parse(UIManager.GetInputdata()));
            ActionCommand gotoaction = new GotoCommand("forEnd", scope+1, foraction, int.Parse(UIManager.GetInputdata()));
            commandlist.Insert(editcursor, foraction);
            CursorUp();
            commandlist.Insert(editcursor, gotoaction);
        } else if (UIManager.GetcommandfromUI() == "IFstartwall")
        {
            ActionCommand passaction = new PassCommand("IfEnd", scope+1);
            ActionCommand ifaction = new FrontCheckCommand("IfStart", scope+1, passaction, "wall", true);
            commandlist.Insert(editcursor, ifaction);
            CursorUp();
            commandlist.Insert(editcursor, passaction);
        } else if (UIManager.GetcommandfromUI() == "IFstartenemy")
        {
            ActionCommand passaction = new PassCommand("IfEnd", scope + 1);
            ActionCommand ifaction = new FrontCheckCommand("IfStart", scope + 1, passaction, "enemy", true);
            commandlist.Insert(editcursor, ifaction);
            CursorUp();
            commandlist.Insert(editcursor, passaction);
        } else if (UIManager.GetcommandfromUI() == "Attack")
        {
            ActionCommand action = new AttackCommand(UIManager.GetcommandfromUI(), scope);
            commandlist.Insert(editcursor, action);
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
        if (editcursor <= 0) return;
        if ((commandlist[editcursor - 1].commandname == "IfStart") || (commandlist[editcursor - 1].commandname == "forStart"))
        {
            int pairindex = 0;
            for (int i = editcursor; i < commandlist.Count; i++)
            {
                if (commandlist[editcursor - 1].scope == commandlist[i].scope)
                {
                    if ((commandlist[i].commandname == "IfEnd") || (commandlist[i].commandname == "forEnd"))
                    {
                        pairindex = i;
                        break;
                    }
                }
            }
            ScopeDownRange(editcursor - 1, pairindex);
            commandlist.RemoveAt(pairindex);
            commandlist.RemoveAt(editcursor - 1);

        } else if ((commandlist[editcursor - 1].commandname == "IfEnd") || (commandlist[editcursor - 1].commandname == "forEnd"))
        {
            int pairindex = 0;
            for (int i = editcursor - 2; i >= 0; i--)
            {
                if (commandlist[editcursor - 1].scope == commandlist[i].scope)
                {
                    if ((commandlist[i].commandname == "IfStart") || (commandlist[i].commandname == "forStart"))
                    {
                        pairindex = i;
                        break;
                    }
                }
            }
            //scopeダウンの設定
            ScopeDownRange(pairindex, editcursor - 1);
            commandlist.RemoveAt(editcursor - 1);
            commandlist.RemoveAt(pairindex);
            CursorDown();
        }
        else
        {
            commandlist.RemoveAt(editcursor - 1);
        }

        CursorDown();

        UpdateUI();
    }

    private void ScopeDownRange(int start, int end)
    {

        for (int i = start; i <= end; i++)
        {
            commandlist[i].scopeDown();
        }
    }

    private void ExecuteNextAction(ActionCommand action)
    {

        //現在のexecutecursorの示すUIの更新
        UIManager.executeCursorOff(executecursor);

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

        //次のexecutecursorの示すUIの更新
        UIManager.executeCursorOn(executecursor);

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
        UIManager.Deleteeditcursor();
        UIManager.executeCursorOn(0);
        commandlist[0].Action();
    }

    public void FinishExecute()
    {
        UIManager.executeCursorOff(executecursor);

        //現在ある全コマンドのactionfinishedイベントの購読を停止する
        foreach (IDisposable events in disposableeventlist)
        {
            events.Dispose();
        }
        disposableeventlist.Clear();
        UIManager.Appeareditcursor(editcursor);
        executesubject.OnNext(Unit.Default);
    }

    public void ResetCommand()
    {
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        editcursor = 0;
        commandlist.Clear();
        commandlist_string.Clear();
        UpdateUI();
    }

    public void CursorUp()
    {
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        if (commandlist.Count <= editcursor) return;
        editcursor += 1;
        UIManager.MoveeditCursor(editcursor);
    }

    public void CursorDown()
    {
        if (gameManager.CurrentState != GameManager.States.Idle) return;
        if (editcursor <= 0) return;
        editcursor -= 1;
        UIManager.MoveeditCursor(editcursor);
    }

    public void UpdateUI()
    {

        if (gameManager.CurrentState == GameManager.States.Idle)
        {
            commandlist_string = commandlist.Select(_ => _.scope.ToString()).ToList();
            UIManager.DisplayCommandlist(commandlist);
            UIManager.MoveeditCursor(editcursor);
            //Listからデータを生成
            //Cursorの位置をずらす
        }
        else return;
        //UpdateUI
    }
}
