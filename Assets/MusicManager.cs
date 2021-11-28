using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if(!instance){
            instance = this;
            GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(gameObject);
        }
    }
}
