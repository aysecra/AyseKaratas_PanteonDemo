namespace PanteonDemo.Serializable
{
    [System.Serializable]
    public class LevelProgress
    {
        [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")]
        public string LevelName;
    }
}