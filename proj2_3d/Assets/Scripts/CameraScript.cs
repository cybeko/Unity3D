using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset;

    private Transform cameraAnchor;

    private InputAction lookAction;
    private float angleY;
    private float angleX;
    private float angleY0;
    private float angleX0;
    private float sensitivityY = 30.0f;
    private float sensitivityX = 15.0f;
    private float minOffset = 1.5f;
    private float maxOffset = 12f;

    private float minAngle = 10f;
    private float maxAngle = 80f;
    private float minAngleYFpv = -10f;
    private float maxAngleYFpv = 40f;

    public static bool isFixed = false;
    public static Transform fixedCameraPosition = null!;
    public static Transform fixedCameraPosition2 = null!;
    public static int currentFixedIndex = 0;

    void Start()
    {
        cameraAnchor = GameObject.Find("Player").transform;
        offset = this.transform.position - cameraAnchor.position;
        lookAction = InputSystem.actions.FindAction("Look");
        angleY = angleY0 = this.transform.eulerAngles.y;
        angleX = angleX0 = this.transform.eulerAngles.x;
        GameState.isFpv = offset.magnitude < minOffset;
    }
    void Update()
    {
        if (isFixed)
        {
            Transform currentFixedTransform = currentFixedIndex == 0 ? fixedCameraPosition : fixedCameraPosition2;
            this.transform.position = currentFixedTransform.position;
            this.transform.rotation = currentFixedTransform.rotation;
        }

        else
        {
            Vector2 zoom = Input.mouseScrollDelta * Time.timeScale;

            if (zoom.y > 0 && !GameState.isFpv)
            {
                offset *= 0.9f;
                if (offset.magnitude < minOffset)
                {
                    offset *= 0.01f;
                    GameState.isFpv = true;
                }
            }
            else if (zoom.y < 0)
            {
                if (GameState.isFpv)
                {
                    offset *= minOffset / offset.magnitude;
                    GameState.isFpv = false;
                }
                if (offset.magnitude < maxOffset)
                {
                    offset *= 1.1f;
                }
            }

            Vector2 lookValue = Time.deltaTime * lookAction.ReadValue<Vector2>();

            angleY += lookValue.x * sensitivityY;
            angleX -= lookValue.y * sensitivityX;

            if (GameState.isFpv)
            {
                angleX = Mathf.Clamp(angleX, minAngleYFpv, maxAngleYFpv);
            }
            else
            {
                angleX = Mathf.Clamp(angleX, minAngle, maxAngle);
            }

            this.transform.eulerAngles = new Vector3(angleX, angleY, 0f);

            if (GameState.isFpv)
            {
                Vector3 headOffset = new Vector3(0f, 0.2f, 0f);
                this.transform.position = cameraAnchor.position + headOffset;
            }
            else
            {
                this.transform.position = cameraAnchor.position + Quaternion.Euler(angleX - angleX0, angleY - angleY0, 0f) * offset;
            }

        }
    }
}
