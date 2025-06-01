using UnityEngine;

public class CameraFixedScript : MonoBehaviour
{
    [SerializeField] private Transform fixedPosition1;
    [SerializeField] private Transform fixedPosition2;

    void Start()
    {
        CameraScript.fixedCameraPosition = fixedPosition1;
        CameraScript.fixedCameraPosition2 = fixedPosition2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CameraScript.currentFixedIndex = 0;
            CameraScript.isFixed = true;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            CameraScript.currentFixedIndex = 1;
            CameraScript.isFixed = true;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CameraScript.isFixed = false;
        }
    }
}
