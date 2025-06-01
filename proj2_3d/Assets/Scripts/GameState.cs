using NUnit.Framework;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static Dictionary<string, int> bag {  get; } = new Dictionary<string, int>();    

    private static float _effectsSingleVolume = 0.5f;
    public static float effectsSingleVolume
    {
        get => _effectsSingleVolume;
        set
        {
            if (_effectsSingleVolume != value)
            {
                _effectsSingleVolume = value;
                Notify(nameof(effectsSingleVolume));
            }
        }
    }

    private static float _effectsVolume = 0.5f;
    public static float effectsVolume
    {
        get => _effectsVolume;
        set
        {
            if (_effectsVolume != value)
            {
                _effectsVolume = value;
                Notify(nameof(effectsVolume));
            }
        }
    }
    private static float _musicVolume = 0.09f;
    public static float musicVolume
    {
        get => _musicVolume;
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                Notify(nameof(musicVolume));
            }
        }
    }

    private static bool _isDay;
    public static bool isDay
    {
        get => _isDay;
        set
        {
            if (_isDay!= value)
            {
                _isDay = value;
                Notify(nameof(isDay));
            }
        }
    }

    public static bool _isFpv;
    public static bool isFpv
    {
        get => _isFpv;
        set
        {
            if (_isFpv != value)
            {
                _isFpv = value;
                Notify(nameof(isFpv));
            }
        }
    }

    #region Change Notifier
    private static List<Action<string>> listeners = new List<Action<string>>();

    public static void AddListener(Action<string> listener)
    {
        listeners.Add(listener);
        listener(null);
    }
    public static void RemoveListener(Action<string> listener)
    {
        listeners.Remove(listener);
    }
    public static void Notify(string fieldName) {
        foreach (Action<string> listener in listeners)
        {
            listener(fieldName);
        }
    }
    #endregion

    public static void SetProperty(string name, object value)
    {
        var prop = typeof(GameState).GetProperty(
            name,
            System.Reflection.BindingFlags.Static | 
            System.Reflection.BindingFlags.Public);
        if (prop == null)
        {
            Debug.LogError($"Error setting property: name not found '{name}','{value}'");
        }
        else
        {
            prop.SetValue(null, value);
        }
    }
}
