using UnityEngine;

namespace UnityMovementAI
{
    public class WallAvoidance : MonoBehaviour
    {
        // 검사할 레이어 설정(레이어에 없으면 적용되지 않음)
        public LayerMask castMask = Physics.DefaultRaycastLayers; 
        [SerializeField] private Rigidbody rb;

        public float maxAcceleration = 40f; // 최대 회피 가속도(레이 감지시 멀어질 힘)
        public float wallAvoidDistance = 1f; // 벽과 떨어질 목표 거리
        public float rayLength = 2f; // 레이 길이
        public int rayCount = 32; // 쏠 레이의 개수 (360도 수평 방향)

        public AnimationCurve avoidanceStrengthCurve; // 거리 기반 가속도 비율 조정

        private void Start()
        {
            if (rb == null)
            {
                //벽 회피에 힘을 가해줄 Rigidbody
                rb = GetComponent<Rigidbody>();
            }

            // AnimationCurve 초기화 (선형으로 조정)
            //AnimationCurve를 이용해서 가까우면 밀어내는 힘이 강하고
            //멀면 힘이 약해지게 조정.
            if (avoidanceStrengthCurve == null)
            {
                avoidanceStrengthCurve = new AnimationCurve
                (
                    new Keyframe(0, 1), // 가까울수록 힘 100%
                    new Keyframe(1, 0) // 멀수록 힘 0%
                );
            }
        }

        private void FixedUpdate()
        {
            Vector3 avoidanceForce = Vector3.zero;

            if (rb != null)
            {
                //360도 방향벡터를 생성하고 해당 방향벡터는 rayCount가 됨.
                Vector3[] directions = GenerateHorizontalDirections(rayCount);

                foreach (var dir in directions)
                {
                    avoidanceForce += AvoidWall(dir); // 각 방향의 회피 힘 누적
                }

                if (avoidanceForce != Vector3.zero)
                {
                    Debug.Log($"총 회피할 힘: {avoidanceForce}"); // 총 회피 힘 확인
                    rb.AddForce(avoidanceForce, ForceMode.Acceleration); // 누적된 회피 힘 적용
                }
            }
            else
            {
                Debug.LogError("WallAvoidance 스크립트의 Rigidbody가 없습니다.");
            }
        }

        public Vector3 AvoidWall(Vector3 direction)
        {
            Vector3 avoidanceObject = transform.position; // 레이 시작 위치
            RaycastHit hit;

            if (Physics.Raycast(avoidanceObject, direction, out hit, rayLength, castMask))
            {
                Debug.DrawLine(avoidanceObject, hit.point, Color.red, 0.1f); // 충돌 시 레이 표시

                // 벽과의 거리 계산
                float distanceToWall = hit.distance;

                // 거리에 따른 힘의 비율 계산 (애니메이션 커브 사용)
                float strengthFactor = avoidanceStrengthCurve.Evaluate(Mathf.Clamp01(distanceToWall / rayLength));

                // 벽의 법선 벡터를 따라 힘 계산
                Vector3 avoidanceDirection = hit.normal; // 벽 표면의 법선 벡터
                Vector3 force = avoidanceDirection * maxAcceleration * strengthFactor;

                Debug.Log($"Hit Normal: {hit.normal}, Avoidance Force: {force}");
                return force; // 계산된 힘 반환
            }
            else
            {
                Debug.DrawLine(avoidanceObject, avoidanceObject + direction * rayLength, Color.green, 0.1f); // 레이 표시
                return Vector3.zero; // 충돌이 없으면 힘 없음
            }
        }

        /// <summary>
        /// 수평 방향에 균일하게 분포된 방향 벡터를 생성
        /// </summary>
        private Vector3[] GenerateHorizontalDirections(int count)
        {
            Vector3[] directions = new Vector3[count];
            float angleStep = 360f / count; // 각 방향의 간격 (수평 360도 기준)

            for (int i = 0; i < count; i++)
            {
                float angle = i * angleStep; // 현재 각도
                float radians = Mathf.Deg2Rad * angle;

                // 수평 평면에서 방향 계산
                float x = Mathf.Cos(radians);
                float z = Mathf.Sin(radians);

                directions[i] = new Vector3(x, 0, z).normalized; // 수평 방향으로만 벡터 생성
            }

            return directions;
        }
    }
}



