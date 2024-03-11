using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //���泡��
    public string sceneToSave;

    //��ɫ������Ϣ
    public Dictionary<string, Vector3> characterPosDic = new Dictionary<string, Vector3>();
    //��ɫfloat���͵���Ϣ��Ѫ��...
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    //����ģʽ����GameSceneSO����ת��Ϊstring����
    public void SaveGameScene(GameSceneSO gameSceneSO)
    {
        //JsonUtility���Խ�object����ת��Ϊjson����
        sceneToSave = JsonUtility.ToJson(gameSceneSO);
        //Debug.Log(sceneToSave);
    }

    //��json��ת��ΪGameSceneSO
    public GameSceneSO GetGameSceneSO()
    {
        //����һ���յ�GameSceneSOʵ��
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        //��json�ٷ�json��ȥ
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);

        return newScene;
    }
}
