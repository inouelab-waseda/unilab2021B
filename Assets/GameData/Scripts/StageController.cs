using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Tilemaps;

public class StageController : MonoBehaviour
{
    private StageData stage_data;
    private const string assetpath = "ScriptableObject/StageData/";
    private GameObject stageobj;
    private GameObject goalflag;
    private List<GameObject> enemiesobj = new List<GameObject> { };
    private Tilemap walltile;

    public StageData StartInitialize(string stage_num)
    {
        stage_data = Resources.Load<StageData>(assetpath + stage_num);
        if (stage_data == null) Debug.LogAssertion("Failure at ResouceLoad");
        MakeStage(stage_data.Stage_Tilemap);
        MakeEnemy();
        MakeGoal();
        return stage_data;
    }

    private void MakeStage(GameObject stage)
    {
        stageobj = Instantiate(stage, Vector3.zero, Quaternion.identity) as GameObject;
        stageobj.transform.parent = this.gameObject.transform;
        Debug.Log("Stageê∂ê¨");
    }

    private void MakeEnemy()
    {
        foreach (var e in stage_data.enemies) {
            var enemyobj = Instantiate(e.enemyimage, e.position, Quaternion.identity) as GameObject;
            enemyobj.transform.parent = this.gameObject.transform;
            enemiesobj.Add(enemyobj);
        }
    }

    private void MakeGoal()
    {
        if (goalflag == null) goalflag = this.transform.Find("goal").gameObject;
        goalflag.transform.position = stage_data.goal_position;
    }

    public bool WallExists(Vector3 pos)
    {
        if (walltile == null) walltile = stageobj.transform.Find("Wall").GetComponent<Tilemap>();
        if (walltile.HasTile(walltile.WorldToCell(pos))) return true;
        return false;
    }

    public bool EnemyExists(Vector3 pos)
    {
        if (enemiesobj == null) return false;
        foreach (var e in enemiesobj)
        {
            if (e is null) continue;
            if (e.transform.position == pos) return true;
        }
        return false;
    }

    public void InitializeEnemy()
    {
        if (enemiesobj == null) return;
        foreach (var e in enemiesobj)
        {
            Destroy(e);
        }
        enemiesobj.Clear();
        MakeEnemy();
    }

    public GameObject GetEnemy(Vector3 pos)
    {
        if (enemiesobj == null) return null;
        foreach (var e in enemiesobj)
        {
            if (e is null) continue;
            if (e.transform.position == pos) return e;
        }
        return null;
    }

    public void RemoveEnemy(Vector3 pos)
    {
        var e = GetEnemy(pos);
        if (e is null) return;
        enemiesobj.Remove(e);
        Destroy(e);
    }
}
