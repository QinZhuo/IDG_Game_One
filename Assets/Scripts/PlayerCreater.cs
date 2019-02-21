using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreater : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        var client = GetComponentInParent<FightClientForUnity3D>().client;
        for (int i = 0; i < 3; i++)
        {
            var player = new PlayerData();
            player.Init(client);
            player.clientId = i;
            player.transform.Position = new IDG.Fixed2(0, i);
            client.objectManager.Instantiate(player);
        }
        

	}
	

}
