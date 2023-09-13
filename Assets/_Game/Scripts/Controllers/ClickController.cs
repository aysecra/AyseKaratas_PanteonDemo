using PanteonDemo.Interfaces;

namespace PanteonDemo.Controller
{
    public static class ClickController
    {
        public static void OnClick(IClickable client)
        {
            client.OnClick();
        }
    }
}
