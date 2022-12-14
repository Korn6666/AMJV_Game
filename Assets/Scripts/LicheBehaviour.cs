using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicheBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform spellPoint;
    public bool isSpelling;
    public bool presence;
    [SerializeField] float spellRange = 10;
    [SerializeField] float spellDamage = 1;
    [SerializeField] float spellHeal = 1;
    [SerializeField] private LayerMask Skeleton;
    [SerializeField] private float CoolDown = 10;
    private float CoolDownTimer;

    [SerializeField] private float spellTime = 5;
    private float spellTimeTimer;
    [SerializeField] private float TimeBetweenAttackOrHeal = 1;



    
    void Start()
    {
       spellTimeTimer = spellTime;
       isSpelling = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Pour vérifier si il doit lancer le sort
        Collider[] hitEntities = Physics.OverlapSphere(spellPoint.position, spellRange);
        foreach (Collider entity in hitEntities)
        {
            if ( entity.tag == "Player" || entity.tag == "Skeleton" || entity.tag == "OtherEnemy")
            {
                if (CoolDownTimer <= 0)
                {
                    StartCoroutine(Spell());
                }
            }
        }

        // CoolDown
        if (CoolDownTimer > 0)
        {
            CoolDownTimer -= Time.deltaTime;
        }

        //Casting
        if (isSpelling)
        {
            if (spellTimeTimer > 0)
            {
                spellTimeTimer -= Time.deltaTime;
            }else 
            { 
                isSpelling = false; 
            }
        }
    }

    private IEnumerator Spell()
    {
        isSpelling = true;
        spellTimeTimer = spellTime;
        CoolDownTimer = CoolDown;

        float Timer = 1.1f;
        while (isSpelling)
        {
            if (Timer > 1)
            {
                Collider[] hitEntities = Physics.OverlapSphere(spellPoint.position, spellRange);
                presence = false;
                foreach (Collider entity in hitEntities) //infliger les degats aux ennemies
                {                   
                    if (entity.tag == "Player")  
                    {
                        entity.GetComponent<PlayerHealth>().TakeDamage(spellDamage);
                        presence = true;
                    }else if (entity.tag == "Skeleton" || entity.tag == "TheOtherEnemy")
                    {
                        entity.GetComponent<EnemyHealth>().TakeDamage(-spellHeal);
                        presence = true;
                    }
                }
                Timer = 0;
            }
            if ( !presence )
            {
                isSpelling = false;
            }
            Timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
