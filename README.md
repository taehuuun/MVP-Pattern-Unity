# MVP-Pattern-Unity
- 해당 MVP 패턴은 실제 유니티 TCG 장르 팬게임 프로젝트에 유지보수와 코드 일관성을 목적으로 설계 및 적용 되었던 MVP 패턴 구조로 개선 된 버전 입니다.
- 많은 유니티 개발자들이 대부분 개념적으로 이해하고 있는 패턴이지만, 유니티 게임 프로젝트에 적용하기 어려웠던 부분을 개선 시키고자 하였습니다.

## 기술 스택
- ![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white) (2022.3.22f1)
- ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
## 설치 방법
- 레포지토리 클론
```
1. 해당 레포지토리를 클론.
2. 클론 된 폴더 내 MVP-Pattern-Unity 폴더를 복사
   (C# 파일만 필요시, MVP-Pattern-Unity/Scripts/Base 폴더 내 파일만 복사)
3. 사용이 필요한 프로젝트 내 붙여넣기
```
-  릴리즈 된 유니티 패키지 임포트
```
1. 레포지토리의 우측 릴리즈 탭 클릭
2. 원하는 릴리즈 버전의 유니티 패키지 파일 다운로드
3. 사용이 필요한 프로젝트에 해당 패키지를 임포트
```
## MVP 패턴
MVP 패턴은 간단하게 데이터 조작, UI(상호작용), 컨트롤 각 파트가 구분되어 있는 구조로 각 파트 별 역할에 집중 할 수 있다는 점이 장점인 패턴입니다.

### 클래스 다이어그램
- 현재 아래와 같은 구조로 MVP 패턴이 설계 되었습니다.

![MVP](https://github.com/taehuuun/MVP-Pattern-Unity/assets/43982651/f2e1cd1c-a694-4fad-83e6-dd2a332bb008)
### ModelBase
- 데이터를 관리하고 조작을 담당하는 추상 클래스 입니다.
- `Initailize()` 에서 실제 데이터를 로드 및 초기화 작업을 진행합니다.
- `TriggerEvent()`를 통해 모델의 업데이트를 `Presenter`에게 전달 합니다.
```csharp
/// <summary>
/// IModel 인터페이스를 상속 받는 모든 모델의 부모 추상 클래스
/// </summary>
public abstract class ModelBase : MonoBehaviour
{
    private readonly UnityEvent _onModelChanged = new();     // 모델 변경 시 발생하는 이벤트

    /// <summary>
    /// Model의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize()
    {
        
    }
    
    /// <summary>
    /// 모델 내 이벤트에 리스너를 추가하는 메서드
    /// </summary>
    /// <param name="listener">추가 할 리스너</param>
    public void AddListener(UnityAction listener)
    {
        _onModelChanged.AddListener(listener);
    }

    /// <summary>
    /// 모델 내 등록 된 이벤트 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="listener">제거 할 리스너</param>
    public void RemoveListener(UnityAction listener)
    {
        _onModelChanged.RemoveListener(listener);
    }

    /// <summary>
    /// 모델 내 등록 된 모든 리스너를 제거하는 메서드
    /// </summary>
    public void ClearListener()
    {
        _onModelChanged.RemoveAllListeners();
    }

    /// <summary>
    /// 모델 내 등록 된 이벤트를 발생 시키는 메서드
    /// </summary>
    public void TriggerEvent()
    {
        _onModelChanged?.Invoke();
    }
}
```
### ViewBase
- UI를 업데이트 하고 플레이어의 상호작용을 담당하는 추상 클래스 입니다.
- `Initailize()` 에서 UI요소의 이벤트 할당 및 초기화 작업을 진행합니다.
- `UpdateView()`를 통해 Model이 업데이트 될 때 마다 UI를 업데이트 합니다.
``` csharp
/// <summary>
/// IView 인터페이스를 상속 받는 모든 View의 부모 추상 클래스
/// </summary>
public abstract class ViewBase<TModel> : MonoBehaviour where TModel : ModelBase
{
    /// <summary>
    /// View의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize(PresenterBase<TModel> presenter)
    {
    }
    
    /// <summary>
    /// Model 변경 시 View를 업데이트 하는 메서드
    /// </summary>
    /// <param name="changedModel">변경 모델</param>
    public abstract void UpdateView(TModel changedModel);
}
```
### PresenterBase
- Model과 View를 컨트롤하는 추상 클래스 입니다.
- `Initailize()`: Model, View의 초기화 작업을 진행합니다.
- `AddListener()`: 이벤트의 리스너를 추가합니다.
- `RemoveListener()`: 이벤트의 리스너를 제거합니다.
- `TriggerEvent()`: 이벤트를 발생 시킵니다.
``` csharp
/// <summary>
/// IPresenter 인터페이스를 상속 받는 모든 Presenter의 부모 추상 클래스
/// </summary>
public abstract class PresenterBase<TModel> : MonoBehaviour where TModel : ModelBase 
{
    // View의 상호작용 이벤트를 관리하는 딕셔너리 필드
    public Dictionary<string, UnityEventBase> Events { get; }= new();
    
    [SerializeField] protected TModel model;                // ModelBase를 상속 받는 model 필드
    [SerializeField] protected ViewBase<TModel> view;       // ViewBase를 상속 받는 view 필드

    /// <summary>
    /// 최초 활성화 시 호출되는 메서드
    /// </summary>
    protected void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 오브젝트 파괴 시 호출되는 메서드
    /// </summary>
    protected virtual void OnDestroy()
    {
        model.ClearListener();
        RemoveAllEvent();
    }

    /// <summary>
    /// Presenter의 초기화를 진행하는 메서드
    /// </summary>
    protected virtual void Initialize()
    {
        model.Initialize();
        view.Initialize(this);
        
        model.AddListener(HandleModelUpdate);
        
        model.TriggerEvent();
    }

    /// <summary>
    /// eventName에 해당하는 이벤트에 리스너를 추가 시키는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">추가 할 리스너</param>
    protected virtual void AddListener(string eventName, UnityAction listener)
    {
        if (!Events.ContainsKey(eventName))
        {
            Events.Add(eventName, new UnityEvent());
        }

        ((UnityEvent)Events[eventName]).AddListener(listener);
    }

    /// <summary>
    /// eventName에 해당하는 이벤트에 인자를 1개 이상 가지는 리스너를 추가 시키는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">추가 할 리스너</param>
    protected virtual void AddListener(string eventName, UnityAction<object[]> listener)
    {
        if (!Events.ContainsKey(eventName))
        {
            Events.Add(eventName, new UnityEvent<object[]>());
        }

        ((UnityEvent<object[]>)Events[eventName]).AddListener(listener);
    }

    /// <summary>
    /// eventName에 해당하는 이벤트의 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">제거 할 리스너</param>
    protected virtual void RemoveListener(string eventName, UnityAction listener)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent unityEvent)
            {
                unityEvent.RemoveListener(listener);
            }
        }
    }

    /// <summary>
    /// eventName에 해당하는 이벤트의 인자를 1개 이상 가지는 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">제거 할 리스너</param>
    protected virtual void RemoveListener(string eventName, UnityAction<object[]> listener)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent<object[]> unityEvent)
            {
                unityEvent.RemoveListener(listener);
            }
        }
    }

    /// <summary>
    /// eventName에 해당하는 이벤트를 발생 시키는 메서드
    /// </summary>
    /// <param name="eventName">발생 시킬 이벤트 명</param>
    public virtual void TriggerEvent(string eventName)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent unityEvent)
            {
                unityEvent.Invoke();
            }
        }
    }
    
    /// <summary>
    /// eventName에 해당하는 인자를 1개 이상 가지는 이벤트를 트리거 시키는 메서드
    /// </summary>
    /// <param name="eventName">발생 시킬 이벤트 명</param>
    /// <param name="args">이벤트 발생 시 전달되는 인자 배열</param>
    public virtual void TriggerEvent(string eventName, object[] args)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent<object[]> unityEvent)
            {
                unityEvent.Invoke(args);
            }
        }
    }
    
    /// <summary>
    /// eventName에 해당하는 이벤트를 제거하는 메서드
    /// </summary>
    /// <param name="eventName">제거 할 이벤트 명</param>
    protected virtual void RemoveEvent(string eventName)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            unityEventBase.RemoveAllListeners();
        }
        
        Events.Remove(eventName);
    }

    /// <summary>
    /// _events 딕셔너리의 모든 이벤트 및 리스너를 제거하는 메서드
    /// </summary>
    protected virtual void RemoveAllEvent()
    {
        foreach (var events in Events.Values)
        {
            events.RemoveAllListeners();
        }

        Events.Clear();
    }

    /// <summary>
    /// View를 활성화 시키는 메서드
    /// </summary>
    public virtual void ShowView()
    {
        view.gameObject.SetActive(true);
    }

    /// <summary>
    /// View를 비활성화 시키는 메서드
    /// </summary>
    public virtual void HideView()
    {
        view.gameObject.SetActive(false);
    }

    /// <summary>
    /// 모델이 변경될 때 호출 되는 메서드
    /// </summary>
    protected virtual void HandleModelUpdate()
    {
        view.UpdateView(model);
    }
}
```
## 샘플 (클리커 게임)
### 샘플 코드
  - [ClickerModel](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/Assets/MVP-Pattern-Unity/Scripts/Samples/Clicker/ClickerModel.cs)
  - [ClickerView](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/Assets/MVP-Pattern-Unity/Scripts/Samples/Clicker/ClickerView.cs)
  - [ClickerPresenter](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/Assets/MVP-Pattern-Unity/Scripts/Samples/Clicker/ClickerPresenter.cs)

### 플레이 영상

![MVP_Sample_Video](https://github.com/taehuuun/MVP-Pattern-Unity/assets/43982651/c136acc5-3f32-4ba3-aa45-15a08374b77f)

## 라이선스
이 프로젝트는 MIT 라이선스를 따릅니다. 자세한 내용은 [LICENSE](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/LICENSE) 파일을 참조하세요.
## 연락처 정보
- TISTORY 블로그: <https://devvdevv.tistory.com/>
- 이메일: <lsw463@naver.com>
