using PerfectMazeProject.Camera.Pointers;
using UnityEngine;

namespace PerfectMazeProject.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Vector Elements")]
        private Vector3 cameraPos;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 10f;

        // Move:
        private int moveIndex;
        private bool isMovePointerDown;

        // Zoom:
        private int zoomIndex;
        private bool isZoomPointerDown;

        // Camera Clamping limits:
        private int XZminValue = -100;
        private int XZmaxValue = 250;
        private int YminValue = -140;
        private int YmaxValue = 100;

        #region Helpers:

        private float MovementSpeed()
        {
            return movementSpeed * Time.deltaTime;
        }

        // Update position of the camera:
        private void UpdateNewPosition()
        {
            transform.position = cameraPos;
        }

        // Camera boundries, this will prevent camera from moving too far away from screen:
        private void ClampCameraPositions()
        {
            cameraPos.x = Mathf.Clamp(cameraPos.x, XZminValue, XZmaxValue);
            cameraPos.z = Mathf.Clamp(cameraPos.z, XZminValue, XZmaxValue);
            cameraPos.y = Mathf.Clamp(cameraPos.y, YminValue, YmaxValue);
        }

        #endregion

        private void Start()
        {
            ClampCameraPositions();
            UpdateNewPosition();
        }

        private void OnEnable()
        {
            MovePointers.OnMoveButtonClicked += SetMovePointersState;
            ZoomPointers.OnZoomButtonClicked += SetZoomPointersState;
        }

        private void OnDisable()
        {
            MovePointers.OnMoveButtonClicked -= SetMovePointersState;
            ZoomPointers.OnZoomButtonClicked -= SetZoomPointersState;
        }


        private void Update()
        {
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
            HandleMovement();
#endif
            // Mobile controls:
            MoveCameraWithPointers();
            ZoomCameraWithPointers();
            ClampCameraPositions();
        }

        #region PC_Controls:

        private void HandleMovement()
        {
            transform.position = cameraPos;

            HandleMovementVertical();
            HandleMovementHorizontal();
        }

        private void HandleMovementHorizontal()
        {
            if (Input.GetKey(KeyCode.D))
                cameraPos += transform.right * MovementSpeed();

            else if (Input.GetKey(KeyCode.A))
                cameraPos += transform.right * -MovementSpeed();
        }

        private void HandleMovementVertical()
        {
            if (Input.GetKey(KeyCode.W))
                cameraPos += transform.forward * MovementSpeed();

            else if (Input.GetKey(KeyCode.S))
                cameraPos += transform.forward * -MovementSpeed();
        }


        #endregion

#region Mobile_Controls:

        #region Pointer_Listeners:

        /// <summary>
        /// Event listener for MoveCameraPointers. This will trigger camera movement:
        /// </summary>
        /// <param name="moveIndex"></param>
        /// <param name="isMovePointerDown"></param>
        private void SetMovePointersState(int moveIndex, bool isMovePointerDown)
        {
            this.moveIndex = moveIndex;
            this.isMovePointerDown = isMovePointerDown;
        }

        /// <summary>
        /// Event listener for MoveCameraPointers. This will trigger camera zoom:
        /// </summary>
        /// <param name="zoomIndex"></param>
        /// <param name="isZoomPointerDown"></param>
        private void SetZoomPointersState(int zoomIndex, bool isZoomPointerDown)
        {
            this.zoomIndex = zoomIndex;
            this.isZoomPointerDown = isZoomPointerDown;
        }

        private void MoveCameraWithPointers() // 0 - Right, 1 - Left, 2 - Up, 3 - Down
        {
            // if mouse pointer released:
            if (!isMovePointerDown)
                return;

            // if mouse click and held:
            if (moveIndex == 0)
                MoveCameraUp();
            else if (moveIndex == 1)
                MoveCameraDown();
            else if (moveIndex == 2)
                MoveCameraRight();
            else if (moveIndex == 3)
                MoveCameraLeft();
        }

        private void ZoomCameraWithPointers() // 0 - Right, 1 - Left, 2 - Up, 3 - Down
        {
            // if mouse pointer released:
            if (!isZoomPointerDown)
                return;

            // if mouse click and held:
            if (zoomIndex == 0)
                ZoomIn();
            else if (zoomIndex == 1)
                ZoomOut();
        }

        #endregion

        #region Camera Movement:

        private void MoveCameraUp()
        {
            cameraPos.z += MovementSpeed();
            UpdateNewPosition();
        }
        private void MoveCameraDown()
        {
            cameraPos.z -= MovementSpeed();
            UpdateNewPosition();
        }
        private void MoveCameraRight()
        {
            cameraPos.x += MovementSpeed();
            UpdateNewPosition();
        }
        private void MoveCameraLeft()
        {
            cameraPos.x -= MovementSpeed();
            UpdateNewPosition();
        }

        #endregion

        #region Camera Zoom:

        private void ZoomIn()
        {
            cameraPos.y -= MovementSpeed();
            UpdateNewPosition();
        }
        private void ZoomOut()
        {
            cameraPos.y += MovementSpeed();
            UpdateNewPosition();
        }

        #endregion


#endregion
    }
}

