using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public GameObject enemyimage;
	public int maxHp;
	public Vector2 position;
	public int damage;
}