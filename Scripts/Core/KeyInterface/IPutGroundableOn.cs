using Barterta.InputTrigger;
using Barterta.ItemGrid;

namespace Barterta.Core.KeyInterface
{
    public interface IPutGroundableOn
    {
        /// <summary>
        ///     This method will triggered before the groundable set on and do not execute any function.
        /// </summary>
        public bool JudgePut(Groundable groundable);
        
        public void OnJudgePut(bool judge, Groundable groundable, GrabTrigger trigger = null);

        /// <summary>
        ///     This method will triggered before the groundable set on.
        /// </summary>
        public void EffectBeforeSetOn(Groundable groundable, GrabTrigger trigger = null);

        /// <summary>
        ///     This method will triggered before the groundable set on.
        /// </summary>
        public void EffectAfterSetOn(Groundable groundable, GrabTrigger trigger = null);
    }
}