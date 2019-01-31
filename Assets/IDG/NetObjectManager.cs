using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG.FSClient;
namespace IDG
{
    public class NetObjectManager
    {
        public static GameObject Instantiate<T>(NetData data) where T:NetData,new()
        {
            GameObject obj = GameObject.Instantiate(GetPrefab(data), data.transform.Position.ToVector3(), data.transform.Rotation.ToUnityRotation());
            obj.GetComponent<NetObjectView<T>>().data = data;
            data.view = obj.GetComponent<NetObjectView<T>>();
            return obj;
        }
        public static void Destory<T>(MonoBehaviour show) where T : NetData, new()
        {
            if (show == null) { Debug.Log("show is Null"); }
            (show as NetObjectView<T>).data.Destory();
            GameObject.Destroy(show.gameObject);
        }
        public static GameObject GetPrefab(NetData data)
        {
            return Resources.Load(data.PrefabPath())as GameObject;
        }
        public static GameObject GetPrefab(string PrefabPath)
        {
            return Resources.Load(PrefabPath) as GameObject;
        }
    }
}