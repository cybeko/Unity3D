using UnityEngine;

public class Gates1Script : MonoBehaviour
{
    [SerializeField] private int keyNumber;
    [SerializeField] private Vector3 openDirection = Vector3.back;
    [SerializeField] private float size = 0.19f;

    private float openTime;
    private float openTime1 = 1f;
    private float openTime2 = 10.0f;

    private bool isKeyInserted;
    private bool isKeyCollected = false;
    private bool isKeyInTime = true;

    private int hitCount;

    void Start()
    {
        isKeyInserted = false;
        hitCount = 0;
        GameState.AddListener(OnGameStateChanged);
    }
    void Update()
    {
        if (isKeyInserted && transform.localPosition.magnitude < size)
        {
            Debug.Log("Opening");
            transform.Translate(size * Time.deltaTime / openTime * openDirection);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("Player hit gate");

            if (isKeyCollected)
            {
                isKeyInserted = true;
                openTime = isKeyInTime ? openTime1 : openTime2;
                Debug.Log("The gate should open now");
            }
            else
            {
                hitCount += 1;
                if (hitCount == 1)
                {
                    ToasterScript.Toast($"You need a key{keyNumber} to open the blue gate.");
                }
                else
                {
                    ToasterScript.Toast($"Hitting the gate {hitCount} times won't open it.");
                }
            }
        }
    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == $"Key{keyNumber}Collected")
        {
            isKeyCollected = true;
        }
        else if (fieldName == $"isKey{keyNumber}InTime")
        {
            isKeyInTime = false;
        }
    }
    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
}
