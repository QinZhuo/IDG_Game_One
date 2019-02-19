using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FSClient;
using IDG.MobileInput;
using UnityEngine.UI;
/// <summary>
/// 【战斗客户端Unity接口】管理【战斗客户端】并在每帧进行解析消息
/// </summary>
public class FightClientForUnity3D : MonoBehaviour {
    
    public FSClient client;
    //public static FightClientForUnity3D instance;
    public List<JoyStick> joySticks;
    public int testCount=2;
    static int idCount=0;
    public int id;
    Dictionary<KeyCode, int> KeyToId;
    // public static FightClientForUnity3D Instance
    // {
    //     get { return instance; }
    // }
    // Use this for initialization
    void Awake () {
        id = idCount++;
        if (idCount <= 1)
        {
            for (int i = 0; i < testCount; i++)
            {
                GameObject.Instantiate(gameObject);
            }
        }
        client = new FSClient();
        KeyToId = new Dictionary<KeyCode, int>();
        KeyToId.Add(KeyCode.F1, 0);
        KeyToId.Add(KeyCode.F2, 1);
        KeyToId.Add(KeyCode.F3, 2);
        client.unityClient = this;
        client.Connect("127.0.0.1", 12345,10);

        // foreach (JoyStick joyStick in joySticks)
        // {
        //     InputCenter.Instance.AddJoyStick(joyStick.frameKey,joyStick.GetInfo());
        // }
       
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

        // V2 v2 = new V2(-5, 99);
        // Debug.Log("(-5, 99)" + v2.x * v2.x + "," + v2.y * v2.y);
        //  Debug.Log("normalized"+v2.normalized);
      

    }
	

  
    
	// Update is called once per frame
	void Update () {
        ChangeClient();
       if (client.MessageList.Count > 0)
       {
           client.ParseMessage(client.MessageList.Dequeue());
       }
       
        
        CommitKey();
    }
    public void ChangeClient()
    {
        foreach (var t in KeyToId)
        {
            if (Input.GetKeyDown(t.Key))
            {
                if (id == t.Value)
                {
                    ChildActive(true);
                }
                else
                {
                    ChildActive(false);
                }
            }
        }
 
    }
    public void ChildActive(bool active)
    {
        foreach (var item in GetComponentsInChildren<Transform>(true))
        {
            if (item.gameObject != gameObject)
            {
                item.gameObject.SetActive(active);
            }
        }
    }
    public void CommitKey()
    {
        
        // if (Input.GetKey(UnityEngine.KeyCode.J))
        // {
        //     key |= IDG.KeyNum.Attack;
        // }
       // InputCenter.Instance.
        client.inputCenter.SetKey(Input.GetKey(KeyCode.J),KeyNum.Attack);


        foreach (var joy in joySticks)
        {
              client.inputCenter.SetJoyStick(joy.key,joy.GetInfo());
        }
        
        // InputCenter.Instance.SetKey(Input.GetKey(KeyCode.A),KeyNum.Left);
        // InputCenter.Instance.SetKey(Input.GetKey(KeyCode.D),KeyNum.Right);
        // InputCenter.Instance.SetKey(Input.GetKey(KeyCode.W),KeyNum.Up);
        // InputCenter.Instance.SetKey(Input.GetKey(KeyCode.S),KeyNum.Down);
        
    }
    public void OnDestroy()
    {
        client.Stop();
    }
}
