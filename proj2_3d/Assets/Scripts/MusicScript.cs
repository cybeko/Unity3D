using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private static MusicScript prevInstance;
    private AudioSource music;

    void Start()
    {
        if (prevInstance == null) {
            prevInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
        music = GetComponent<AudioSource>();
        GameState.AddListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(string fieldName)
    {
        music.volume = GameState.musicVolume;
    }
    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
    void Update()
    {
        
    }
}
