/*using System.Collections;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.UI.WorldUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Interactable
{
    public class Teleporter : MonoBehaviour, IPutGroundableOn, IBeSettled
    {
        [ReadOnly] public Teleporter another;

        [SerializeField] private float teleportTime;
        [SerializeField] [ReadOnly] private float teleportTimer;

        private Groundable _groundable;
        private PlayerStayUI _playerStayUI;
        public Coroutine NowCoroutine;

        public bool SelfActive => _groundable.blockUnder.island;

        //Both active then portal is active.
        public bool BothActive => SelfActive && another && another.SelfActive;

        private void Start()
        {
            _groundable = GetComponent<Groundable>();
            _playerStayUI = GetComponentInChildren<PlayerStayUI>();
        }

        public void OnSettled(GroundBlock block)
        {
            if (!another)
            {
                var color = new Color(Random.value, Random.value, Random.value);

                var newBlock = block.island.GetRandomSurroundStackableBlock(block.coordinate);

                var partner = Instantiate(gameObject);
                partner.GetComponent<Teleporter>().another = this;

                partner.GetComponent<Groundable>().SetOn(newBlock);

                another = partner.GetComponent<Teleporter>();

                GetComponentInChildren<MeshRenderer>().materials[0].color = color;
                another.GetComponentInChildren<MeshRenderer>().materials[0].color = color;
            }
        }

        public bool JudgePut(Groundable groundable)
        {
            //Can't place on teleporter;
            return !groundable.GetComponent<Teleporter>();
        }

        public void EffectBeforeSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
            //If no teleport is going on and is set by hand
            if (BothActive && NowCoroutine == null && groundable.blockUnder.CompareTag("Player"))
            {
                NowCoroutine = StartCoroutine(TeleportTo());
                another.NowCoroutine = another.StartCoroutine(another.TeleportIn());
            }
        }

        public void EffectAfterSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
        }

        private IEnumerator TeleportTo()
        {
            yield return null;
            while (_groundable.blockUnder.groundablesOn.Count > 1)
            {
                while (teleportTimer < teleportTime)
                {
                    teleportTimer += UnityEngine.Time.deltaTime;
                    _playerStayUI.SetProgressBar(teleportTimer / teleportTime);
                    yield return null;
                }

                var groundable = _groundable.blockUnder.groundablesOn[_groundable.blockUnder.groundablesOn.Count - 1];
                groundable.BeRemovedFromNowBlock();
                groundable.SetOn(another._groundable.blockUnder);

                teleportTimer = 0;
            }

            StopTeleport();
        }

        private IEnumerator TeleportIn()
        {
            //Update UI
            while (another.NowCoroutine != null)
            {
                //If can't stack on, process canceled
                if (_groundable.blockUnder.groundablesOn.Count > 1 && !_groundable.blockUnder.groundablesOn[1]
                        .CanStackOn(
                            another._groundable.blockUnder.groundablesOn[
                                another._groundable.blockUnder.groundablesOn.Count - 1]))
                    another.StopTeleport();
                _playerStayUI.SetProgressBar(another.teleportTimer / teleportTime);
                yield return null;
            }
        }

        private void StopTeleport()
        {
            if (NowCoroutine != null)
            {
                //Stop Coroutines
                StopCoroutine(NowCoroutine);
                NowCoroutine = null;

                another.StopCoroutine(another.NowCoroutine);
                another.NowCoroutine = null;

                //Reset Timer
                teleportTimer = 0;
                //Reset ui
                _playerStayUI.SetProgressBar(0);
                another._playerStayUI.SetProgressBar(0);
            }
        }
    }
}*/