using System.Collections.Generic;
using StrategyDemo.Interfaces;
using StrategyDemo.Logic;

namespace StrategyDemo.Controller
{
    public static class PlacementController
    {
        public static void Place(IPlaceable client, List<CellInfo> cell)
        {
            client.Place(cell);
        }
    }
}
