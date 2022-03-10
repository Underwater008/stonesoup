using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractableSpikes : Tile
{
    [Header("Spike")]
    //The Animator component on the object
    public Animator animator;
    //The Collider component on the object
    public Collider2D col;
    //How long should the spikes remain concealed in between attacks
    public float downTime;
    //How long should the spikes be attacking every time
    public float upTime;
    //How much damage should the spikes do
    public int damage = 1;

    //The length of the Pre-attack and Post-attack animation
    const float preAttackTime = 0.25f;
    const float postAttackTime = 0.15f;

    //If the spikes can attack
    bool _canAttack = true;


    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(SpikeAnimation());
    }

    protected IEnumerator SpikeAnimation ()
    {
        //The spikes start concealed
        animator.SetInteger("State", 0);
        yield return new WaitForSeconds(downTime);

        //Play Pre-attack animation
        animator.SetInteger("State", 1);
        yield return new WaitForSeconds(preAttackTime);

        if (_canAttack)
        {
            //Enable hitbox when the spikes attack
            col.enabled = true;
            animator.SetInteger("State", 2);
            yield return new WaitForSeconds(upTime);

            //Disable hitbox when the spikes retract
            //It will automatically transition to concealed animation
            col.enabled = false;
            animator.SetInteger("State", 1);
            yield return new WaitForSeconds(postAttackTime);
        }

        //Loop
        StartCoroutine(SpikeAnimation());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.hasTag(TileTags.Creature))
            {
                collision.GetComponent<Tile>().takeDamage(this, damage);
            }
        }
    }
}
