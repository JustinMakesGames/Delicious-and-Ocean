using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{

    public static AudioManagement Instance;
    [SerializeField] private List<string> stringList = new List<string>();
    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField] private Dictionary<string, AudioSource> audioPairs = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        for (int i = 0; i < stringList.Count; i++)
        {
            audioPairs.Add(stringList[i], audioSources[i]);
        }
    }

    public void PlayAudio(string audioName)
    {
        audioPairs[audioName].Play();
    }


}
