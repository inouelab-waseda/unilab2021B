using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private Rotation direction;

    public enum Rotation
    {
        front,
        back,
        right,
        left
    };

    public Vector2 GetPlayerpos() => player.transform.position;

    public void SetPlayerpos(Vector2 pos)
    {
        if (player == null) FindPlayer();
        Debug.Log(player);
        player.transform.position = pos;
    }

    public void SetDirection(Rotation rotation)
    {
        direction = rotation;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (player = null) FindPlayer();
        SpriteRenderer spriterend = player.GetComponent<SpriteRenderer>();

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
        //CommandListからイベントが飛ばされたときに、一度だけ動かす

    }

    public void PlayerRotate()
    {
        UpdateSprite();
        //CommandListからイベントが飛ばされたときに、一度だけ動かす
    }
}
