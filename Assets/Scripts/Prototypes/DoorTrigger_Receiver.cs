﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using EventBus;
using System;

public class DoorTrigger_Receiver : MonoBehaviour, IEventSubscriber {

    private Door door;
    private Dispatcher dispatcher;
    private int address;

    public void OnReceived(EBEvent e)
    {
        switch (e.type)
        {
            case EBEventType.TestDoorTriggerEntered:
                door.Open();
                break;
            case EBEventType.TestDoorTriggerExited:
                door.Close();
                break;
        }
    }

    // Use this for initialization
    void Start () {
        door = GetComponent<Door>();
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.TestDoorTriggerEntered, address, gameObject);
        dispatcher.Subscribe(EBEventType.TestDoorTriggerExited, address, gameObject);
    }
}
