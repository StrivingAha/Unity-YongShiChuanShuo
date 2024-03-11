using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class SceneLoad : MonoBehaviour,ISaveable
{
    public Transform playerTransform;
    public Vector3 firstPosition;
    public Vector3 menuPosition;

    [Header("事件监听")]
    public SceneLoadEventSO LoadEventSO;
    public VideoShakeSO newgameEventSO;
    public VideoShakeSO backToMenuEvent;

    [Header("广播")]
    public VideoShakeSO afterSceneEventSO;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO UnloadedSceneEvent;

    [Header("场景")]
    public GameSceneSO menuScene;
    public GameSceneSO firstScene;
    private GameSceneSO currLoadedScene;
    private GameSceneSO locationToLoad;
    private Vector3 positionTogo;
    private bool fadeScreen;
    private bool isLoading;

    public float fadeTime=0.5f;

    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstScene.sceneReference, LoadSceneMode.Additive);
        //currLoadedScene = firstScene;
        //currLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        LoadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnEnable()
    {
        LoadEventSO.LoadRequestEvent += OnLoadEventSO;
        newgameEventSO.onEventRaised += NewGame;

        //无法注册，只能迁移到start中注册
        //ISaveable saveable = this;
        //saveable.RegisterSaveData();

        backToMenuEvent.onEventRaised += OnbackToMenuEvent;
    }

    private void OnDisable()
    {
        LoadEventSO.LoadRequestEvent -= OnLoadEventSO;
        newgameEventSO.onEventRaised -= NewGame;

        ISaveable saveable = this;
        saveable.UnregisterSaveData();

        backToMenuEvent.onEventRaised -= OnbackToMenuEvent;
    }

    private void OnbackToMenuEvent()
    {
        locationToLoad = menuScene;
        LoadEventSO.RaiseLoadRequestEvent(locationToLoad, menuPosition, true);
    }

    private void NewGame()
    {
        locationToLoad = firstScene;
        //OnLoadEventSO(locationToLoad, firstPosition, true);
        LoadEventSO.RaiseLoadRequestEvent(locationToLoad,firstPosition,true);
    }


    private void OnLoadEventSO(GameSceneSO LocationToLoad, Vector3 posTogo, bool FadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;

        locationToLoad = LocationToLoad;
        positionTogo = posTogo;
        fadeScreen = FadeScreen;

        if(currLoadedScene != null)
            StartCoroutine(UnloadPreviousScene());
        else
            LoadNewScene();

    }

    //使用协程卸载场景
    private IEnumerator UnloadPreviousScene()
    {
        if (fadeScreen)
        {
            fadeEvent.fadeIn(fadeTime);
        }

        yield return new WaitForSeconds(fadeTime);

        UnloadedSceneEvent.RaiseLoadRequestEvent(locationToLoad,positionTogo,true);
        
        yield return currLoadedScene.sceneReference.UnLoadScene() ;

        //关闭人物
        playerTransform.gameObject.SetActive(false);

        //加载新场景
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = locationToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadingCompleted;
    }


    //场景加载完成之后更新当前场景变量值
    private void OnLoadingCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currLoadedScene = locationToLoad;
        playerTransform.position = positionTogo;
        playerTransform.gameObject.SetActive(true);

        if(fadeScreen)
        {
            fadeEvent.FadeOut(fadeTime);
        }

        isLoading = false;

        if(currLoadedScene.sceneType ==SceneType.Location)
            afterSceneEventSO.eventRaised();
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currLoadedScene);
    }

    public void LoadSaveData(Data data)
    {
        //场景没有ID，通过玩家ID来判断是否有存储点
        var playerID = playerTransform.GetComponent<DataDefination>().ID;
        if (data.characterPosDic.ContainsKey(playerID))
        {
            positionTogo = data.characterPosDic[playerID];
            locationToLoad = data.GetGameSceneSO();

            OnLoadEventSO(locationToLoad, positionTogo, true);
            
        }
    }

}
