using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyAfterPlayVFX : MonoBehaviour
{
    private VisualEffect _vfx;
    private bool _isPlaying;

    void Start()
    {
        _vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_vfx.aliveParticleCount > 0)
        {
            _isPlaying = true;
        }

        if (_vfx.aliveParticleCount < 1 && _isPlaying)
        {
            Destroy(gameObject);
        }
    }
}