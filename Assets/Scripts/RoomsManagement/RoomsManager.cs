﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsManager : MonoBehaviour
{

    [SerializeField]
    private string id;

    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private bool loadPrologue = true;

    [SerializeField]
    private string prologueSceneName;

    [SerializeField]
    private bool loadHall = true;

    [SerializeField]
    private string hallSceneName;

    [SerializeField]
    private bool loadPositiveEpilogue = true;

    [SerializeField]
    private string positiveEpilogueSceneName;

    [SerializeField]
    private bool loadNegativeEpilogue = true;

    [SerializeField]
    private string negativeEpilogueSceneName;

    [SerializeField]
    private string[] roomNames;

    private static Dictionary<string, RoomsManager> managers = new Dictionary<string, RoomsManager>();
    private List<RoomEntry> roomEntries = new List<RoomEntry>();
    private List<RoomScene> roomScenes = new List<RoomScene>();
    private RoomScene prologueScene;
    private RoomScene hallScene;
    private RoomScene positiveEpilogueScene;
    private RoomScene negativeEpilogueScene;

    public static RoomsManager GetManager(string sceneName)
    {
        return managers[sceneName];
    }

    void Awake()
    {
        managers.Add(id, this);

        if (loadPrologue)
        {
            SceneManager.LoadSceneAsync(prologueSceneName, LoadSceneMode.Additive);
        }
        StartCoroutine(WaitForLoadingAndInitRoomScene(prologueSceneName));

        if (loadHall)
        {
            SceneManager.LoadSceneAsync(hallSceneName, LoadSceneMode.Additive);
        }
        StartCoroutine(WaitForLoadingAndInitRoomScene(hallSceneName));

        if (loadPositiveEpilogue)
        {
            SceneManager.LoadSceneAsync(positiveEpilogueSceneName, LoadSceneMode.Additive);
        }
        StartCoroutine(WaitForLoadingAndInitRoomScene(positiveEpilogueSceneName));

        if (loadNegativeEpilogue)
        {
            SceneManager.LoadSceneAsync(negativeEpilogueSceneName, LoadSceneMode.Additive);
        }
        StartCoroutine(WaitForLoadingAndInitRoomScene(negativeEpilogueSceneName));

        foreach (string room in roomNames)
        {
            SceneManager.LoadSceneAsync(room, LoadSceneMode.Additive);
            StartCoroutine(WaitForLoadingAndInitRoomScene(room));
        }
    }

    public void RegisterRoomEntry(RoomEntry door)
{
    roomEntries.Add(door);
}

public RoomEntry GetRandomRoomEntry()
{
    List<RoomEntry> filtered = new List<RoomEntry>(roomEntries.Count);
    foreach (RoomEntry re in roomEntries)
    {
        if (re.CheckPredicate(worldState))
        {
            filtered.Add(re);
        }
    }
    int index = Random.Range(0, filtered.Count);
    return filtered[index];
}

public RoomScene GetRandomRoomScene()
{
    List<RoomScene> filtered = new List<RoomScene>(roomScenes.Count);
    foreach (RoomScene rs in roomScenes)
    {
        if (rs.CheckPredicate(worldState))
        {
            filtered.Add(rs);
        }
    }
    int index = Random.Range(0, filtered.Count);
    return filtered[index];
}

public void UnloadPrologue()
{
    prologueScene.SetEnabled(false);
    SceneManager.UnloadSceneAsync(prologueSceneName);
}

private IEnumerator WaitForLoadingAndInitRoomScene(string sceneName)
{
    Scene scene = SceneManager.GetSceneByName(sceneName);
    if (!scene.IsValid())
    {
        yield break;
    }
    yield return new WaitUntil(() => scene.isLoaded);
    GameObject root = scene.GetRootGameObjects()[0];
    RoomScene roomScene = root.GetComponent<RoomScene>();
    roomScene.SetScene(scene);

    // hack, i don't like it
    if (sceneName.Equals(prologueSceneName))
    {
        prologueScene = roomScene;
    }
    else if (sceneName.Equals(hallSceneName))
    {
        hallScene = roomScene;
    }
    else if (sceneName.Equals(positiveEpilogueSceneName))
    {
        positiveEpilogueScene = roomScene;
    }
    else if (sceneName.Equals(negativeEpilogueSceneName))
    {
        negativeEpilogueScene = roomScene;
    }
    else
    {
        root.transform.position = new Vector3(-100, 0, -100);
        roomScenes.Add(roomScene);
        yield return null;
        yield return null;
        roomScene.SetEnabled(false);
    }
}

}
