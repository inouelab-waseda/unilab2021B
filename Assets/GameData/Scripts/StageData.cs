using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create StageData")]
public class StageData : ScriptableObject
{
    public GameObject Stage_Tilemap;
    public Vector2 start_position;
    public Vector2 goal_position;
    public PlayerController.Rotation start_direction;
    public List<EnemyData> enemies;
    public List<Vector2> holes;
    public float cameraview;
    public Vector2 cameratransform;
}