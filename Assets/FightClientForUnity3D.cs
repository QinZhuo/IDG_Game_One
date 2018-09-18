using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
using IDG.MobileInput;
using UnityEngine.UI;
public class FightClientForUnity3D : MonoBehaviour {
    
    protected FightClient client;
    public static FightClientForUnity3D instance;
    public List<JoyStick> joySticks;
    public static FightClientForUnity3D Instance
    {
        get { return instance; }
    }
    // Use this for initialization
    void Awake () {
        if (instance == null) instance = this;
        client = new FightClient();
        client.Connect("127.0.0.1", 12345,10);
        InputCenter.Instance.AddKey(GetKey);
        foreach (JoyStick joyStick in joySticks)
        {
            InputCenter.Instance.AddJoyStick(joyStick.frameKey,joyStick.GetInfo());
        }
        //V2 v2 = new V2(1, 0);
        //for (int i =0; i <= 360; i+=30)
        //{
        //   // Debug.Log("sin"+i+":"+MathR.SinAngle(new Ratio(i)).ToFloat());
        //   // Debug.Log("cos" + i + ":" + MathR.CosAngle(new Ratio(i)).ToFloat());
        //}

        // InputCenter.Instance.framUpdate += FrameUpdate;
        //  V2 v2 = new V2(-1,-1);
        //  Debug.Log(v2.ToRotation());
        //  Debug.LogError((10 & 2) == 2);
        // Debug.LogError(V2.left+V2.up);
        //Ratio r = new Ratio(16);
        //Debug.Log(r.SQR(r));
        //Debug.Log(r.InvSqrt(16));
       
    }
	

  
    
	// Update is called once per frame
	void Update () {
        
       if (client.MessageList.Count > 0)
       {
           client.ParseMessage(client.MessageList.Dequeue());
       }
        InputCenter.Instance.ResetKey();
        
        //CommitKey();
    }
    public FrameKey GetKey()
    {
        FrameKey key=0;
        if (Input.GetKey(KeyCode.A))
        {
            key|=FrameKey.Left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            key |= FrameKey.Right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            key |= FrameKey.Down;
        }
        if (Input.GetKey(KeyCode.W))
        {
            key |= FrameKey.Up;
        }
        return key;
    }
    public void OnDestroy()
    {
        client.Stop();
    }
}
