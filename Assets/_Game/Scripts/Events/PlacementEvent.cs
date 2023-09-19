
namespace PanteonDemo.Event
{
    /// <summary>
    /// Event is triggered when a placeable object is spawned and placement is completed 
    /// </summary>
    public struct PlacementEvent
    {
        public bool IsPlaced { get; private set; }

        public PlacementEvent(bool isPlaced)
        {
            IsPlaced = isPlaced;
        }
    }
}