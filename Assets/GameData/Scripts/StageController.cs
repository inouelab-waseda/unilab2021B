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
    private Tilemap walltile;

    public StageData StartInitialize(string stage_num)
    {
        stage_data = Resources.Load<StageData>(assetpath + stage_num);
        if (stage_data == null) Debug.LogAssertion("Failure at ResouceLoad");
        MakeStage(stage_data.Stage_Tilemap);
        MakeEnemy(stage_data.enemies);
        return stage_data;
    }

    private void MakeStage(GameObject stage)
    {
        stageobj = Instantiate(stage, Vector3.zero, Quaternion.identity) as GameObject;
        stageobj.transform.parent = this.gameObject.transform;
        Debug.Log("Stageê∂ê¨");
    }

    private void MakeEnemy(EnemyData[] enemy)
    {
        
    }

    public bool WallExists(Vector3 pos)
    {
        if (walltile == null) walltile = stageobj.transform.Find("Wall").GetComponent<Tilemap>();
        if (walltile.HasTile(walltile.WorldToCell(pos))) return true;
        return false;
    }
}
