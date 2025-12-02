using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class EquipmentSystem : MonoBehaviour
{
    public Transform cam;
    public Animator animator;

    [Space(6)]
    [Header("EQUIPMENTS")]

    [Space(2)]
    public Equipments equiped = Equipments.Shotgun;
    public enum Equipments
    {
        None,
        Cane,
        Shotgun
    }
    

    [Space(2)]
    [Header("Cane")]

    [SerializeField] GameObject lightTouch;
    [SerializeField] bool lightTouchEnable = true;
    [SerializeField] float caneCooldown = .2f;
    [SerializeField] float caneRange = 5f;

    [SerializeField] GameObject caneDetectSphere;
    [SerializeField] GameObject Sonar;
    float caneTimer = 0f;
    Coroutine caneCoroutine = null;



    [Space(2)]
    [Header("Shotgun")]

    [SerializeField] int shotgunDamage = 1;
    [SerializeField] float spray = .2f;
    [SerializeField] float shotgunFirerate = .25f;


    //[SerializeField] float shotgunReload = 1f;
    float shotgunFirerateTimer = 0f;
    int bullet = 2;
    int ammo = 100;


    private void Start()
    {
        playEquipAnimation();
    }
    private void Update()
    {
        //timers
        if(caneTimer < caneCooldown)
        {
            caneTimer += Time.deltaTime;
        }
        if(shotgunFirerateTimer < shotgunFirerate)
        {
            shotgunFirerateTimer += Time.deltaTime;
        }
    }
    public void useEquipment(InputAction.CallbackContext context)
    {
        switch (equiped)
        {
            case Equipments.Cane:
                if (context.started && caneTimer >= caneCooldown && caneCoroutine == null)
                {
                    if (Physics.Raycast(cam.position, cam.forward, 3f,~0,QueryTriggerInteraction.Ignore))
                    {
                        playAnimation("Cane_Use");

                        if (caneCoroutine == null)
                        {
                            caneCoroutine = StartCoroutine(useCaneSkill());
                        }
                    }
                }
                break;
            case Equipments.Shotgun:
                if (context.started)
                {
                    if (bullet > 0 && shotgunFirerateTimer >= shotgunFirerate)
                    {
                        for (int i = 0; i < 5; i++)
                        {

                            float offsetx = Random.Range(-spray, spray);
                            float offsety = Random.Range(-spray, spray);
                            //Randomize the shotgun bullet

                            if (Physics.Raycast(cam.position , cam.forward + ((cam.right * offsetx) + (cam.up * offsety)), out RaycastHit hit, 20f, ~0, QueryTriggerInteraction.Ignore))
                            {
                                if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Enemy")
                                {
                                    if (hit.collider.TryGetComponent<Enemy>(out Enemy E))
                                    {
                                        E.TakeDamage(shotgunDamage);
                                        print((E as SimpleEnemy).health);
                                    }
                                }
                                Global_Sonar.spawnSonar(hit.point, cam.forward);
                            }
                        }
                        shotgunFirerateTimer = 0f;
                        bullet--;
                        animator.SetInteger("Bullet", bullet);
                        playAnimation("Shotgun_Use" , 0);
                    }
                    else if (bullet <= 0 && shotgunFirerateTimer >= shotgunFirerate)
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shotgun_Idle"))
                        {
                            playAnimation("Shotgun_Open",0);
                        }
                        
                    }
                }
                break;


        }
    }

    public void shotgunReload(InputAction.CallbackContext context)
    {
        if (context.started && bullet < 2 && equiped == Equipments.Shotgun)
        {
            if (shotgunFirerateTimer >= shotgunFirerate)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shotgun_Idle"))
                {
                    playAnimation("Shotgun_Open", 0);
                }
            }
        }
    }

    public void switchEquipmentToRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            int tempEquipped = (int)equiped;
            if(tempEquipped < Enum.GetNames(typeof(Equipments)).Length - 1)
            {
                tempEquipped++;
                equiped = (Equipments)tempEquipped;
                playEquipAnimation();
            }
        }
    }
    public void switchEquipmentToLeftt(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            int tempEquipped = (int)equiped;
            if (tempEquipped > 0)
            {
                tempEquipped--;
                equiped = (Equipments)tempEquipped;
                playEquipAnimation();
            }
            
        }
    }
    void playEquipAnimation()
    {
        if (animator.HasState(0, Animator.StringToHash(Enum.GetName(typeof(Equipments), equiped) + "_Equip")))
        {
            animator.Play(Enum.GetName(typeof(Equipments), equiped) + "_Equip");
        }
        else
        {
            Debug.Log("No Animation for " + Enum.GetName(typeof(Equipments), equiped));
        }
    }

    void playAnimation(string animName, float duration = .2f)
    {
        if (animator.HasState(0, Animator.StringToHash(animName)))
        {
            animator.CrossFadeInFixedTime(animName,duration);
        }
        else
        {
            Debug.Log("No Animation for " + animName);
        }
    }

    public void addBullet()
    {
        if(ammo > 0)
        {
            bullet++;
            ammo--;
            animator.SetInteger("Bullet", bullet);
        }
    }

    IEnumerator useCaneSkill()
    {
        yield return new WaitForSeconds(.3f);

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 3f, ~0, QueryTriggerInteraction.Ignore))
        {
            Vector3 tempPos = hit.point + hit.normal * .25f;
            if (lightTouchEnable)
            {
                GameObject light = Instantiate(lightTouch, tempPos, Quaternion.identity);
            }
            //collision
            CaneScan c = Instantiate(caneDetectSphere, tempPos, Quaternion.identity).GetComponent<CaneScan>();
            c.lifeSpan = 4f;
            c.maxSize = caneRange;
            //sonar
            Global_Sonar.spawnSonar(hit.point, Vector3.up, 4f, caneRange);
            caneTimer = 0f;
        }
        caneCoroutine = null;
    }
}
