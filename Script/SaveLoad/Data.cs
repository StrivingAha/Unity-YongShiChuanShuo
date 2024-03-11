using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //保存场景
    public string sceneToSave;

    //角色坐标信息
    public Dictionary<string, Vector3> characterPosDic = new Dictionary<string, Vector3>();
    //角色float类型的信息如血量...
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    //工厂模式；将GameSceneSO类型转换为string类型
    public void SaveGameScene(GameSceneSO gameSceneSO)
    {
        //JsonUtility可以将object类型转换为json类型
        sceneToSave = JsonUtility.ToJson(gameSceneSO);
        //Debug.Log(sceneToSave);
    }

    //将json再转换为GameSceneSO
    public GameSceneSO GetGameSceneSO()
    {
        //创建一个空的GameSceneSO实例
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        //将json再反json回去
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);

        return newScene;
    }
}
