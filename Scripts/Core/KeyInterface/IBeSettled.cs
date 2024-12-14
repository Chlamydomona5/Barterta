using Barterta.ItemGrid;

namespace Barterta.Core.KeyInterface
{
    public interface IBeSettled
    {
        public void OnSettled(GroundBlock block);
    }
}