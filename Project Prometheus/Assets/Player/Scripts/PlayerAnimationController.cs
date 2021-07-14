using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Sprite idle, thrown;
    [SerializeField] Animator playerAnim, demonAnim, summoningAnim, mCircleAnim;

    private const string PLAYER_READY = "Ready";
    private const string TRIGGER_THROW = "triggerThrow";
    private const string CIRCLE_ON = "Magic circle_creating";
    private const string CIRCLE_OFF = "Magic circle_fading";
    private const string SUMMON_FX = "Summon fx_summoning";
    private const string SUMMON_RECALL = "Summon fx_recalling";
    private const string PLAYER_RECALL = "Recall";

    private SpriteRenderer playerRenderer;

    private void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayReady()
    {
        playerAnim.Play(PLAYER_READY); 
    }

    public void PlayThrow()
    {
        playerAnim.SetTrigger(TRIGGER_THROW);
        playerRenderer.sprite = thrown;
    }

    public void PlayRecall()
    {
        //playerAnim.Play(PLAYER_RECALL);
        mCircleAnim.Play(CIRCLE_ON);
        playerRenderer.sprite = idle;
    }

    public void OnCircleOnAnimComplete()
    {
        summoningAnim.Play(SUMMON_RECALL);
    }

    public void PlaySummon()
    {
        summoningAnim.Play(SUMMON_FX);
        mCircleAnim.Play(CIRCLE_OFF);
    }
}
