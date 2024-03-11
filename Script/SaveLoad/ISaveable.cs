using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    DataDefination GetDataID();

    void RegisterSaveData() => DataManger.instance.RegisterSaveData(this);

    void UnregisterSaveData() => DataManger.instance.UnRegisterSaveData(this);

    void GetSaveData(Data data);
    void LoadSaveData(Data data); 
}
