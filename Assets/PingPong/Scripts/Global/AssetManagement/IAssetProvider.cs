using PingPong.Scripts.Global.Services;
using UnityEngine;

namespace PingPong.Scripts.Global.AssetManagement
{
    public interface IAssetProvider : IProjectService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 place);
        GameObject Instantiate(string path, Vector3 place, Quaternion rotation);
    }
}