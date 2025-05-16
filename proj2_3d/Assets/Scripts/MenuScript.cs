using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private GameObject content;
    private bool isMuted = false;
    private Slider effectsSlider;
    private Slider effectsSingleSlider;

    private Slider musicSlider;
    private Toggle muteToggle;

    private float startTimeScale;
    private float defaultMusicVolume;
    private float defaultEffectsVolume;
    private float defaultEffectsSingleVolume;
    private bool defaultIsMuted;


    void Start()
    {
        content = transform.Find("Content").gameObject;
        effectsSlider = transform.Find("Content/Sounds/EffectsSlider").GetComponent<Slider>();
        effectsSingleSlider = transform.Find("Content/Sounds/EffectsSingleSlider").GetComponent<Slider>();
        musicSlider = transform.Find("Content/Sounds/MusicSlider").GetComponent<Slider>(); 
        muteToggle = transform.Find("Content/Sounds/MuteToggle").GetComponent<Toggle>();
        GetDefault();

        LoadSaves();
        startTimeScale = Time.timeScale;
        OnValueChanged(isMuted);
        Hide();
    }

    private void GetDefault()
    {
        defaultMusicVolume = GameState.musicVolume;
        defaultEffectsVolume = GameState.effectsVolume;
        defaultEffectsSingleVolume = GameState.effectsSingleVolume;
        defaultIsMuted = muteToggle.isOn;

    }
    private void LoadSaves()
    {
        if (PlayerPrefs.HasKey("effectsVolume"))
        {
            GameState.effectsVolume = effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume");
        }
        else
        {
            effectsSlider.value = defaultEffectsVolume;
        }
        if (PlayerPrefs.HasKey("effectsSingleVolume"))
        {
            GameState.effectsSingleVolume = effectsSingleSlider.value = PlayerPrefs.GetFloat("effectsSingleVolume");
        }
        else
        {
            effectsSingleSlider.value = defaultEffectsSingleVolume;
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            GameState.musicVolume = musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicSlider.value = defaultMusicVolume;
        }

        if (PlayerPrefs.HasKey("isMuted"))
        {
            isMuted = muteToggle.isOn = PlayerPrefs.GetInt("isMuted") == 1;
        }
        else
        {
            isMuted = defaultIsMuted;

        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (content.activeInHierarchy)
            {
                Hide();
            }
            else { 
                Show();
            }
        }
    }

    private void Show()
    {
        content.SetActive(true);
        Time.timeScale = startTimeScale;
    }
    private void Hide()
    {
        startTimeScale = Time.timeScale;
        content.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OnMusicVolumeChanged(float volume)
    {
        if (!isMuted){ GameState.musicVolume = volume; }
    }
    public void OnEffectsVolumeChanged(float volume)
    {
        if(!isMuted){ GameState.effectsVolume = volume; }

    }
    public void OnEffectsSingleVolumeChanged(float volume)
    {
        if (!isMuted) { GameState.effectsSingleVolume = volume; }
    }
    public void OnValueChanged(bool isMute)
    {
        isMuted = isMute;
        if (isMute)
        {
            GameState.musicVolume = 0f;
            GameState.effectsVolume = 0f;
            GameState.effectsSingleVolume = 0f;
        }
        else
        {
            GameState.musicVolume = musicSlider.value;
            GameState.effectsVolume = effectsSlider.value;
            GameState.effectsSingleVolume = effectsSingleSlider.value;
        }
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
    public void OnDefaultsClick()
    {
        isMuted = muteToggle.isOn = defaultIsMuted;
        GameState.effectsVolume = effectsSlider.value = defaultEffectsVolume;
        GameState.effectsSingleVolume = effectsSingleSlider.value = defaultEffectsSingleVolume;
        GameState.musicVolume = musicSlider.value = defaultMusicVolume;
    }
    public void OnContinueClick()
    {
        Hide();
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("effectsVolume",effectsSlider.value);
        PlayerPrefs.SetFloat("effectsSingleVolume", effectsSingleSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);

        PlayerPrefs.Save();
    }
}
