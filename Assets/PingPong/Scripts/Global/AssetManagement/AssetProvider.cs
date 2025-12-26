using PingPong.Scripts.Global.Services.GameMusicPlayer;
using UnityEngine;

namespace PingPong.Scripts.Global.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Vector3 place)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, place, Quaternion.identity);
        }
        
        public GameObject Instantiate(string path, Vector3 place, Quaternion rotation)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, place, rotation);
        }
    }
}