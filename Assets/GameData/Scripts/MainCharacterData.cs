using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create MainCharacterData")]
public class MainCharacterData : ScriptableObject
{
	public int maxHp;
	public int X_Position;
	public int Y_Position;
}