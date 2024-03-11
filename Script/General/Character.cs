using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;//�¼���ͷ�ļ�

public class Character : MonoBehaviour,ISaveable
{
    [Header("�¼�����")]
    public VideoShakeSO newGameEvent;

    [Header("��������")]
    public float maxHealth;
    public float currHealth;

    //�����޵�
    public float invulnerableDuration=2;
    private float invulnerableCounter;
    public bool invulnerable;

    //unity�¼�
    public UnityEvent<Transform> onTakeDamage;
    public UnityEvent onDead;
    public UnityEvent<Character> onHealthChange;

    private float fadeTime=0.5f;


    //newGameҰ��Ѫ��δ��ֵ����start��ʼ��ֵ
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

    //����
    public void takeDamage(Attack attacker)
    {
        if (invulnerable) return;

        if(currHealth-attacker.damage>0)
        {
            currHealth -= attacker.damage;
            triggerInvulnerable();
            //ִ������
            onTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currHealth = 0;

            //��������
            onDead?.Invoke();
        }

        //UI
        onHealthChange?.Invoke(this);
    }

    //�����޵�
    private void triggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }


    //�洢���߼�
    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        //��������ʱ
        if (data.characterPosDic.ContainsKey(GetDataID().ID))
        {
            //�洢λ��
            data.characterPosDic[GetDataID().ID] = transform.position;
            //�洢Ѫ��
            data.floatSaveData[GetDataID().ID + "health"] =  this.currHealth;
        }
        //����������ʱ
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
