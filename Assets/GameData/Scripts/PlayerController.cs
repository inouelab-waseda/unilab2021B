using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;

    public Vector2 GetPlayerpos() => player.transform.position;

    public void SetPlayerpos(Vector2 pos)
    {
        if (player == null) FindPlayer();
        Debug.Log(player);
        player.transform.position = pos;
    }

    private void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void PlayerMove()
    {
        //CommandListからイベントが飛ばされたときに、一度だけ動かす

    }

    public void PlayerRotate()
    {
        
    }
}
