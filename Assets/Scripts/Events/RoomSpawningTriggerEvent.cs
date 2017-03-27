﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class RoomSpawningTriggerEvent : EBEvent
{

    public readonly string roomsManagerId;
    public readonly int roomEntryId;
    public readonly TriggerAction action;

    public RoomSpawningTriggerEvent(string roomsManagerId, int roomEntryId, TriggerAction action)
    {
        this.type = EBEventType.RoomSpawningTrigger;
        this.roomsManagerId = roomsManagerId;
        this.roomEntryId = roomEntryId;
        this.action = action;
    }

}