using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IDG.FSClient
{
    public class NetObjectManager
    {
        FSClient client;
        public NetObjectManager(FSClient fsClient){
            client=fsClient;
        }

        public GameObject Instantiate(NetData data)
        {
            GameObject obj = GameObject.Instantiate(GetPrefab(data), data.transform.Position.ToVector3(), data.transform.Rotation.ToUnityRotation());
            obj.transform.parent = (client.unityClient as MonoBehaviour).gameObject.transform;
            var view =obj.GetComponent<View>();
            view.data = data;
            data.view = view;
            return obj;
        }
        //public GameObject Instantiate<T>(NetData data) where T:NetData,new()
        //{
        //    GameObject obj = GameObject.Instantiate(GetPrefab(data), data.transform.Position.ToVector3(), data.transform.Rotation.ToUnityRotation());
        //    obj.GetComponent<NetObjectView<T>>().data = data;
        //    data.view = obj.GetComponent<NetObjectView<T>>();
        //    data.InitClient(client);
        //    return obj;
        //}
        //public void Destory<T>(MonoBehaviour show) where T : NetData, new()
        //{
        //    if (show == null) { Debug.Log("show is Null"); }
        //    (show as NetObjectView<T>).data.Destory();
        //    GameObject.Destroy(show.gameObject);
        //}
        public void Destory(View view) 
        {
            if (view == null) { Debug.Log("show is Null"); }
            view.data.Destory();
            GameObject.Destroy(view.gameObject);
        }
        public GameObject GetPrefab(NetData data)
        {
            var prefab= Resources.Load(data.PrefabPath()) as GameObject;
            if (prefab == null)
            {
                Debug.LogError("{" + data.PrefabPath() + "}is Null");
            }
            return prefab;
        }
        public GameObject GetPrefab(string PrefabPath)
        {
            var prefab = Resources.Load(PrefabPath) as GameObject;
            if (prefab == null)
            {
                Debug.LogError("{" + PrefabPath + "}is Null");
            }
            return prefab;
        }
    }
}