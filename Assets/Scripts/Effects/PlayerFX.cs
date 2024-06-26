using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Screen shake FX")]
    [SerializeField] private float shakeMultiplier;
    private CinemachineImpulseSource screenShake;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;

    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorlooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTime;

    [Space]
    [SerializeField] private ParticleSystem dustFx;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTime -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _sakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_sakePower.x * player.facingDir, _sakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTime < 0)
        {
            afterImageCooldownTime = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);

            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorlooseRate, sr.sprite);
        }
    }

    public void PlayDustFX()
    {
        if (dustFx != null)
            dustFx.Play();
    }
}
