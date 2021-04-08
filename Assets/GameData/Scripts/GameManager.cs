using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private States _currentState = States.Idle;
    public States CurrentState { get { return _currentState; } }

    /// <summary>
    /// ゲーム現在のステート
    /// </summary>
    public enum States
    {
        Idle,
        Execute,
        Initialize,
        Hint,
        Result,
        Count
    }

    protected override void Awake()
    {
        SetState(States.Initialize);
    }

    public void SetState(States state)
    {
        if (state == CurrentState) return;
        if (CurrentState == States.Result) return;
        switch(state) {
            case States.Idle:
                ResetPlace();
                break;
            case States.Execute:
                StartExecute();
                break;
            case States.Initialize:
                Initialize();
                break;
            case States.Hint:
                break;
            case States.Result:
                break;
            default:
                Debug.LogWarningFormat("{}への遷移に制限がかけられていません",state.ToString());
                break;
        }
        _currentState = state;
    }

    /// <summary>
    /// OnClick()呼出し用の関数(引数にEnumの指定ができないため)
    /// </summary>
    /// <param name="state"></param>
    public void SetState(int state)
    {
        if (state < (int)States.Count) SetState((States)state);
        else Debug.LogWarning("指定されたステートは存在しません");
    }

    //UIで設定した通りにプレイヤーが動き始める
    public void StartExecute()
    {
        Debug.Log("実行開始");
        _currentState = States.Execute;
    }

    //データを初期配置に戻す
    public void ResetPlace()
    {
        Debug.Log("リセット開始");
        _currentState = States.Idle;
    }

    //Scriptableobjectからステージを読み込み生成する
    public void Initialize()
    {
        Debug.Log("初期化開始");
        _currentState = States.Initialize;
        //uniRxの購読
        Debug.Log("初期化終了");
        _currentState = States.Idle;
    }

}
