using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public static partial class EventsInProject
{
    public class GameEvents
    {
        //< Events from Customer>
        public static Action<OrderPlacingPointData> GetLandingPointDataEvent;
        public static Action<OrderPlacingPointData> RemoveOrderPlacingPointDataEvent;


    }

    public class EconomyEvents
    {
        
    }

    public class UIEvents
    {
        
    }
}
