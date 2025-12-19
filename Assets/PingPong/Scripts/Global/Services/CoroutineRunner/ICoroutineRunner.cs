using System.Collections;
using UnityEngine;

namespace PingPong.Scripts.Global.Services.CoroutineRunner
{
    public interface ICoroutineRunner : IProjectService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}