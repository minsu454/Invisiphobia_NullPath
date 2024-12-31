using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// Entity 초기화 함수
    /// </summary>
    public abstract void Init();
}