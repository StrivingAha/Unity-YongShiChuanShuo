using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManger : MonoBehaviour
{
    public static DataManger instance;
    [Header("ÊÂ¼þ¼àÌý")]
    public VideoShakeSO saveDataEvent;
    public VideoShakeSO loadDataEvent;

    private List<ISaveable> saveableList = new List<ISaveable>();

    private Data saveData;

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        saveData = new Data();
    }

    //inputsystem²âÊÔLoadº¯Êý
    private void Update()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }

    private void OnEnable()
    {
        saveDataEvent.onEventRaised += Save;
        loadDataEvent.onEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.onEventRaised -= Save;
        loadDataEvent.onEventRaised -= Load;
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
            saveableList.Add(saveable);
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach(var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }
        //foreach(var item in saveData.characterPosDic)
        //{
        //    Debug.Log(item.Key+"  "+item.Value);
        //}
    }

    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadSaveData(saveData);
        }
    }

}
