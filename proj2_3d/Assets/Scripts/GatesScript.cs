using UnityEngine;

public class GatesScript : MonoBehaviour
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
    private AudioSource openingSound1;
    private AudioSource openingSound2;
    private bool hasEmittedOpenEvent = false;

    private bool isOpened = false;
    void Start()
    {
        isKeyInserted = false;
        hitCount = 0;

        AudioSource[] openingSounds = GetComponents<AudioSource>();

        if (openingSounds.Length > 0)
            openingSound1 = openingSounds[0];

        if (openingSounds.Length > 1)
            openingSound2 = openingSounds[1];

        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(OnGameStateChanged);
        OnGameStateChanged(null);
    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.effectsVolume))
        {
            if (openingSound1 != null) openingSound1.volume = GameState.effectsVolume;
            if (openingSound2 != null) openingSound2.volume = GameState.effectsVolume;
        }
    }

    void Update()
    {
        if (!isOpened && isKeyInserted && transform.localPosition.magnitude < size)
        {
            transform.Translate(size * Time.deltaTime / openTime * openDirection);

            if (!hasEmittedOpenEvent && transform.localPosition.magnitude >= size * 0.95f)
            {
                hasEmittedOpenEvent = true;
                GameEventSystem.EmitEvent(new GameEvent
                {
                    type = $"Gate{keyNumber}Opened"
                });
                Debug.Log($"[GATE] Emitting Gate{keyNumber}Opened event");

                isOpened = true;
                if (openingSound1 != null && openingSound2 != null)
                {
                    openingSound1?.Stop();
                    openingSound2?.Stop();
                }

            }
        }
        if((openingSound1 != null && openingSound2 != null && (openingSound1.isPlaying || openingSound2.isPlaying)))
        {
            openingSound1.volume= openingSound2.volume = Time.timeScale == 0.0f? 0.0f : GameState.effectsVolume;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Player hit gate");

            if (isKeyCollected)
            {
                if (!isKeyInserted)
                {
                    isKeyInserted = true;
                    openTime = isKeyInTime ? openTime1 : openTime2;
                    if (isKeyInTime)
                    {
                        if (openingSound1 != null)
                            openingSound1.Play();
                    }
                    else
                    {
                        if (openingSound2 != null)
                            openingSound2.Play();
                        else if (openingSound1 != null)
                            openingSound1.Play();
                    }


                    Debug.Log("The gate should open now");
                }
                
            }
            else
            {
                hitCount++;

                string toastMessage = hitCount == 1
                    ? $"You need a key{keyNumber} to open the gate."
                    : $"Hitting the gate multiple ({hitCount}) times won't open it.";

                GameEventSystem.EmitEvent(new GameEvent
                {
                    type = "GateCollision",
                    toast = toastMessage
                });
            }
        }
    }   
    private void OnGameEvent(GameEvent gameEvent)
    {
        if(gameEvent.type == $"Key{keyNumber}Collected")
        {
            isKeyCollected = true;
            isKeyInTime = (bool)gameEvent.payload;
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(OnGameStateChanged);
    }
}
