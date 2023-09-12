
namespace PanteonDemo.Interfaces
{
    public interface ISwitchable
    {
        public bool IsActivate { get; }
        public void Activate();
        public void Deactivate();
    }
}
