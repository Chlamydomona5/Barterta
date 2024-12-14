using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.Player
{
    public class MouseHandler : MonoBehaviour
    {
        [SerializeField] private Camera playerCam;

        public Vector2 GetMousePositionScreen()
        {
            return Mouse.current.position.ReadValue();
        }
        
        public Ray GetMousePositionWorldRay()
        {
            Vector3 mousePos = GetMousePositionScreen();
            mousePos.z = playerCam.nearClipPlane;
            
            Ray ray = playerCam.ScreenPointToRay(mousePos);
            //Draw ray
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
            return ray;
        }

        public GroundBlock GetBlockOnMouse()
        {
            //use GetMousePositionWorldRay() to raycast groundblock and log
            RaycastHit hit;
            Physics.Raycast(GetMousePositionWorldRay(), out hit, 100f, LayerMask.GetMask("Ground", "Boat"));
            if (hit.collider && hit.collider.GetComponent<GroundBlock>())
            {
                return hit.collider.GetComponent<GroundBlock>();
            }
            return null;
        }

        public void TurnToMouseDirection()
        {
            var ray = GetMousePositionWorldRay();
            //Find the cross point of ray and plane at y = 0.5
            var crossPoint = ray.GetPoint((0.5f - ray.origin.y) / ray.direction.y);
            var dir = Methods.YtoZero(crossPoint - transform.position);
            transform.rotation = 
                Quaternion.LookRotation(dir);
        }
    }
}