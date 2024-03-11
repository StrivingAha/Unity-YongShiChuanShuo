using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;//事件的头文件

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VideoShakeSO newGameEvent;

    [Header("基础属性")]
    public float maxHealth;
    public float currHealth;

    //受伤无敌
    public float invulnerableDuration=2;
    private float invulnerableCounter;
    public bool invulnerable;

    //unity事件
    public UnityEvent<Transform> onTakeDamage;
    public UnityEvent onDead;
    public UnityEvent<Character> onHealthChange;

    private float fadeTime=0.5f;


    //newGame野猪血量未赋值，在start初始赋值
    private void Start()
    {
        currHealth = maxHealth;
    }

    private void newGame()
    {
        //currHealth = maxHealth;

        onHealthChange?.Invoke(this);
    }

    private void Update()
    {
        invulnerableCounter -= Time.deltaTime;
        if(invulnerableCounter <= 0)
        {
            invulnerable = false;
        }
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += newGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= newGame;
        ISaveable saveable = this;
        saveable.UnregisterSaveData();
    }

    //受伤
    public void takeDamage(Attack attacker)
    {
        if (invulnerable) return;

        if(currHealth-attacker.damage>0)
        {
            currHealth -= attacker.damage;
            triggerInvulnerable();
            //执行受伤
            onTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currHealth = 0;

            //触发死亡
            onDead?.Invoke();
        }

        //UI
        onHealthChange?.Invoke(this);
    }

    //触发无敌
    private void triggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }


    //存储点逻辑
    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        //存在数据时
        if (data.characterPosDic.ContainsKey(GetDataID().ID))
        {
            //存储位置
            data.characterPosDic[GetDataID().ID] = transform.position;
            //存储血量
            data.floatSaveData[GetDataID().ID + "health"] =  this.currHealth;
        }
        //不存在数据时
        else
        {
            data.characterPosDic.Add(GetDataID().ID, transform.position);
            data.floatSaveData.Add(GetDataID().ID + "health", this.currHealth);
        }
    }

    public void LoadSaveData(Data data)
    {
        if(data.characterPosDic.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDic[GetDataID().ID];
            this.currHealth = data.floatSaveData[GetDataID().ID + "health"];
        }

        //updata ui
        onHealthChange?.Invoke(this);
    }
}
