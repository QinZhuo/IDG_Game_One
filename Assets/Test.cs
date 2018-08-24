using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
using UnityEngine.UI;
public class Test : MonoBehaviour {
    
    protected FightClient client;
    public static Test instance;
    public static Test Instance
    {
        get { return instance; }
    }
	// Use this for initialization
	void Awake () {
        if (instance == null) instance = this;
        client = new FightClient();
        client.Connect("127.0.0.1", 12345,10);
       // InputCenter.Instance.framUpdate += FrameUpdate;

    }
	

  
    
	// Update is called once per frame
	void Update () {
        
            if (client.MessageList.Count > 0)
            {
                ProtocolBase protocol = client.MessageList.Pop();
                byte t = protocol.getByte();
                //Debug.Log("receiveMessage" + t);
                switch ((MessageType)t)
                {

                    case MessageType.Init:
                        client.ServerCon.clientId = protocol.getByte();
                        Debug.Log("clientID:" + client.ServerCon.clientId);

                        break;
                    case MessageType.Frame:
                        InputCenter.Instance.ReceiveStep(protocol);
                        break;
                    default:
                        break;
                }


            }
        
        CommitKey();
    }
    public void CommitKey()
    {
        if (Input.GetKey(KeyCode.A))
        {
            InputCenter.Instance.SetKey(FrameKey.Left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            InputCenter.Instance.SetKey(FrameKey.Right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            InputCenter.Instance.SetKey(FrameKey.Down);
        }
        if (Input.GetKey(KeyCode.W))
        {
            InputCenter.Instance.SetKey(FrameKey.Up);
        }
    }
    public void OnDestroy()
    {
        client.Stop();
    }
}
