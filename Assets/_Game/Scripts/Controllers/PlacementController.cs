using System.Collections;
using System.Collections.Generic;
using PanteonDemo.Interfaces;
using UnityEngine;

namespace PanteonDemo.Controller
{
    public static class PlacementController
    {
        public static void Place(IPlaceable client)
        {
            client.Place();
        }
    }
}
