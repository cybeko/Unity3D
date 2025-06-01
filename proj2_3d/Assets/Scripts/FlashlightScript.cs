using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;

    [SerializeField] private float yawSpeed = 0.1f;

    private float yawOffset = 0f;

    public static float charge;
    private float chargeLifetime = 30f;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("None");
        }
        _light = GetComponent<Light>();
        _light.shadowBias = 0.05f;         // Try between 0.05 and 0.2
        _light.shadowNormalBias = 0.4f;    // Try between 0.3 and 0.5

        if (_light == null)
        {
            Debug.LogError("No Light component found");
        }
        charge = 1.0f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            yawOffset -= yawSpeed;
        if (Input.GetKey(KeyCode.E))
            yawOffset += yawSpeed;

        Quaternion baseRot = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);

        Quaternion totalRot = baseRot * Quaternion.Euler(0f, yawOffset, 0f);
        if (GameState.isFpv)
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.2f;
        }
        else
        {
            transform.position = player.transform.position;
        }

        transform.rotation = totalRot;


        if (GameState.isFpv && !GameState.isDay)
        {
            _light.intensity = Mathf.Clamp01(charge);
            charge -= charge < 0 ? 0 : Time.deltaTime / chargeLifetime;
        }
        else
        {
            _light.intensity = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var batteryPickup = other.GetComponent<Battery>();
        if (batteryPickup != null)
        {
            charge += batteryPickup.chargeAmount;
            GameEventSystem.EmitEvent(new GameEvent
            {
                type = "Battery",
                toast = $"{batteryPickup.batteryName} battery collected, charge added: {batteryPickup.chargeAmount}\n Total charge: {charge:F1}",
                sound = EffectsSounds.batteryCollected
            });
            Destroy(other.gameObject);
        }
    }

}
