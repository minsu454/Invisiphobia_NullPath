# 2024 스파르타 Unity track 6기 최종프로젝트

## ![image](https://github.com/user-attachments/assets/66344946-0ac7-4618-bc6b-469e3908c9cd)
# Invisiphobia : Null Path

---
## 🎥 시연 영상

https://youtu.be/LLINUvYx3uk
![화면 캡처 2025-01-21 155657](https://github.com/user-attachments/assets/6968d4f5-6d37-4c02-a498-28819f448d07)

---

## 기술 스택

- Unity 버전: [2022.3.17f1]
- C# 버전: [C#-17]
- 발표 자료 & 프로젝트 기술 소개: [Figma](https://www.figma.com/slides/867H91N1ZlACgRbVFskaeb/15%EC%A1%B0-%EC%A4%91%EA%B0%84%EB%B0%9C%ED%91%9C%EC%9E%90%EB%A3%8C?node-id=91-32&t=KDSpzmPqs5J08Pfg-0)

---

### 😱 게임 소개

Invisiphobia : null path 는 보이지 않는 물체와 그 사이에 있는 괴물들로 가득 찬 미궁에서 펼쳐지는 **3D 1인칭 호러 어드벤처 게임**입니다.            

**소중한 사람을 구출**하기 위해 당신은 보이지 않는 존재들을 조심스럽게 **탐색**하고, 갑작스럽게 나타나는 위협을 경계하며 살아남고, 마지막엔 구출 대상을 구해 당신이 들어왔던 지하실 입구로 되돌아 나가야 합니다.



https://github.com/user-attachments/assets/1290d155-72d3-4704-9994-f09c758bff35


https://github.com/user-attachments/assets/44d088ca-48ff-4044-8ecd-e6acc264a2a2


https://github.com/user-attachments/assets/4e76ebb0-85ca-4c76-b4d8-40ac4adcd805


https://github.com/user-attachments/assets/e4f85635-eefe-4165-b7d5-1c793623954b


https://github.com/user-attachments/assets/1850bafc-bfa5-440c-b035-f6040885650e




### 🔖 가이드
    
- 게임 가이드
    
    **태블릿 1번 탭** : 물체 감지
    
    **태블릿 2번 탭** : 사진기 - 몬스터를 공격하는 용도
    
    **태블릿 3번 탭** : 해킹 - 문 옆의 차단기를 상호작용하여 퍼즐을 푸는 용도)
    
    **붉은 문** : 벽돌을 던져 버튼에 상호작용하면 열리는 문
    
    **잠긴 문** : 열쇠로 상호작용하여 여는 문
    
    **초록 문** : 옆의 차단기에 상호작용하여 퍼즐을 풀면 열리는 문


## 조작법
| 키           | 동작            |
|--------------|----------------|
| W            | 앞으로 이동     |
| A            | 왼쪽으로 이동   |
| S            | 뒤로 이동       |
| D            | 오른쪽으로 이동  |
| Shift        | 달리기          |
| ctrl         | 앉기            |
| E            | 아이템 상호작용  |
| Tab          | 태블릿 사용      |
| 1, 2         | 태블릿 기능 전환 |
| 우클릭        | 화면 줌         |
| 우클릭 + 좌클릭| 아이템 사용     |

---

<aside>
📖
  
# 기획
</aside>

### ★ 메인 아이디어

- **리미널 스페이스**
  
![image](https://github.com/user-attachments/assets/8382dcb0-4c39-4dc6-abfa-fd011bfe8324)
![image](https://github.com/user-attachments/assets/a31d973b-b522-4dd8-b463-321016485843)


‘리미널 스페이스’는 현대 공포 게임에서 자주 활용되는 미학으로, **익숙하면서도 낯선 분위기**를 통해 플레이어에게 불확실성, 괴리감, 그리고 불안감을 조성합니다. 
    
이러한 불안감은 **‘공간의 형성과 경험 사이의 경계’를 흐릿하게** 만드는 데서 비롯되며, 이를 구현하기 위해 **현실적 요소와 비현실적 요소가 공존**해야 합니다.
    

    
2019년에 처음 인터넷에 등장한 ‘Backroom’ 괴담
![image](https://github.com/user-attachments/assets/d600c0ff-ad27-46cb-92c7-1d92f01e8db3)

대표적인 사례로 2020년대에 유행하기 시작한 ‘Backroom’이 있습니다. 이 개념은 친숙한 공간 요소를 비현실적으로 (무한정)반복 배치함으로써 불안감을 효과적으로 전달합니다. 특히, 이러한 접근법은 비디오 게임 제작에서 **적은 리소스로도 넓은 공간을 표현할 수 있는 당위성**을 제공하여 관련 게임들이 다수 등장하고 성공 사례도 많이 나타나고 있습니다.
    
- **화면 전환 점프스케어 유도**
    
![image](https://github.com/user-attachments/assets/faaf0c1a-50c3-4e05-85d8-c30a6480c030)
![image](https://github.com/user-attachments/assets/45970918-405a-4c9c-9b3d-678d2b0ee96c)


    
‘화면 전환 점프스케어 유도’의 핵심은, 플레이어가 **스스로 시야를 전환하거나 화면을 이동하는 과정에서 공포 상황을 발견**하게 함으로써 강제적인 공포 유발을 줄이고, 대신 능동적인
조작을 통해 긴장감과 몰입도를 높이려는 데 있습니다.
    
전통적인 ‘갑작스러운 점프스케어’와 달리 플레이어가 **자발적으로 공포 대상에 부딪히기 때문**에 거부감이 줄어들고, 게임플레이와 공포가 자연스럽게 결합됩니다. 이러한 방식은 Five Nights at Freddy's나 I'm on Observation Duty 같은 예시에서 잘 드러나는데, **직접 화면을 옮겨 위협 요소를 확인**하는 과정에서 공포와 긴장감이 한층 더 생생하게 전달됩니다.
    

---

- **두 요소의 조합**
    
![image](https://github.com/user-attachments/assets/db09113f-9f23-4c53-9598-a57da592f1d0)


비어있는 공간에서 물체를 감지


![image](https://github.com/user-attachments/assets/4a0ccc50-56e3-468c-8afd-55c203c1ecf7)


타블렛을 들여다 봐 밝혀내고


![image](https://github.com/user-attachments/assets/2d15f8a2-a223-497f-afe9-94879ca76cd2)


타블렛을 내리면 그대로 물체들이 세상에 드러난다

    
    동적인 공간에서 화면이 전환되어야 하기 때문에 휴대용 기기 화면을 들여다보다 내리는 상황을 만들어서 해당 경험을 게임플레이에 직접적으로 연관시키려 했습니다.

---

### 🎮게임플레이

- **걷기 시뮬레이션?**
    
    위에 ‘화면 전환 점프스케어 유도’ 에서 예시를 든  정적인 환경의 공포게임을 제외하면  거의 모든 공포게임은 걷기 시뮬레이션이다.
    
    이런 단순한 게임플레이에 몰입시키기 위해선 흥미로운 공간 배치가 필수적이다.
    
![image](https://github.com/user-attachments/assets/1c942b57-28cc-4cdf-b757-8a10c9caaba8)
![image](https://github.com/user-attachments/assets/e54d5079-bc89-4231-9bd3-3633e2ef3758)
![image](https://github.com/user-attachments/assets/6197fa45-c77a-4a5b-945f-42d79ae73364)


    
    위에서 설명한 ‘리미널 스페이스’의 느낌을 유지하되 어딜 가더라도 새로운 공간인 것처럼 하기 위해 노력했다.
    
- **같은공간 다른느낌**
    
    본 게임에선 2D맵(타블렛 화면)과 에 의존하며 3D 구조를 탐험하게 되는데, 지도를 펼치면서 길을 가다가 지도가 사라지면 전혀 다른공간에 와있는것같은 느낌을 주기 위해 클라이막스에서 구출대상을 업고 갈땐 타블렛을 들 손이 없어 지도 없이 출발지를 찾아가도록 하는 시퀸스를 만들었다.

  # 핵심 기능

---
<details>
<aside>

<Summary>
    
🔥**생명주기**

</Summary>

<aside>


### 생명주기 직접관리 - 초기화 관리

</aside>

초기화하는 객체의 모든 것들을 직접 관리할 수 있도록 하였습니다. (순서로 인한 충돌 방지)

- 1. Managers.Init
    - Managers
        
        : 보통 Core에 초기화가 필요할 때 사용되고 DontDestroyOnLoad로 파괴가 되지 말아야 할 객체들도 들고다니는 정적 객체
        
        - **Awake보다 먼저 실행**되는 것이 보장되어 해당 장소에서 **모든 Core단에 초기화**가 이루어지도록 하였습니다.
        - [**RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)**]을 사용해 모든 스크립트에서 제일 먼저 초기화해줄 객체들을 Managers에서 초기화하였습니다.
        ![image](https://github.com/user-attachments/assets/ab300d24-bf09-4320-8e14-0b8ce50f818b)


        

2. Awake

- 3. SceneLoaded
    - **SceneJobLoader**
        - 씬이 로딩이 되었을 때 실행되는 SceneManager.sceneLoaded에 초기화 순서가 보장되어있지 않기에 발생하는 NullReference를 잡기 위해 사용하였습니다.
        - 구현
            
            초기화 시켜줄 함수들을 우선순위에 맞춰 관리해주는 클래스 SortedList의 키값으로 enum을 들고 있어 내가 우선순위를 직접 관리할 수 있도록 구현
           ![image](https://github.com/user-attachments/assets/b1d6868c-831b-4eee-91ed-eb85ff3b9d48)
 

            
        - SceneLoader
            - 사용 이유 : 씬이 로딩되었을 때 실행하는 모든 초기화를 직접 관리하기 위함.
            - 구현 : 해당 씬을 호출할 때 초기화될 GameObject들을 관리하고 싱글톤 패턴을 사용해 동적으로 생성되는 객체도 접근이 편하도록 설계
            
![image](https://github.com/user-attachments/assets/aac7bdeb-64a6-413f-892f-041408f0d41b)

            

4. Start

</aside>
<aside>
</details>

<details>

<Summary>

🏘️**MapEditor**

</Summary>
<aside>



### **MapEditor로 맵 작업 및 맵 데이터 관리**

</aside>

편의성을 위해 해당 오브젝트들에 직관적으로 이미지와 설명 프리뷰가 포함된 MapEditor를 제작

- 사용 이유
    - 맵을 찍을 때 일일히 프리팹을 보고 가져오고 하는 불편함
    - 맵 자체에 **json파일**로 저장할 수 있는 기능이 필요할 것이라고 예상
    - 장점
        - Unity에 기본 기능을 사용함과 동시에 Inspector 창을 공유할 필요 없이 독립적으로 작업을 진행 가능
        - 멀티창을 지원하기에 나중에 다른 기능과 병렬 작업을 해도 유용
            
![image](https://github.com/user-attachments/assets/c027549e-2d21-4fc4-b56f-df6d2d8c84d6)

            
- Parts
    - Room : 방 벽과 천장, 바닥으로 이루어진 공간 오브젝트
        
![image](https://github.com/user-attachments/assets/1f2f385e-3466-4a7e-a0e1-b92f3d49f638)

        
    - Deco : 방을 꾸며주는 기본 오브젝트 (파이프, 일반 전등 등)
        
![image](https://github.com/user-attachments/assets/f0ecc8ac-5d8e-44b2-9a1d-aa4293cff636)

        
    - Item
        - Prop : 맵 아이콘을 가지고 있고 감지할 수 있는 가장 기본적인 객체 (상자, 드럼통 등)
            
![image](https://github.com/user-attachments/assets/10ff6131-7c87-4e51-8dee-a12eb21b0a7a)

            
        - BaseItem : 상호작용이 되는 객체
            
![image](https://github.com/user-attachments/assets/4768fd13-12d4-4bc6-a0c8-a3ee57f84151)

            
        - HandItem : 손에 들 수 있는 객체
            
![image](https://github.com/user-attachments/assets/98ba4e29-b6bc-4067-a3eb-b286f5f2cf79)

            
    - Event : Item과 유사하나 특정 조건을 만족했을 때 한 번만 실행되는 interact를 지닌 객체
        
![image](https://github.com/user-attachments/assets/5aff7f65-fe16-45b3-afa6-7dc1381e154c)

        
</aside>

</details>

<details>
<aside>

<Summary>

♻️**ObjectPool**

</Summary>
<aside>



### **ObjectPool을 사용한 메모리 관리 및 최적화**

</aside>

- 사용 이유
    
    ObjectPool로 메모리를 재사용하여 메모리 할당 및 해제에 따른 성능 최적화
    
    우리가 오브젝트를 사용할 땐 GameObject에서 사용하는 것이 아닌 해당 클래스를 GetCompnent를 사용하여야 되는데 Unity에서 **무거운 GetCompnent의 연산을 줄이기 위해 제너릭을 사용하여 최적화**
    
- 구현
    - **ObjectPool<T>** 사용
    - 해당 오브젝트가 어떤 Pool로 반환해줘야 되는지 **IObjectPoolable 인터페이스**를 통해 **ReturnEvent를 강제**하고 해당 ObjectPool로 **다시 반환**될 수 있게 설정
    
![image](https://github.com/user-attachments/assets/dbc7b8f8-b6d2-498e-b499-685dd1f0663f)

    
</aside>

</details>

<details>
<aside>

<Summary>

📁**Loading**

</Summary>

<aside>



### 비동기로 씬 전환 관리

</aside>

- 사용 이유
    
    씬 전환 시 다음 씬에 동적으로 데이터를 가져오고 다음 씬 준비 시간을 주어 사용자가 씬 전환 시 프레임 드랍되는 문제를 해결하기 위해 사용
    
- 구현
    - 다음 씬이 로드 준비되는 시간을 벌기 위해 씬 로드를 비동기로 호출
    - ObjectManager가 해당 로드 준비 시간을 벌 때 다음 씬에 사용할 오브젝트들을 메모리에 올리는 작업
    - 객체 생성시 발생하는 메모리를 아끼기 위해 Coroutine(class)대신 UniTask(struct)를 사용
    - 페이크로딩을 사용해 씬이 자연스럽게 넘어가도록 구성
    
![image](https://github.com/user-attachments/assets/f6e27c62-b858-4364-956c-0c3bd574b7a9)

    
    - Addressable
        - Resources에 문제점인 빌드할 때 모든 데이터를 들고 빌드되는 것(크기가 커짐)과 GameObject에 간단한 수정 시 재빌드해야하는 것, 런타임시 모든 메모리를 로드하는 것으로 메모리 낭비가 발생하는 문제 등으로 인해 Addressable을 사용
        
![image](https://github.com/user-attachments/assets/3703ee7f-c2e5-4211-8ba3-7807be670316)

        
    - ObjectManager
        - InGame에서 비동기 처리로 인해 발생할 수 있는 문제를 없애기 위해 해당 씬에 나오는 모든 아이템 데이터를 메모리에 올려놓고 실질적으로 게임에선 비동기함수 호출이 안되도록 하기 위한 클래스
        - 해당 씬이 동작할 때 필요한 모든 오브젝트를 Dictionary에 담고, 필요 시에 해당 오브젝트를 Return
        
![image](https://github.com/user-attachments/assets/09e05d1d-403f-4b77-bd12-b119b66f10b5)

        
</aside>

</details>

<details>
<aside>

<Summary>

📱**Tablet**

</Summary>



<aside>


### 여러 디자인 패턴을 사용한 Tablet 기능

</aside>

- **Strategy 패턴**: 동작을 동적으로 교체할 수 있는 구조
    - 구현
        - 내가 태블릿을 전환할 때마다 그에 해당하는 모든 이벤트들을 Subscribe로 연결
        
![image](https://github.com/user-attachments/assets/075542c3-ba36-4e1a-9088-1a994da0fc71)

        
- **Observer 패턴**: 이벤트를 사용하여 상태 변화와 UI 동작을 연결 및 결합도 감소
    - 구현
        - 상태변환할 때 구독한 모든 이벤트에게 상태를 전달
        
![image](https://github.com/user-attachments/assets/1473e895-9739-4965-aee6-deeee414f952)

        
- **State 패턴**: 상태별 동작 정의와 상태 전환 관리
    - 구현
        - 상태를 변경할 때마다 해당 상태에 맞는 상호작용을 하도록 구현
        
![image](https://github.com/user-attachments/assets/3ce1e99a-3065-4d2e-a0eb-77aaeb148b99)

        

</aside>

</details>

<details>
<aside>

<summary>

📗**Excel To Json**

</summary>

<aside>


### 엑셀을 통한 데이터 관리

</aside>

- DataService를 정적으로 배치하여 json 파일을 꺼내 쓸 수 있도록 설계
    - DataService
        
![image](https://github.com/user-attachments/assets/cb29a707-f743-48e6-aa20-4ac59e220c14)

        
- Excel을 통해 데이터 관리를 하고 json파일로 변환하여 유지보수성과 확장성을 높이고 직관적으로 데이터를 볼 수 있도록 하였습니다.
    - ItemTable.xlsx
        
![image](https://github.com/user-attachments/assets/b6cff574-1ec4-4e84-beb4-3ca98074e071)

        
- 이를 통해 인게임에서 사용되는 텍스트의 언어를 자유롭게 변경하고 확장 가능하도록 하였습니다.
    - InteractTextTable.xlsx
        
![image](https://github.com/user-attachments/assets/e3f4a22e-def5-4e31-8e9f-6839dfda424d)

        
</aside>
</details>

<aside>
    
<details>

<summary>

⏸️**Pause**

</summary>

<aside>



### 게임 pause 관리

</aside>

- EventManager를 동적으로 배치해 외부에서 간단하게 모든 객체에 움직임을 컨트롤 할 수 있게 설계
    - GameEventType 및 이벤트 구독
        
![image](https://github.com/user-attachments/assets/0bc0ed62-fd2a-44b3-90eb-df5c92618515)

        
- UI에 애니메이션 삽입 가능
</aside>
</details>

# 유저테스트

### 🫨 여러분의 평가

- 호평
    1. **스캔(TAB) 시스템의 참신함**
        - TAB 키를 눌러야만 눈에 보이지 않던 사물이 나타난다는 점이 “공포 연출”에 큰 역할을 함.
        - 플레이어가 직접 숨겨진 요소를 찾아내야 하므로 긴장감이 유지되고, 어디서 무언가가 튀어나올지 예측하기 어려워 스릴을 높여 줌.
      
          
    2. **분위기(그래픽·사운드·UI)의 완성도**
        - “어둡고 음산한 맵+사운드”가 잘 어우러져서 공포감을 제대로 전달한다는 반응이 많았음.
        - 메뉴 화면, BGM, 효과음, 마네킹이나 크리처의 등장 효과 등 전체적으로 몰입도가 높았다는 평가.
      
          
    3. **아이디어·콘셉트의 우수성**
        - 인디 호러게임 트렌드를 잘 반영했고, ‘스캔’이라는 소재를 접목해 게임적으로 차별화를 시도한 점이 인상 깊다는 의견.
      
          
    4. **조작 난이도 자체는 어렵지 않음**
        - “조작법이 복잡하지 않아 금방 적응할 수 있다”는 피드백도 일부 있었음.
        - 일단 움직임이나 마우스 조작 자체는 크게 어렵지 않아 호러 장르를 즐기지 않는 사람도 쉽게 진입 가능.
      
          
    5. **공포 연출과 점프 스케어(깜놀 요소)**
        - 마네킹·괴물 등의 시각적 연출과 ‘갑툭튀’가 제대로 무섭다는 평.
        - TAB 스캔 후 바뀐 화면에서 들려오는 사운드나 비주얼 이펙트가 게임의 분위기를 배가시킴.
          
    
    전체적으로 **공포 연출**과 **핵심 메커니즘**에 대한 호평이 있었다.
    
- 혹평
    - **가이드·튜토리얼 부족**
        - 게임 시작 후 목표나 진행 방향, 아이템 사용법 등을 알려주지 않아 “무엇을 해야 할지 모르겠다”는 의견이 가장 많음.
        - 조작키(벽돌 던지기, 아이템 사용 등)에 대한 설명이 부족하고, 플레이 흐름(맵 공략 방식·퍼즐 해법)이 명확하지 않아 초반 이탈 가능성 존재.
     
          
    - **맵이 너무 어둡거나, 구조가 복잡한 데 비해 빈약함**
        - “조금만 더 밝아졌으면 좋겠다”거나, “감마(밝기) 조절이 제대로 안 된다”라는 피드백이 다수.
        - 일부 플레이어들은 맵 자체가 커 보이나 반복되는 구조로 인해 길 찾기가 어렵고, 맵 내부에 상호작용 요소가 부족하다고 느낌.
     
          
    - **목적과 스토리 부족**
        - “게임 안에서 서사 혹은 스토리 동기가 부족하다”라는 반응.
        - 게임 내 스토리(예: ‘왜 여기에 갇혔는지?’, ‘어떤 목표가 있는지?’)가 없어서 진행 이유를 찾지 못하고 흥미가 떨어진다는 지적.
     
          
    - **아이템 사용·퍼즐 진행의 불명확성**
        - 건전지, 벽돌 등 파밍한 아이템의 쓰임새가 잘 설명되지 않음.
        - 벽돌을 던져야 문이 열리는 기믹이나, 업데이트 칩을 이용해서 다음 구역을 열어야 하는 등 로직이 직관적이지 않아 막히는 경우가 많았음.
     
          
    - **버그와 기술적 이슈**
        - 괴물이 특정 지형에 낑겨서 움직이지 않는 현상.
        - 벽돌 투척 시 마우스 에임 방향과 다르게 발사되는 문제.
        - 스캔이 먹히지 않는 오브젝트, 스캔 게이지가 안 차거나 UI가 해상도에 맞게 표시되지 않는 등 각종 버그 제보.
     
          
    - **세이브·체크포인트 미비**
        - 죽으면 처음부터 다시 해야 해서, 공포감보다는 귀찮음이 커진다는 의견.
        - 호러게임 특성상 반복 시 긴장감이 떨어지므로 중간 세이브 포인트/체크포인트가 필요하다고 주장.
    

### 🙇‍♂️ 개선 방안

- 테스트 기간 동안 반영된 부분
    - **세이브·체크포인트 :** 게임을 저장할수 있는 오브젝트가 배치되었다.
 
      
    - **약간의 가이드 :** 맵상으로 문이 열렸는지 닫혔는지 확인할수 있거나, 좀더 직관적으로 배치하는등으로 개선이 이루어졌다.
 
      
    - **상호작용 택스트** : 일부나마 상호작용 실패시 택스트를 출력시켜 의도된 사항임을 알리려 했다.
 
      
- 앞으로 고쳐나갈 부분
    - **코드 최적화** : ****아직 코드적으로 최적화 할수 있는 코드들이 많이 남아있다.
 
      
    - **상호작용 이팩트** : 상호작용 + 상호작용 실패시 적절한 효과음과 택스트가 나타나 의도된 사항임을 알리고, 상호작용으로 무엇이 일어났는지 플레이어에게 전달해야한다.
 
      
    - **튜토리얼/가이드** : 게임 초반에 튜토리얼 세션을 만들어 해당 이벤트가 최초로 발생했을때 튜토리얼 팝업과 UI강조 등을 통해 핵심 기능을 안내하는 기능이 필요함.
 
      
    - **스토리 전달** : 왜 이곳에 떨어졌는지, 뭘 해야 하는지에 대한 서사 전달이 필요하다. 미로에 들어오기 전의 인트로 씬이 있어야 하지만 아직은 생략된 상태.
 
      
    - **괴물 AI 개선** : 현재는 괴물이 시야각에 들어와야만 추적을 시작하고 범위를 벗어나면 바로 추적이 멈추는 간단한 형태이기 때문에 입체적인 공포게임을 위해 좀더 조정할 필요가 있다.
 
      
    - **탐지 기능의 당위성 부여 :** 기본적으로 배터리, 열쇠, 벽돌 등을 찾기 위해 탐지를 진행해야 한다는 기획이였으나, 탐지 가능한 대부분이 잡동사니이고 그중에 괴물이 껴있어 그냥 무시하더라도 별다른 패널티가 없다. 괴물의 경우 무시하면 더 무섭게 쫓아온다거나 꾸준히 아이템을 더 찾아야하는 이유를 부여할 필요가 있다.
    

---

<aside>
🤯

# 트러블슈팅

</aside>

<aside>

### 1. 프레임 저하로 인한 게임 진행 불가

- 문제 상황
    
    <aside>
    
    프레임이 심하게 저하되어 플레이에 지장이 갈 정도의 성능을 보였습니다.
    
    </aside>
    
- 해결 방안
    
    <aside>
    
    에셋의 문제와 카메라를 5개 사용하고 있어 프레임이 나오지 않았습니다.
    
    프레임을 올리기 위해 에셋들의 텍스처 사이즈를 4090에서 512로 줄이고, light를 realtime이 아닌 베이킹으로 바꿔주었습니다. 또한, 맵에디터에서 Room이 중복 저장되는 문제를 수정하고, 카메라가 찍는 범위와 오클루전 컬링을 사용하여 카메라 최적화에 신경썼습니다.
    
    </aside>
    

### 2. 빌드 후 비동기 로딩 실행 시황

- 문제 상황
    
    <aside>
    
    빌드 후 실행 시 Loading바가 시간이 지나도 차오르지 않는 문제가 있었습니다.
    
    </aside>
    
- 해결 방안
    
    <aside>
    
    비동기 순서 문제로 처리 순서를 바꿔주어 해결하였습니다.
    
    - ObjectManager.Add가 메인 스레드가 아닌 별도의 스레드에서 실행될 수 있으므로, Unity의 AsyncOperation과 상호작용할 때 메인 스레드에서 작업이 안전하게 처리되지 않으면 문제가 생길 수 있습니다.
    - ObjectManager.Add가 완료되기 전에 op.allowSceneActivation이나 op.progress를 확인하면 예기치 않은 동작이 발생할 수 있습니다.
    </aside>
    

### 3. 길 안내 요원이 꺼질 때도 나오는 등장 사운드

- 문제 상황
    
    <aside>
    
    길을 안내해주는 객체가 감지가 되었을 때 나오는 사운드가 객체가 꺼지고 자리로 돌아왔을 때 한 번 더 재생되는 문제가 있었습니다.
    
    </aside>
    
- 해결 방안
    
    <aside>
    
    action에 연결된 우선순위 문제
    
    action에 컨트롤러, 사운드 순서로 연결을 해주어 상태 변경이 되는 순간에 컨트롤러, 사운드 순서로 실행됩니다. 컨트롤러에서도 상태를 변경해 주는 부분이 있어  a상태에서 b상태가 될 때에 컨트롤러에서 상태를 변경해주고 사운드에서는 컨트롤러에서 상태가 변경되기 전 상태의 사운드를 재생하게 되어 문제 발생하였습니다.
    
    사운드를 먼저 실행시켜주도록 action에 연결해준 순서에서  사운드를 위로 올리고 컨트롤러를 아래로 내려 사운드 재생을 하고 컨트롤러에서 상태가 변경될 수 있도록 하였습니다.
    
    </aside>
    

### 4. bool변수 중복 사용

- 문제 상황
    
    <aside>
    
    스태미나 전부 소진 후 의도와는 다르게 바로 스태미나 사용이 가능하여 계속해서 달릴 수 있는 문제가 있었습니다.
    
    </aside>
    
- 해결 방안
    
    <aside>
    
    Action에 연결된 함수가 Update에서 실행되는 상황에 앉기 함수인  isCrouched의 조건문에서 웅크리지 않은 상태에서는 항상 enableSprint가 Update에서 true로 바꿔주게 되어 계속 코루틴이 실행되는 문제
    
    달리는 중이라면 웅크리기 버튼을 눌렀을 때 Return을 하여 앉기를 실행하지 못하는 방법으로 enableSprint가 계속 true로 바뀌지 않게 코드를 수정해주었습니다.
    
    또한 enableSprint를 다른 외부 함수에서 강제 설정을 해주고 있어 그 부분을 지워 해결하였습니다.
    
    </aside>
    
</aside>

## 참여 인원

팀원들의 GitHub Link 입니다.

[리더]

[강민수](https://github.com/minsu454)

[부리더]

[김찬](https://github.com/moloch-kim)

[팀원]

[김나영](https://github.com/keubung?tab=repositories)

[이호영](https://github.com/leecoading)

