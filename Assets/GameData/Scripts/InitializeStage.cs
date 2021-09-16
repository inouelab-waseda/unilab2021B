using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InitializeStage : MonoBehaviour
{
    private StageData stage_data;
    private const string assetpath = "ScriptableObject/StageData/";

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
        GameObject stageobj = Instantiate(stage, Vector3.zero, Quaternion.identity) as GameObject;
        stageobj.transform.parent = this.gameObject.transform;
        Debug.Log("Stageê∂ê¨");
    }

    private void MakeEnemy(EnemyData[] enemy)
    {
        
    }
}
