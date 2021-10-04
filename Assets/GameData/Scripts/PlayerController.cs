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

    SpriteRenderer player_spriterend;

    private Subject<Vector2> playersubject = new Subject<Vector2>();
    public IObservable<Vector2> CheckedReachedGoal
    {
        get { return playersubject; }
    }

    public enum Rotation
    {
        front,
        back,
        right,
        left
    };

    public Vector2 GetPlayerpos() => player.transform.position;

    public void Start()
    {
        FindPlayer();
        player_spriterend = player.GetComponent<SpriteRenderer>();

    }

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

        switch (direction)
        {
            case Rotation.front:
                //spriterend.sprite = 
                break;
            case Rotation.back:
                //player.transform.position += new Vector3(0.0f, 1.0f);
                break;
            case Rotation.right:
                //player.transform.position += new Vector3(1.0f, 0.0f);
                break;
            case Rotation.left:
                //player.transform.position += new Vector3(-1.0f, 0.0f);
                break;
        }
    }

    private void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void PlayerMove()
    {
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

    public void PlayerRotate()
    {
        UpdateSprite();
        //CommandListからイベントが飛ばされたときに、一度だけ動かす
    }
}
