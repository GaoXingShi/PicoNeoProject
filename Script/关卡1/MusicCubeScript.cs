using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCubeScript : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    void OnTriggerEnter(Collider _collider)
    {
        if (_collider.tag.Equals("Player"))
        {
            if (source.clip != clip)
            {
                source.clip = clip;
                source.Play(0);
            }
           
        }
    }
}
