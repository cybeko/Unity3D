using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyCollectedInTimeSound;
    private AudioSource keyCollectedOutOfTimeSound;
    private AudioSource batteryCollectedSound;

    void Start()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        keyCollectedInTimeSound = audioSources[0];
        batteryCollectedSound = audioSources[1];
        keyCollectedOutOfTimeSound = audioSources[2];

        GameEventSystem.Subscribe(OnGameEvent);
    }
    void Update()
    {
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
    }
}
