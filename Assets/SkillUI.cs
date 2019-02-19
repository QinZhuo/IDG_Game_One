using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IDG;
using IDG.MobileInput;
public class SkillUI : MonoBehaviour {
    public JoyStick joy;
    public Image fillImage;
    public Image backImage;
    public SkillList skillList;
	// Use this for initialization
	
	
	// Update is called once per frame

    public FightClientForUnity3D unityClient;
    void Start () {
       

    }
	void Update () {
        if (skillList == null)
        {
            if(unityClient!=null
                &&unityClient.client.localPlayer!=null
                )
            {
                skillList =(unityClient.client.localPlayer as PlayerData).skillList;
            }
        }
        SkillBase skill = GetSkill(joy.key);
       
        if (skill != null)
        {
            fillImage.fillAmount =(skill.timer / skill.time).ToFloat();
            backImage.enabled = fillImage.fillAmount > 0;
        }
        
        

    }
    
    
    SkillBase GetSkill(KeyNum key)
    {
        if (skillList!=null&&skillList.skillTable[joy.key].Count > 0)
        {
            return skillList.skillTable[joy.key][0];
        }
        return null;
    }
}
