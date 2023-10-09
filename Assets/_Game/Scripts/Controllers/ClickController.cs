using StrategyDemo.Interfaces;

namespace StrategyDemo.Controller
{
    public static class ClickController
    {
        public static void OnClick(IClickable client)
        {
            client.OnClick();
        }
    }
}
