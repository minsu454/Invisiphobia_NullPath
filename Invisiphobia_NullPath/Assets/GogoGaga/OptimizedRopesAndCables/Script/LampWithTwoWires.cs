using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Tripolygon.UModeler.UI;
using UnityEngine;


public class LampWithTwoWires : MonoBehaviour
{
    public Rigidbody wire1; // 첫 번째 선의 Rigidbody
    public Rigidbody wire2; // 두 번째 선의 Rigidbody
    public LineRenderer wire1Line; // 첫 번째 선의 LineRenderer
    public LineRenderer wire2Line; // 두 번째 선의 LineRenderer
    public float springStrength = 100f; // 스프링 강도
    public float damper = 10f; // 감쇠

    void Start()
    {
        SpringJoint spring1 = gameObject.AddComponent<SpringJoint>();
        spring1.connectedBody = wire1;
        spring1.spring = springStrength;
        spring1.damper = damper;

        // 두 번째 선 연결
        SpringJoint spring2 = gameObject.AddComponent<SpringJoint>();
        spring2.connectedBody = wire2;
        spring2.spring = springStrength;
        spring2.damper = damper;
    }

    void Update()
    {
        // 첫 번째 연결선(LineRenderer) 업데이트
        wire1Line.SetPosition(0, wire1.position); // 선의 시작점: Wire1의 위치
        wire1Line.SetPosition(1, transform.position); // 선의 끝점: 전등의 위치

        // 두 번째 연결선(LineRenderer) 업데이트
        wire2Line.SetPosition(0, wire2.position); // 선의 시작점: Wire2의 위치
        wire2Line.SetPosition(1, transform.position); // 선의 끝점: 전등의 위치
    }
}
