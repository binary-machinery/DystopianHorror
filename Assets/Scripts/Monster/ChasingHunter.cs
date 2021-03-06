﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventBus;

public class ChasingHunter : MonoBehaviour, IEventSubscriber
{

    [SerializeField]
    private float movePeriod = 2.0f;

    [SerializeField]
    private bool ignoreStanding = true;

    [SerializeField]
    private float standingDistance = 0.2f;

    private Dispatcher dispatcher;
    private int address;
    private Queue<Waypoint> waypointsQueue = new Queue<Waypoint>();
    private float lastMoveTime = 0.0f;
    private bool active = true;

    public void setActive(bool active)
    {
        this.active = active;
    }

    public bool isActive()
    {
        return active;
    }

    public void OnReceived(EBEvent e)
    {
        if (e.type == EBEventType.NewWaypointCreated)
        {
            waypointsQueue.Enqueue((e as NewWaypointEvent).waypoint);
        }
    }

    void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        address = dispatcher.GetFreeAddress();
        dispatcher.Subscribe(EBEventType.NewWaypointCreated, address, gameObject);
        lastMoveTime = Time.time;
    }

    void OnDestroy()
    {
        dispatcher.Unsubscribe(EBEventType.NewWaypointCreated, address);
    }

    void Update()
    {
        if (!isActive())
        {
            return;
        }
        if (Time.time - lastMoveTime > movePeriod)
        {
            Waypoint wp = GetNextWaypoint();
            if (wp != null)
            {
                MoveToNextWaypoint(wp);
            }
            lastMoveTime += movePeriod;
        }
    }

    private void MoveToNextWaypoint(Waypoint waypoint)
    {
        transform.position = waypoint.position;
    }

    private Waypoint GetNextWaypoint()
    {
        Waypoint res = null;
        while (res == null)
        {
            if (waypointsQueue.Count == 0)
            {
                break;
            }
            Waypoint wp = waypointsQueue.Dequeue();
            if (!ignoreStanding || Vector3.Distance(transform.position, wp.position) > standingDistance)
            {
                res = wp;
            }
        }
        return res;
    }
}
