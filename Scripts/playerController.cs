using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using TMPro;

public class playerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] Image healthbarImage;
    [SerializeField] Image HurtEffectImage;
    private float HurtEffectImageWait;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] public static float jumpForce = 9;
    [SerializeField] public static float walkSpeed = 12;
    public float mouseSensitivity = 1f;
    [HideInInspector]
    public PhotonView PV;
    bool grounded;
    Rigidbody rb;
    
    
    const float maxHealth = 100f;
    public float currentHealth = maxHealth;
    playerManager playerManager;
    [HideInInspector]public static bool GameIsPaused;
    public Pause pauseScript;
    [SerializeField] public Image Crosshair;
    public AudioClip footstepSound;
    public AudioClip gotKillSound;
    public AudioSource audioSource;
   
    public Image EyeBoostInnerGlow;
    public Camera FPScam;
    public Camera WeaponCam;
    [HideInInspector] public static playerController instance;
    public bool IsAlive;
    public Camera EyeBoostCamera;
    public Animator anim;
    [SerializeField]GameObject PlayerHead;
    [SerializeField]GameObject PlayerBody;
    [SerializeField] private float smoothing;
    private Vector2 smoothedVelocity;
    private Vector2 currentLookingPosition;
    public Transform itemHolder;
    public TMP_Text healthText;
    public TMP_Text ShieldText;
    private bool canGetHealth = false;
    public TMP_Text KillFeedText;
    public CameraShake cameraShake;
    bool HasBoostPowerUp = false;

    //Hotbar
    Image slot1;
    Image slot2;
    Image slot3;

    //loadout
    private float WeaponSwitchCooldown = 0.125f;
    private float lastSwitch;
    [SerializeField] int itemIndex;
    int previousItemIndex = -1;
    [SerializeField] ItemScript[] items;

    //power up 
    public Image speed;
    public Image jump;
    public Image weapon;
    public Image eye;
    public Image health;
    private int healthAddAmount = 1;

    void Awake()
    {
        slot1 = transform.GetChild(3).transform.GetChild(8).transform.GetChild(0).transform.GetComponent<Image>();
        slot2 = transform.GetChild(3).transform.GetChild(8).transform.GetChild(1).transform.GetComponent<Image>();
        slot3 = transform.GetChild(3).transform.GetChild(8).transform.GetChild(2).transform.GetComponent<Image>();
        HasBoostPowerUp = false;
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            //PlayerBody.transform.GetComponent<CapsuleCollider>().radius = 0.05f;
            PlayerBody.transform.GetComponent<MeshRenderer>().enabled = false;
            PlayerBody.layer = 2;
        }
        else
        {
            //PlayerBody.transform.GetComponent<CapsuleCollider>().radius = 0.5f;
            PlayerBody.transform.GetComponent<MeshRenderer>().enabled = true;
            PlayerBody.layer = 7;
        }
        EyeBoostInnerGlow.enabled = false;
        EyeBoostCamera.enabled = false;
        if(ColorPicker.HasChosenColor == false)
        {
            Crosshair.color = Color.white;
            Crosshair.GetComponent<Outline>().effectColor = Color.white;
        }
        else
        {
            Crosshair.color = ColorPicker.instance.PreviewCrosshair2.color;
            Crosshair.GetComponent<Outline>().effectColor = ColorPicker.instance.PreviewCrosshair2.color;
        }
        
        instance = this;
        mouseSensitivity = PlayerPrefs.GetFloat("SENSITIVITY");
        FPScam.fieldOfView = PlayerPrefs.GetFloat("FOV");
        EyeBoostCamera.fieldOfView = PlayerPrefs.GetFloat("FOV");
        WeaponCam.fieldOfView = PlayerPrefs.GetFloat("FOV");
        if (PlayerPrefs.GetString("CROSSHAIR") == "True")
            Crosshair.enabled = true;
        else
            Crosshair.enabled = false;
        Pause.paused = false;
        rb = GetComponent<Rigidbody>();
        

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<playerManager>();

        WeaponCam.enabled = false;
        WeaponCam.enabled = true;
    }
    private void ChangeLayersRecursively(GameObject p_target, int p_layer)
    {
        p_target.layer = p_layer;
        foreach (Transform a in p_target.transform) ChangeLayersRecursively(a.gameObject, p_layer);
    }
    void Start()
    {
        
        canGetHealth = true;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }

        

    }
    void Update()
    {

        
        WeaponCam.enabled = false;
        WeaponCam.enabled = true;

        
        
        IsAlive = playerManager.isAlive;
        if(FPScam.fieldOfView <= 60)
        {
            FPScam.fieldOfView = 60f;
        }
        if (WeaponCam.fieldOfView <= 60)
        {
            WeaponCam.fieldOfView = 60f;
        }
        if (EyeBoostCamera.fieldOfView <= 60)
        {
            EyeBoostCamera.fieldOfView = 60f;
        }
        
        if(HurtEffectImageWait > 0)
        {
            HurtEffectImageWait -= Time.deltaTime;
        }
        else
        {
            HurtEffectImage.color = Color.Lerp(HurtEffectImage.color, new Color(1, 1, 1, 0), Time.deltaTime * 3.5f);
        }
        
        if(!PV.IsMine)
            return;

        

        PV.RPC("HealthAdding", RpcTarget.All);
        healthText.text = currentHealth.ToString();
        bool pause = Input.GetKeyDown(KeyCode.Escape);

        if (pause)
        {
            GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
        }
        
        if(Pause.paused)
            return;
        
        RotateCamera();
        Jump();
        
        for(int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (Time.time - lastSwitch < WeaponSwitchCooldown)
                return;
            lastSwitch = Time.time;
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
                
            }
            else
            {
                EquipItem(itemIndex + 1);
                
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (Time.time - lastSwitch < WeaponSwitchCooldown)
                return;
            lastSwitch = Time.time;
            if (itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
                
            }
            else
            {
                EquipItem(itemIndex - 1);
                
            }
        }
        if(Input.GetMouseButton(0))
        {
            items[itemIndex].Use();
        }
        if(transform.position.y < -10f)
        {
            Die();
        }
        if(itemIndex == 0)
        {
            Crosshair.fillAmount = 1f;
            slot1.color = new Color(1, 1, 1, 1);
            slot2.color = new Color(1, 1, 1, 0.35f);
            slot3.color = new Color(1, 1, 1, 0.35f);
        }
        if(itemIndex == 1)
        {
            Crosshair.fillAmount = 0.6f;
            slot1.color = new Color(1, 1, 1, 0.35f);
            slot2.color = new Color(1, 1, 1, 1);
            slot3.color = new Color(1, 1, 1, 0.35f);
        }
        if (itemIndex == 2)
        {
            Crosshair.fillAmount = 0f;
            slot1.color = new Color(1, 1, 1, 0.35f);
            slot2.color = new Color(1, 1, 1, 0.35f);
            slot3.color = new Color(1, 1, 1, 1);
        }
        


    }
    public void SpeedUpgrade()
    {
        if (!PV.IsMine)
            return;
        speed.color = new Color(1, 1, 1, 1);
        jump.color = new Color(0, 0, 0, 0);
        weapon.color = new Color(0, 0, 0, 0);
        eye.color = new Color(0, 0, 0, 0);
        health.color = new Color(0, 0, 0, 0);
        EyeBoostCamera.enabled = false;
        walkSpeed = 18f;
    }
    public void JumpUpgrade()
    {
        if (!PV.IsMine)
            return;
        speed.color = new Color(0, 0, 0, 0);
        jump.color = new Color(1, 1, 1, 1);
        weapon.color = new Color(0, 0, 0, 0);
        eye.color = new Color(0, 0, 0, 0);
        health.color = new Color(0, 0, 0, 0);
        EyeBoostCamera.enabled = false;
        jumpForce = 14f;
        HasBoostPowerUp = true;
    }
    public void WeaponUpgrade()
    {
        if (!PV.IsMine)
            return;
        speed.color = new Color(0, 0, 0, 0);
        jump.color = new Color(0, 0, 0, 0);
        weapon.color = new Color(1, 1, 1, 1);
        eye.color = new Color(0, 0, 0, 0);
        health.color = new Color(0, 0, 0, 0);
        SingleShotGun.cooldown = 0.3f;
        EyeBoostCamera.enabled = false;
    }
    public void EyeUpgrade()
    {
        if (!PV.IsMine)
            return;
        speed.color = new Color(0, 0, 0, 0);
        jump.color = new Color(0, 0, 0, 0);
        weapon.color = new Color(0, 0, 0, 0);
        eye.color = new Color(1, 1, 1, 1);
        health.color = new Color(0, 0, 0, 0);
        EyeBoostCamera.enabled = true;
        EyeBoostInnerGlow.enabled = true;
        anim.Play("BlueInnerGlowFadeIn");
        
    }
    public void HealthUpgrade()
    {
        if (!PV.IsMine)
            return;
        speed.color = new Color(0, 0, 0, 0);
        jump.color = new Color(0, 0, 0, 0);
        weapon.color = new Color(0, 0, 0, 0);
        eye.color = new Color(0, 0, 0, 0);
        health.color = new Color(1, 1, 1, 1);
        EyeBoostCamera.enabled = false;
        healthAddAmount = 10;
    }
    public void ResetUpgrade()
    {
        if (!PV.IsMine)
            return;
        speed.color = new Color(0, 0, 0, 0);
        jump.color = new Color(0, 0, 0, 0);
        weapon.color = new Color(0, 0, 0, 0);
        eye.color = new Color(0, 0, 0, 0);
        health.color = new Color(0, 0, 0, 0);
        EyeBoostCamera.enabled = false;
        walkSpeed = 12f;
        healthAddAmount = 1;
        HasBoostPowerUp = false;
        jumpForce = 9f;
        SingleShotGun.cooldown = 0.75f;
        anim.Play("BlueInnerGlowFadeOut");
        
    }
    [PunRPC]
    public void HealthAdding()
    {
        if(!PV.IsMine)
            return;

        if(canGetHealth == true && currentHealth < 100)
            StartCoroutine(Health());
    }
    IEnumerator Health()
    {
        canGetHealth = false;
        yield return new WaitForSeconds(2);
        currentHealth += healthAddAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        healthbarImage.fillAmount = currentHealth / maxHealth;
        if (healthbarImage.fillAmount <= 0.3f)
            healthbarImage.color = Color.red;
        else
            healthbarImage.color = Color.green;
        canGetHealth = true;
    }
    
    void Move()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        
        Vector3 moveVector = transform.TransformDirection(new Vector3(xMove, 0, zMove));
        if(moveVector.sqrMagnitude > 1.0f)
        {
            moveVector.Normalize();
        }
        rb.velocity = (moveVector * walkSpeed) + new Vector3(0, rb.velocity.y, 0);

        
    }
    void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        inputValues = Vector2.Scale(inputValues, new Vector2(mouseSensitivity * smoothing, mouseSensitivity * smoothing));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);

        currentLookingPosition += smoothedVelocity;

        cameraHolder.transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
        PlayerHead.transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
        PlayerBody.transform.localRotation = Quaternion.AngleAxis(currentLookingPosition.x, PlayerBody.transform.up);

        currentLookingPosition.y = Mathf.Clamp(currentLookingPosition.y, -75f, 70f);

    }
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            rb.velocity += Vector3.up * jumpForce;
        }
    }
    public void SetGrounded(bool _grounded)
    {
        grounded = _grounded;
    }
    public void FixedUpdate()
    {
        if(Pause.paused)
            return;
        if(!PV.IsMine)
            return;
        Move();
        
        
    }
    
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;
        
        itemIndex = _index;
        
        
        if (PV.IsMine) ChangeLayersRecursively(items[itemIndex].itemGameObject.gameObject, 3);
        else ChangeLayersRecursively(items[itemIndex].itemGameObject.gameObject, 0);
        items[itemIndex].itemGameObject.SetActive(true);
        
        

        if(previousItemIndex != -1)
        {
            
            if (PV.IsMine) ChangeLayersRecursively(items[itemIndex].itemGameObject.gameObject, 3);
            else ChangeLayersRecursively(items[itemIndex].itemGameObject.gameObject, 0);
            items[previousItemIndex].itemGameObject.SetActive(false);           
        }
        previousItemIndex = itemIndex;
        if(PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        
        if(!PV.IsMine)
            return;

        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;
        
        if(healthbarImage.fillAmount <= 0.3f)
            healthbarImage.color = Color.red;
        else
            healthbarImage.color = Color.green;
        HurtEffectImage.color = Color.white;
        HurtEffectImageWait = 0.25f;
        

        if (currentHealth <= 0)
        {
            Die();
            
        }
    }
    void Die()
    {
        playerManager.Die();
       
        PlayerPrefs.SetInt("DEATHS", +1);
    }
    [PunRPC]
    public void AddKillField()
    {
        Debug.Log("Killfeed added");

    }
    
    private void OnCollisionEnter(Collision hit)
    {
        switch(hit.gameObject.tag)
        {
            case "BouncePad":
                jumpForce = 14;
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                break;
            default:
                if(HasBoostPowerUp)
                    jumpForce = 14;
                else
                    jumpForce = 9;
                break;
        }
    }
    
}
