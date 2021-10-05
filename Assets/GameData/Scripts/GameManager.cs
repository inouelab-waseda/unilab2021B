using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    public StageController StageController;
    public PlayerController PlayerController;
    public ActManager ActManager;
    private States _currentState = States.Idle;
    public States CurrentState { get { return _currentState; } }

    private string _stage = null;
    public string Stage { get { return _stage; } set { _stage = value; } }
    private StageData stage_data = null;

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
        base.Awake();
    }

    private void Start()
    {
        print(_stage);
        SetState(States.Initialize);

        ActManager.ExecuteFinished.Delay(System.TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
        {
            Debug.Log("実行状態終了");
            SetState(States.Idle);
        });
        PlayerController.CheckedReachedGoal.Where(x => x == stage_data.goal_position).Subscribe(_ =>
        {
            Debug.Log("プレイヤーがゴールに到達");
            StopExecute();
            SetState(States.Result);
        });
    }

    public void SetState(States state)
    {
        if (state == CurrentState) return;
        if (CurrentState == States.Result) return;
        switch (state) {
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
                StageClear();
                break;
            default:
                Debug.LogWarningFormat("{}への遷移に制限がかけられていません",state.ToString());
                break;
        }
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
        if (_currentState != States.Idle) return;
        Debug.Log("実行開始");
        _currentState = States.Execute;
        ActManager.StartExecute();
    }

    //Stopボタンを押した
    public void StopExecute()
    {
        if (CurrentState == States.Execute) ActManager.FinishExecute();
        else if (CurrentState == States.Idle) ActManager.ResetCommand();
    }

    //データを初期配置に戻す
    public void ResetPlace()
    {
        Debug.Log("リセット開始");
        PlayerController.SetPlayerpos(stage_data.start_position);
        PlayerController.SetDirection(stage_data.start_direction);
        _currentState = States.Idle;
        Debug.Log("リセット終了");
    }

    //Scriptableobjectからステージを読み込み生成する
    public void Initialize()
    {
        Debug.Log("初期化開始");
        _currentState = States.Initialize;
        if (_stage == null)
        {
            Debug.LogAssertion("読み込まれるStageがnullのため、Stage1に設定します");
            _stage = "Stage1";
        }
        stage_data = StageController.StartInitialize(_stage);
        if (stage_data.cameraview <= 0) Camera.main.orthographicSize = 10;
        else Camera.main.orthographicSize = stage_data.cameraview;
        ResetPlace();
        //uniRxの購読
        Debug.Log("初期化終了");
    }

    public void StageClear()
    {
        Debug.Log(stage_data.name);
    }

}
