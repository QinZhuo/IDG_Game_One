using UnityEngine;
using IDG;
using System.Collections;
using System.Collections.Generic;
using System;
public class ItemManager
{
    public static ItemManager Instance
    {
        get
        {
            if(_m_instance!=null){
                return _m_instance;
            }else
            {
                _m_instance=new ItemManager();
                return _m_instance;
            }
        }
    }
    protected static ItemManager _m_instance;    
    public Dictionary<SkillId,Func<SkillBase>> skillTable;
    public ItemManager(){
        
        skillTable=new Dictionary<SkillId, Func<SkillBase>>();
        skillTable.Add(SkillId.Shoots,()=>{return new SkillShoots();});
        skillTable.Add(SkillId.Gun,()=>{return new SkillGun();});
         skillTable.Add(SkillId.Ray,()=>{return new SkillRay();});
    }
    public static SkillBase GetSkill(SkillId skillId){
        
        return Instance.skillTable[skillId]();
    }
} 

public enum SkillId
{
    Shoots,
    Gun,
    Ray,
}