using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;
   
    private static PlayerScript prevInstance = null;
    void Start()
    {
        if(prevInstance != null )
        {
            GameObject.Destroy( this.gameObject );
        }
        else
        {
            prevInstance = this;
        }
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        
        camForward.y = 0f;
        if(camForward == Vector3.zero)
        {
            camForward = Camera.main.transform.up;
        }
        else
        {
            camForward.Normalize();
        }
        Vector3 force = camForward * moveValue.y + camRight * moveValue.x;
        rb.AddForce(force * Time.timeScale);
    }
}
