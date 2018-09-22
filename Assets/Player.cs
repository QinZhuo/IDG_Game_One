using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
public class Player : NetObject {
    // public NetInfo net;
    static int playerUnitId = 1;
    public NetObject netPrefab;

    protected new void Start()
    {
        base.Start();
        name = "player" + playerUnitId++;
        //ShapPhysics.tree.Add(net);
        net.Position = new V2(transform.position.x, transform.position.z);
        if (net.Shap == null) net.Shap = new BoxShap(new Ratio(1), new Ratio(1));
        net.name = name;



    }
    //float last;

    protected override void FrameUpdate()
    {

        V2 move = net.Input.GetJoyStickDirection(FrameKey.MoveKey);

        net.Position += move * net.deltaTime;
        if (move.x != 0 || move.y != 0)
        {
            net.Rotation = move.ToRotation();
        }
        if (net.Input.GetKey(FrameKey.Attack))
        {
            Instantiate(netPrefab, net.Position+net.forward, net.Rotation);
            //Instantiate(netPrefab.gameObject, transform.position + transform.forward,net.Rotation.ToUnityRotation());
        }
        //Debug.Log(net.Position);
    }

    // Use this for initialization


    // Update is called once per frame
    //void Update () {

    //}
}
