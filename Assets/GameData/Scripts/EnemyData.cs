using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]
public class EnemyData : ScriptableObject
{
	public int maxHp;
	public int X_Position;
	public int Y_Position;
	public int damage;
}