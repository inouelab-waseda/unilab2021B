using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private Rotation direction;

    private SpriteRenderer player_spriterend;

    public StageController Stagecontroller;

    public Sprite player_front;
    public Sprite player_back;
    public Sprite player_right;
    public Sprite player_left;

    private Subject<Vector2> playersubject = new Subject<Vector2>();
    public IObservable<Vector2> CheckedReachedGoal
    {
        get { return playersubject; }
    }

    public enum Rotation
    {
        front = 0,
        right = 1,
        back = 2,
        left = 3
    };

    public Vector2 GetPlayerpos() => player.transform.position;

    /*public void Start()
    {
        FindPlayer();
        player_spriterend = player.GetComponent<SpriteRenderer>();

    }*/

    public void SetPlayerpos(Vector2 pos)
    {
        if (player == null) FindPlayer();
        player.transform.position = pos;
    }

    public void SetDirection(Rotation rotation)
    {
        direction = rotation;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (player == null) FindPlayer();
        if (player_spriterend == null) player_spriterend = player.GetComponent<SpriteRenderer>();

        switch (direction)
        {
            case Rotation.front:
                player_spriterend.sprite = player_front;
                break;
            case Rotation.back:
                player_spriterend.sprite = player_back;
                break;
            case Rotation.right:
                player_spriterend.sprite = player_right;
                break;
            case Rotation.left:
                player_spriterend.sprite = player_left;
                break;
        }
    }

    private void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    public bool FrontWallExists()
    {
        switch (direction)
        {
            case Rotation.front:
                return Stagecontroller.WallExists(player.transform.position + new Vector3(0.0f, -1.0f));
            case Rotation.back:
                return Stagecontroller.WallExists(player.transform.position + new Vector3(0.0f, 1.0f));
            case Rotation.right:
                return Stagecontroller.WallExists(player.transform.position + new Vector3(1.0f, 0.0f));
            case Rotation.left:
                return Stagecontroller.WallExists(player.transform.position + new Vector3(-1.0f, 0.0f));
        }
        Debug.LogAssertion("Rotationが例外です");
        return true;
    }

    public void PlayerMove()
    {
        if (FrontWallExists()) return;
        switch (direction)
        {
            case Rotation.front:
                player.transform.position += new Vector3(0.0f, -1.0f);
                break;
            case Rotation.back:
                player.transform.position += new Vector3(0.0f, 1.0f);
                break;
            case Rotation.right:
                player.transform.position += new Vector3(1.0f, 0.0f);
                break;
            case Rotation.left:
                player.transform.position += new Vector3(-1.0f, 0.0f);
                break;
        }

        //ゴールチェックイベントを発行する
        //GameManagerはそのイベントを見て、まずStopを呼び出し、その後Resultへ移行する
        playersubject.OnNext(GetPlayerpos());

    }

    public void PlayerRotate(string dir)
    {
        if (dir == "right")
        {
            if (direction == Rotation.front) direction = (Rotation)3;
            else direction -= 1;

        } else if (dir == "left")
        {
            if (direction == Rotation.left) direction = (Rotation)0;
            else direction += 1;
        } else
        {
            Debug.LogAssertion("方向設定がrightまたはleft以外になっています");
        }
        UpdateSprite();
    }
}
