using UnityEngine;
using System.Collections;

namespace UnityMovementAI
{
    public class WallAvoidance : MonoBehaviour
    {
        //LayerMask = 어떤 레이어에 물리계산을 적용할 것인가?
        //(어떤 레이어에 대해서 충돌 검사를 할 것인가?)
        //Physics.default~ = 모든 활성화된 레이어에 대해 레이를 검사하도록 설정.
        public LayerMask castMask = Physics.DefaultRaycastLayers;
        private Rigidbody rb;

        //벽을 피할 때 적용할 최대 가속도.
        public float maxAcceleration = 5f;

        public float wallAvoidDistance = 10f;
        public float rayLength = 10f;

        //WallAvodiance를 적용할 오브젝트
        Vector3 avoidanceObject;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3[] directions =
                {transform.forward, -transform.forward, transform.right, -transform.right};

            if (rb != null)
            {
                foreach (var dir in directions)
                {
                    AvoidWall(dir);
                }
            }
            else
            {
                Debug.LogError("WallAvoidance 스크립트의 Rigidbody가 없습니다.");
            }
        }

        public Vector3 AvoidWall(Vector3 direction)
        {
            //WallAvodiance를 적용할 오브젝트 변수
            avoidanceObject = transform.position;

            //기본적으로 반환할 가속도 초기화.
            //장애물이 없을 경우 이값을 반환
            RaycastHit hit;
            Vector3 accelaeartion = Vector3.zero;


            //point = 레이가 충돌이 발생한 위치
            //normal = 충돌이 발생한 위치로부터 이동할 법선벡터.
            //wallAvoidDistance = 회피를 위한 거리(해당 값만큼 멀리 이동)

            //avoidanceObject = 레이를 쏠 시작점(해당 스크립트가 달린 오브젝트)
            //Physics.Raycast & out hit = 레이의 충돌이 감지되면 hit에 정보가 들어옴
            //direction = 레이를 쏠 방향(앞뒤양옆)
            //
            if (Physics.Raycast(avoidanceObject, direction, out hit, rayLength, castMask))
            {
                Vector3 targetPosition = hit.point + hit.normal * wallAvoidDistance;
                return targetPosition * maxAcceleration;
            }

            else
            {
                return Vector3.zero;
            }    
        }
    }
}
