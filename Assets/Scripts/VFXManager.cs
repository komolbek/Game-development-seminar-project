using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    public static VFXManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void PlayVFX(GameObject effectObject, Vector3 effectPosition)
    {
        GameObject vfxObject = Instantiate(effectObject, effectPosition, Quaternion.identity);

        ParticleSystem[] particleSystems = vfxObject.GetComponentsInChildren<ParticleSystem>();

        float maxLength = 0f;

        foreach (ParticleSystem particleSystem in particleSystems)
        {
            float currentKnowMaxLength = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;

            if (currentKnowMaxLength > maxLength)
                maxLength = currentKnowMaxLength;
        }

        Destroy(vfxObject, maxLength);
    }
 }
