using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyCollectedInTimeSound;
    private AudioSource keyCollectedOutOfTimeSound;
    private AudioSource batteryCollectedSound;

    private static EffectsScript prevInstance;


    void Start()
    {
        if (prevInstance == null)
        {
            prevInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        keyCollectedInTimeSound = audioSources[0];
        batteryCollectedSound = audioSources[1];
        keyCollectedOutOfTimeSound = audioSources[2];

        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(OnGameStateChanged);
    }
    void Update()
    {
    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.effectsSingleVolume))
        {
            keyCollectedInTimeSound.volume =
            batteryCollectedSound.volume =
            keyCollectedOutOfTimeSound.volume = GameState.effectsSingleVolume;
            Debug.Log($"[EffectsScript] OnGameStateChanged({fieldName}): setting volumes to {GameState.effectsSingleVolume}");
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if(gameEvent.sound != null)
        {
            switch (gameEvent.sound)
            {
                case EffectsSounds.batteryCollected: 
                    batteryCollectedSound.Play(); 
                    break;
                case EffectsSounds.keyCollectedOutOfTime:
                    keyCollectedOutOfTimeSound.Play();
                    break;
                default:
                    keyCollectedInTimeSound.Play(); 
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(OnGameStateChanged);
    }
}
