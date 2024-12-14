using Barterta.InputTrigger;
using Barterta.ItemGrid;

namespace Barterta.Core.KeyInterface
{
    public interface IMoveToHand
    {
        /// <summary>
        ///     Only support groundables can't stack
        /// </summary>
        /// <param name="isToHand"></param>
        /// <param name="trigger"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public void OnMove(bool isToHand, GrabTrigger trigger, GroundBlock block);
    }
}