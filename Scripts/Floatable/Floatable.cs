using System.Collections;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Floatable
{
    public class Floatable : MonoBehaviour
    {
        [SerializeField] [ReadOnly] public Groundable referGroundable;
        private FloatableController _floatableController;
        private Tween _sinkTween;
        private Sequence _popTween;

        private void OnCollisionEnter(Collision collision)
        {
            if (_sinkTween == null && collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
                _sinkTween = Sink();
        }

        public void Init(FloatableController manager, Groundable groundable)
        {
            //Set to UI layer to prevent mask by water
            gameObject.layer = LayerMask.NameToLayer("UI");
            _floatableController = manager;
            referGroundable = groundable;
            var model = Instantiate(groundable.GetComponentInChildren<MeshRenderer>(), transform);
            model.transform.localPosition = Vector3.zero;
            Collider cd;
            // ReSharper disable once AssignmentInConditionalExpression
            if (cd = model.GetComponent<Collider>()) cd.enabled = false;
            gameObject.AddComponent<SphereCollider>();

            StartCoroutine(OutCoroutine());
        }

        public bool TransferToGroundable(GroundBlock block)
        {
            if (!block)
            {
                //Solution but WHY???
                DestroyImmediate(gameObject);
                return false;
            }
            
            var groundable = Instantiate(referGroundable);
            groundable.SetOn(block);
            var mesh = groundable.GetComponentInChildren<MeshRenderer>();
            mesh.enabled = false;
            
            //Can't find "Floatable"
            _popTween = transform.BounceToGroundBlock(block).OnComplete(delegate
            {
                Destroy(gameObject);
                mesh.enabled = true;
            });
            return true;
        }

        private IEnumerator OutCoroutine()
        {
            yield return new WaitForSeconds(45f);
            while (true)
            {
                if (!_floatableController.IsAroundPlayer(transform.position)) Destroy(gameObject);
                yield return new WaitForSeconds(10f);
            }
        }

        private Tween Sink()
        {
            var sinkTime = 15f;
            return transform.DOMoveY(transform.position.y - 1f, sinkTime).OnComplete(delegate
            {
                DestroyImmediate(gameObject);
            });
        }
    }
}