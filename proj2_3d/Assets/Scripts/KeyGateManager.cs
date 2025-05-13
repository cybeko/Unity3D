using UnityEngine;

public class KeyGateManager : MonoBehaviour
{
    [SerializeField]
    private KeyScript[] keys;

    private int currentIndex = 0;

    void Start()
    {
        GameEventSystem.Subscribe(OnGameEvent);

        if (keys.Length > 0)
        {
            keys[0].ActivateKey();
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.type == $"Gate{currentIndex + 1}Opened")
        {
            currentIndex++;
            if (currentIndex < keys.Length)
            {
                keys[currentIndex].ActivateKey();
            }
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
    }
}
