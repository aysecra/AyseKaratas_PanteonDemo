namespace PanteonDemo.Enum
{
    public enum InputType
    {
        MouseAndKeyboard,
        MobileFinger,
        MobileJoystick
    }

    public enum TouchState
    {
        LeftClick,
        RightClick,
        MiddleClick,
        Touch,
        End
    }
    
    public enum LevelState
    {
        Opened,
        Started,
        Paused,
        Failed,
        Completed
    }
}