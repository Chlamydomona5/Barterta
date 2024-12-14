/*using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Tool;
using UnityEngine;

namespace Barterta.Boat.Manual
{
    public class ManualBoat : MonoBehaviour, IShortInteractOnGroundEffector, ILongInteractOnGroundEffector
    {
        public Transform ridePoint;
        public float speedLimit;
        public float acc;
        public float rotateSpeed;

        [SerializeField] private Groundable shell;

        private LandDetector _detector;
        private Rigidbody _rb;
        private Net _net;

        private void Start()
        {
            
            _rb = GetComponent<Rigidbody>();
            _detector = GetComponentInChildren<LandDetector>();
            _net = GetComponentInChildren<Net>();
        }

        public void SetNetActive()
        {
            
        }

        public void Move(Vector2 moveVec, float maxSpeedConstant)
        {
            //Cause boat movement is vertical + rotate view, so only use the y axis
            //foward used as direction 
            Vector3 move = new Vector3(moveVec.x, 0, moveVec.y);
            var forward = transform.forward;
            Vector3 rbForward = forward;
            Vector3 torque = Vector3.Cross(move, rbForward);
            var yTorque = - Vector3.Project(torque, Vector3.up);

            _rb.velocity += acc * Vector3.Project(move, forward);
            if (_rb.velocity.magnitude > speedLimit * maxSpeedConstant) _rb.velocity = _rb.velocity.normalized * (speedLimit * maxSpeedConstant);

            _rb.AddTorque(yTorque * rotateSpeed);
        }

        public GameObject GetSurroundBlock()
        {
            return _detector.surroundBlock;
        }

        public void BackToShell(GroundBlock block)
        {
            if (!GetComponentInChildren<SailTrigger_Past>())
            {
                Instantiate(shell).SetOn(block);
                Destroy(gameObject);   
            }
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (!isLong)
            {
                trigger.GetComponent<SailTrigger_Past>().StartBoating(this);
                return true;
            }
            else
            {
                if (trigger.HandBlock.groundablesOn.Count == 0)
                {
                    BackToShell(trigger.HandBlock);
                    return true;   
                }
            }
            return false;
        }

        public void Harvest(GroundBlock block)
        {
            _net.PopAllFloatablesTo(block);
        }
    }
}*/