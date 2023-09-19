using System.Collections.Generic;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;

namespace PanteonDemo.Controller
{
    public static class PlacementController
    {
        public static void Place(IPlaceable client, List<CellInfo> cell)
        {
            client.Place(cell);
        }
    }
}
