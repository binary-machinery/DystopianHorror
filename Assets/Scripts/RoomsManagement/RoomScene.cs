﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomScene : MonoBehaviour
{

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private Predicate predicate;

    [SerializeField]
    private Transform[] collectiblePlaceholders;

    private Scene scene;
    private bool collectibleFound = false;

    public bool CheckPredicate(WorldState world)
    {
        if (predicate == null)
        {
            return true;
        }
        return predicate.Check(world);
    }

    public GameObject GetRoot()
    {
        return gameObject;
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public Transform GetCollectiblePlaceholder()
    {
        if (collectibleFound || (collectiblePlaceholders.Length == 0))
        {
            return null;
        }
        int index = Random.Range(0, collectiblePlaceholders.Length);
        return collectiblePlaceholders[index];
    }

    public void SetScene(Scene scene)
    {
        this.scene = scene;
    }

    public void SetEnabled(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public bool IsValid()
    {
        return scene.IsValid();
    }

	// Use this for initialization
	void Awake () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}