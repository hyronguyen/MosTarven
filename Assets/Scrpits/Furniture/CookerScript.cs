using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

public class CookerScript : MonoBehaviour
{
    // LOCAL VARIABLES ##########################################################################
    [Header("Animation:")]
    [SerializeField]
    private Animator animator;
    private AnimationClip idleClip;
    private AnimationClip cookingClip;
    private AnimationClip doneClip;
    public float animationSpeed = 3f; 

    [Header("Thông tin chung:")]
    private Funiture furnituredDetail; // Thông tin về đồ nội thất

    [Header("Thông tin nấu ăn:")]
    [SerializeField]
    private GameObject foodPrefab; 
    public string cookerId; 
    private GameObject FoodSelector;
    private float cookingTime ;
    private bool isCooking = false;
    private bool isDone = false;
    private string cookerCode;

#region LIFE 
    void Update(){
        if(cookingTime > 0){
            cookingTime -= Time.deltaTime;
            isCooking = true;
            animator.SetBool("isCooking", isCooking);
            // Debug.Log("[CookerScript] Đang nấu món ăn: " + foodPrefab.name + " Thời gian còn lại: " + cookingTime);
            if(cookingTime <= 0){
                Debug.Log("[CookerScript] Món ăn đã sẵn sàng: " + foodPrefab.name);
            isDone = true;
            isCooking = false;
             animator.SetBool("isDone", isDone);
            }
        }
    }

    void Start()
    {
        cookerId = "CO" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Debug.Log("Cooker ID: " + cookerId);

        furnituredDetail = GetComponent<Funiture>();
        cookerCode = furnituredDetail.GetFunitureCode();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("[CookerScript] Không tìm thấy animator: " + gameObject.name);
        }

        Transform parent = GameObject.Find("Canvas").transform;
        FoodSelector = parent.Find("FoodSellect_panel")?.gameObject;
        if (FoodSelector != null)
        {
            Debug.Log("[CookerScript] Đã tìm thấy FoodSellect_panel");
        }
        else
        {
            Debug.LogError("[CookerScript] Không tìm thấy FoodSellect_panel trong scene");
        }
     LoadSpritesAndSetupClips();
    }
#endregion

#region RENDER AND ANIMATION SETUP
#if UNITY_EDITOR
      void LoadSpritesAndSetupClips()
{
    if (string.IsNullOrEmpty(cookerCode))
    {
        Debug.LogError("[CookerScript] Chưa thiết lập cookerCode cho " + gameObject.name);
        return;
    }

    // Load tất cả sprite trong Resources/CookerSprites/{cookerCode}
    Sprite[] sprites = Resources.LoadAll<Sprite>($"CookerSprites/{cookerCode}");

    Debug.Log($"[CookerScript] CookerCode={cookerCode}, Loaded={sprites.Length} sprites");

    for (int i = 0; i < sprites.Length; i++)
    {
        Debug.Log($"[{cookerCode}] Sprite[{i}] = {sprites[i].name}");
    }

    if (sprites == null || sprites.Length < 9)
    {
        Debug.LogError($"[CookerScript] Không đủ sprite cho {cookerCode}. Yêu cầu tối thiểu 9 sprite (0–8)");
        return;
    }

    AnimationClip idle    = CreateClipFromSprites(new Sprite[] { sprites[0], sprites[1], sprites[2] }, 5f, $"{cookerCode}_idle");
    AnimationClip cooking = CreateClipFromSprites(new Sprite[] { sprites[3], sprites[4], sprites[5] }, 5f, $"{cookerCode}_cooking");
    AnimationClip done    = CreateClipFromSprites(new Sprite[] { sprites[6], sprites[7], sprites[8] }, 5f, $"{cookerCode}_done");

    Animator animator = GetComponent<Animator>();
    if (animator != null && animator.runtimeAnimatorController != null)
    {
        AnimatorOverrideController overrideCtrl = new AnimatorOverrideController(animator.runtimeAnimatorController);

        overrideCtrl["Cooker_idle"]    = idle;
        overrideCtrl["Cooker_cooking"] = cooking;
        overrideCtrl["Cooker_done"]    = done;

        animator.runtimeAnimatorController = overrideCtrl;
        Debug.Log($"[CookerScript] ✔️ {cookerCode} đã có Animator riêng với Idle/Working/Done");
    }
    else
    {
        Debug.LogError($"[CookerScript] Không tìm thấy Animator cho {gameObject.name}");
    }
}

AnimationClip CreateClipFromSprites(Sprite[] sprites, float frameRate, string clipName)
{
    if (sprites == null || sprites.Length == 0)
    {
        Debug.LogError($"[CreateClipFromSprites] Không tìm thấy sprite nào cho clip {clipName}");
        return null;
    }

    Debug.Log($"[CreateClipFromSprites] Clip {clipName} có {sprites.Length} sprites");

    AnimationClip clip = new AnimationClip
    {
        frameRate = frameRate,
        name = clipName
    };

    // ⚡ Bắt buộc: Set loop
    AnimationClipSettings settings = new AnimationClipSettings
    {
        loopTime = true
    };
    AnimationUtility.SetAnimationClipSettings(clip, settings);

    EditorCurveBinding binding = new EditorCurveBinding
    {
        type = typeof(SpriteRenderer),
        path = "",
        propertyName = "m_Sprite"
    };

    ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[sprites.Length];
    for (int i = 0; i < sprites.Length; i++)
    {
        keyFrames[i] = new ObjectReferenceKeyframe
        {
            time = (float)i / frameRate,
            value = sprites[i]
        };
    }

    AnimationUtility.SetObjectReferenceCurve(clip, binding, keyFrames);

    return clip;
}


#endif
#endregion


#region UNITY EVENTS
    void OnMouseDown()
    {
        if(isCooking){
            Debug.Log("[CookerScript] Đang nấu món ăn, không thể chọn món mới.");
        }
        else if (!isCooking && animator != null && !isDone)
        {
            FoodSelector.SetActive(true);
            FoodSelector.GetComponent<FoodSelector>().SetCookerId(cookerId);
            
        }
        else if (isDone && animator != null && !isCooking){
            isDone = false;
            animator.SetBool("isCooking", isCooking);
            animator.SetBool("isDone", isDone);
            Debug.Log("[CookerScript] Món ăn đã sẵn sàng, bạn có thể lấy món ăn ra.");
        }
    }
#endregion

#region PUBLIC METHODS
    public void SetFood(GameObject foodPrefab,int cookingTime)
    {

        this.foodPrefab = foodPrefab;
        this.cookingTime = cookingTime;
    }

    public void GetFoodId()
    {
        Debug.Log("[CookerScript] Food ID: " + foodPrefab.name);
    }

    public void SetCookerCode(string code)
    {
        this.cookerCode = code;
        Debug.Log("[CookerScript] Cooker code: " + cookerCode);
    }
}

#endregion