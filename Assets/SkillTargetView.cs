using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.MobileInput;
public class SkillTargetView : MonoBehaviour {
    public JoyStick joyStick;
    public GameObject dirTarget;
	// Use this for initialization
	void Start () {
        joyStick.BeginMove += OnBegin;
        joyStick.OnMove += OnDrag;
        joyStick.EndMove += OnEnd;

    }
    private void Update()
    {
        if (FightClientForUnity3D.Instance != null&& FightClientForUnity3D.Instance.playerData!=null)
        {
            transform.position = FightClientForUnity3D.Instance.playerData.view.transform.position;
        }
    }
    public void OnBegin()
    {
        dirTarget.SetActive(true);
    }
    public void OnDrag(Fixed2 fixed2)
    {
        dirTarget.transform.rotation=Quaternion.LookRotation(fixed2.ToVector3());
    }
    public void OnEnd(Fixed2 fixed2)
    {
        dirTarget.SetActive(false);
    }
}
