using Barterta.InputTrigger;
using Barterta.ItemGrid;

namespace Barterta.Core.KeyInterface
{
    public interface IConsumeGroundable
    {
        /// <summary>
        /// This method will triggered before the groundable set on, and can not execute any function.
        /// </summary>
        public bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null);
        /// <summary>
        /// This method will execute any function on judge.
        /// </summary>
        public void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null);
        /// <summary>
        ///     This method will triggered before the groundable set on.
        /// </summary>
        public void ConsumeEffect(Groundable groundable, GrabTrigger trigger);
    }
}