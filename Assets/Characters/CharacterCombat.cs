using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class CharacterCombat : MonoBehaviour
{
    public CharacterStateType CharacterState;
    public Animator CharacterAnimator;
    private static string TakeDamageAnimation = "TakeDamage";

    public Guid CharacterID;

    //TODO: Extract combat, health and character to 3 separate classes
    public float maxHealth;
    public float MinDamage;
    public float MaxDamage;

    public float BulletDelay;
    private float lastBulletTime;

    public float InvulnerabilityTime;
    private float lastDamageTakenTime;
    
    [SerializeField]
    private float CurrentHealth;
    
    public AudioSource BulletAudioSource;

    private void Start()
    {
        CharacterID = Guid.NewGuid();
        CurrentHealth = maxHealth;
        CharacterState = CharacterStateType.Alive;
    }

    public void TakeDamage(float damage)
    {
        if (CharacterState == CharacterStateType.Alive)
        {
            if (CharacterState != CharacterStateType.Dead && lastDamageTakenTime + InvulnerabilityTime < Time.time)
            {
                lastDamageTakenTime = Time.time;
                CurrentHealth -= damage;
                CharacterAnimator.Play(TakeDamageAnimation);
                //TODO: TakeDamage FX / Audio
                //TODO: Text damage popups

                if (CurrentHealth < 0f)
                {
                    CharacterState = CharacterStateType.Dead;
                    Die();
                }
            }
        }
    }

    public abstract void Die();

    protected void FireBullet(GameObject bulletPrefab, Quaternion direction)
    {
        if (CharacterState == CharacterStateType.Alive && lastBulletTime + BulletDelay < Time.time)
        {
            lastBulletTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = this.transform.position;
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (bulletController != null)
            {
                bullet.transform.rotation = direction;
                bulletController.FireBullet(CharacterID, direction, CalculateDamage());
                BulletAudioSource.PlayOneShot(BulletAudioSource.clip);
            }
        }
    }

    protected float CalculateDamage()
    {
        return Random.Range(MinDamage, MaxDamage);
    }

    private void OnEnable()
    {
        Initialise();
    }

    protected abstract void Initialise();
}

public enum CharacterStateType
{
    Asleep = 1,
    Alive = 2,
    Dead = 3,
}