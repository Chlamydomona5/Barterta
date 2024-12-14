using Barterta.InputTrigger;

namespace Barterta.Core.KeyInterface
{
    public interface IInteractBase
    {
        public bool Judge(bool isLong, GrabTrigger trigger);
        public void OnInteract(bool isLong, GrabTrigger trigger);
    }
}