using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarriorAttack : PlayerAttack
{
    private Animator playerAnimator;


    //SPELL 1
    public Transform spell1AttackPoint;
    public float spell1AttackRange = 1.5f;
    //[SerializeField] private float spell1WaitAnimationTime = 2f;



    //SPELL 2
    // public Transform spell2AttackPoint; // on utilisera ici le spell3Attackpoint
    public float spell2AttackRange = 5f;
    private bool isOnFloor; //Variable qui nous dit si le player est sur le sol ou non
    [SerializeField] private float animationJumpWait = 0.25f;



    //SPELL 3
    public Transform spell3AttackPoint;
    public float spell3AttackRange = 3f; // AOE area range
    [SerializeField] public float jumpForwardForce = 10f;


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && spell1CoolDownTimer <= 0 && !attacking) //premiere attaque(spell 1), clic gauche
        {
            StartCoroutine(SkillsCooldown.Spell1Cooldown(spell1CoolDown));
            StartCoroutine(Spell1Attack());
        }

        if (Input.GetMouseButtonDown(1) && spell2CoolDownTimer <= 0 && !attacking) //deuxieme attaque(spell 2), clique droit
        {
            StartCoroutine(SkillsCooldown.Spell2Cooldown(spell2CoolDown));
            StartCoroutine(Spell2Attack());
        }

        if (Input.GetKeyDown(KeyCode.Space) && spell3CoolDownTimer <= 0 && !attacking) //deuxieme attaque(spell 3), sur espace
        {
            StartCoroutine(SkillsCooldown.Spell3Cooldown(spell3CoolDown));
            StartCoroutine(Spell3Attack());
        }



        // COOLDOWN
        if (spell1CoolDownTimer > 0) //gestion du CoolDown du spell 1
        {
            spell1CoolDownTimer -= Time.deltaTime;
        }
        if (spell2CoolDownTimer > 0) //gestion du CoolDown du spell 2
        {
            spell2CoolDownTimer -= Time.deltaTime;
        }
        if (spell3CoolDownTimer > 0) //gestion du CoolDown du spell 3
        {
            spell3CoolDownTimer -= Time.deltaTime;
        }
    }


    IEnumerator Spell1Attack()
    {
        attacking = true; //nous attaquons
        spell1CoolDownTimer = spell1CoolDown; //lancement du cooldown de l'attaque 

        playerAnimator.SetTrigger("Trigger");
        playerAnimator.SetFloat("Trigger Number", 2);
        playerAnimator.SetTrigger("BasicAttack");

        yield return new WaitForSeconds(0.6f); 

        //detecter les enemy in range
        Collider[] hitEnemies = Physics.OverlapSphere(spell1AttackPoint.position, spell1AttackRange, enemyLayers);

        //infliger les degats aux ennemies
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(spell1Damage);
            enemy.GetComponent<EnemyHealth>().TakeKnockBack(transform.position, -11);
        }
        attacking = false;
        yield return null;
    }

    IEnumerator Spell2Attack()
    {
        attacking = true; //nous attaquons
        spell2CoolDownTimer = spell2CoolDown; //lancement du cooldown de l'attaque 

        //lancer les animations A FAIRE!!!!!!

        playerAnimator.SetTrigger("JumpAttack");


        gameObject.GetComponent<PlayerMovement>().enabled = false; // On désactive le mouvement

        yield return new WaitForSeconds(animationJumpWait);

        GetComponent<Rigidbody>().AddForce(transform.up * 6, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(transform.forward * jumpForwardForce, ForceMode.Impulse); // Fait sauter le joueur en avant

        isOnFloor = false;
        while (isOnFloor == false) // On attend que le player touche de nouveau le sol
        {
            yield return null; 
        }

        //playerAnimator.SetTrigger("OnFloor");

        gameObject.GetComponent<PlayerMovement>().enabled = true; // On redonne accès au mouvement
        Collider[] hitEnemies = Physics.OverlapSphere(spell3AttackPoint.position, spell2AttackRange, enemyLayers); //infliger les degats aux ennemies

        foreach (Collider enemy in hitEnemies) //infliger les degats aux ennemies
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(spell2Damage); // 5 de degats est a titre de test, on appelera un fonction pour calculer les DD
            enemy.GetComponent<EnemyHealth>().TakeKnockBack(transform.position, -11);
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero; // On annule l'impulsion du saut
        attacking = false;
        yield return null;
    }

    void OnCollisionEnter(Collision collision) //Permet de savoir si le joueur a touché le sol 
    {
        if (collision.collider.name == "Floor") //Détecte une collision avec Floor
        {
            isOnFloor = true;
        }
    }


    IEnumerator Spell3Attack()
    {
        attacking = true; //nous attaquons
        spell3CoolDownTimer = spell3CoolDown; //lancement du cooldown de l'attaque
        playerAnimator.SetTrigger("360Attack");
        float global_timer = 0f;
        float timer = 1f;
        while (global_timer <= 3f)
        {
            if (timer >= 0.5)
            {
                Debug.Log("yaa");
                Collider[] hitEnemies = Physics.OverlapSphere(spell3AttackPoint.position, spell3AttackRange, enemyLayers); //infliger les degats aux ennemies

                foreach (Collider enemy in hitEnemies) //infliger les degats aux ennemies
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(spell3Damage); // 5 de degats est a titre de test, on appelera un fonction pour calculer les DD
                }
                   
                timer = 0f;
            }
            global_timer += Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        attacking = false;
        yield return null;

    }
}
