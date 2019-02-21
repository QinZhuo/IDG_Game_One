using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IDG;
using IDG.MobileInput;
public class SkillJoyStick : JoyStick {
    public Image fillImage;
    public Image backImage;
    public SkillList skillList;
    // Use this for initialization
    
    public KeyCode pcKey = KeyCode.Mouse0;

    // Update is called once per frame

    public FightClientForUnity3D unityClient;
    void Start () {
       

    }
	void Update () {

        PcControl();
        SkillMask();
    }
    void PcControl()
    {
        if (!useKey || onDrag|| unityClient.client.localPlayer==null) return;
        isDown = Input.GetKey(pcKey);

        Vector3 pos =Input.mousePosition- unityClient.mainCamera.WorldToScreenPoint(unityClient.client.localPlayer.view.transform.position);
        moveObj.transform.position = transform.position + pos.normalized * maxScale;
        Vector3 tmp = GetVector3();
        dir = new Fixed2(tmp.x, tmp.y);
    }
    void SkillMask()
    {
        if (skillList == null)
        {
            if (unityClient != null
                && unityClient.client.localPlayer != null
                )
            {
                skillList = (unityClient.client.localPlayer as PlayerData).skillList;
            }
        }
        SkillBase skill = GetSkill(key);

        if (skill != null)
        {
            fillImage.fillAmount = (skill.timer / skill.time).ToFloat();
            backImage.enabled = fillImage.fillAmount > 0;
            if(!useKey)group.blocksRaycasts = fillImage.fillAmount <= 0;
        }

    }

    SkillBase GetSkill(KeyNum key)
    {
        if (skillList!=null&&skillList.skillTable[key].Count > 0)
        {
            return skillList.skillTable[key][0];
        }
        return null;
    }
}
