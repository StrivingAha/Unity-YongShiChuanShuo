using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerHealth playerhealth;

    [Header("�¼�����")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO sceneUnloadEvent;
    public VideoShakeSO LoadDataEvent;
    public VideoShakeSO backToMenuEvent;
    public FloatEventSO syncVolumeEvent;

    [Header("�㲥")]
    public VideoShakeSO pauseEvent;

    [Header("���")]
    public GameObject gameOverPanel;
    public GameObject mobileDevice;
    public GameObject PausePanel;
    public Button settingPauseButton;
    public Slider VolumeSlider;

    private void Awake()
    {
        //UNITY_STANDALONE����������
#if UNITY_STANDALONE
        mobileDevice.SetActive(false);
#endif

        settingPauseButton.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable()
    {
        healthEvent.onEventRaised += OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent += OnLoadEvent;
        LoadDataEvent.onEventRaised += OnLoadDataEvent;
        backToMenuEvent.onEventRaised += OnLoadDataEvent;
        syncVolumeEvent.onEventRaised += onsyncVolumeEvent;
    }


    private void OnDisable()
    {
        healthEvent.onEventRaised -= OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent -= OnLoadEvent;
        LoadDataEvent.onEventRaised -= OnLoadDataEvent;
        backToMenuEvent.onEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.onEventRaised -= onsyncVolumeEvent;
    }

    private void onsyncVolumeEvent(float amount)
    {
        VolumeSlider.value = (amount + 30) / 50;
    }

    private void TogglePausePanel()
    {
        if(PausePanel.activeInHierarchy)
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseEvent.eventRaised();
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
            
    }

    //�������һ�Σ������ʧ
    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    //��menu�˵�ʱ����ʾ��ɫѪ����Ϣ
    private void OnLoadEvent(GameSceneSO sceneLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneLoad.sceneType == SceneType.Menu;
        playerhealth.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currHealth / character.maxHealth;
        playerhealth.HealthChange(percentage);
    }
}
