using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    [SerializeField]
    private int keyNumber;
    [SerializeField]
    private string description = "";

    private GameObject content;
    private Image indicatorImage;
    private float timeout = 10.0f;
    private float leftTime;

    private bool isInTime = true;
    private bool isActive = false;

    void Start()
    {
        if(keyNumber == 1)
        {
            isActive = true;
        }
        content = transform.Find("Content").gameObject;
        indicatorImage = transform
            .Find("Indicator/Canvas/Foreground")
            .GetComponent<Image>();
        indicatorImage.fillAmount = 1;
        leftTime = timeout;
    }
    public void ActivateKey()
    {
        isActive = true;
        leftTime = timeout;
        isInTime = true;
    }

    void Update()
    {
        content.transform.Rotate(0, Time.deltaTime * 30f, 0);
        if (isActive) {
            if (leftTime >= 0)
            {
                indicatorImage.fillAmount = leftTime / timeout;
                indicatorImage.color = new Color(
                    Mathf.Clamp01(2.0f * (1.0f - indicatorImage.fillAmount)),
                    Mathf.Clamp01(2.0f * indicatorImage.fillAmount),
                    0.0f
                    );
                leftTime -= Time.deltaTime;
                if (leftTime <= 0)
                {
                    isInTime = false;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameState.bag.Add($"Key{keyNumber}", 1);

            GameEventSystem.EmitEvent(new GameEvent
            {
                type = $"Key{keyNumber}Collected",
                payload = isInTime,
                toast = $"Key {keyNumber} is found. You can now open the {description} gates.",
                sound = isInTime ? EffectsSounds.keyCollectedInTime : EffectsSounds.keyCollectedOutOfTime
            });
            Debug.Log($"[KEY] Emitting event: Key{keyNumber}Collected, isInTime: {isInTime}");
            Destroy(this.gameObject);
        }
    }
}
