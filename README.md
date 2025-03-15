# MVP-Pattern-Unity
- 해당 프로젝트는 실제 유니티 TCG 장르 팬게임 프로젝트에 유지보수와 코드 일관성을 목적으로 설계 및 적용 되었던 MVP 패턴의 개선 된 버전 입니다.
- 많은 유니티 개발자들이 대부분 개념적으로 이해하고 있는 패턴이지만, 유니티 게임 프로젝트에 적용하기 어려웠던 부분을 개선 시키고자 하였습니다.

## 기술 스택
- ![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white) (6000.0.32f1)
- ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
## 설치 방법

### 레포지토리 복사
1. Git에서 해당 레포지토리를 클론:
   ```bash
   git clone <레포지토리 URL>
   ```
2. 프로젝트 내 Scripts/Base 폴더 또는 필요한 MVP 관련 파일을 복사하여 붙여넣기.

### 유니티 패키지 사용
1. 릴리즈 탭에서 최신 **유니티 패키지(Unity Package)** 다운로드.
2. Unity 프로젝트에서 `Assets > Import Package > Custom Package`로 가져오기.

---

MVP는 Model-View-Presenter의 약자로, **UI(User Interface)**와 **비즈니스 로직**을 분리하여 유지보수 및 테스트를 용이하게 만드는 디자인 패턴입니다.

### 구조 소개
- **Model**: 데이터, 비즈니스 로직 및 해당 작업을 처리합니다.
- **View**: UI와 사용자 간 상호작용을 담당하며, UI 변경을 Presenter에 알립니다.
- **Presenter**: View와 Model 간의 중개자로 역할을 하며, 로직이 들어 있습니다.

```plaintext
1. Presenter 초기화 -> Model, View 초기화 -> 데이터 초기화 -> [View (최초 업데이트)] 
2. [View] → 사용자 입력 및 이벤트 → [Presenter]
3. [Presenter] → 비즈니스 로직 호출 → [Model]
4. [Model] → 데이터 변경 및 알림 → [Presenter] → [View]
```

![MVP](https://github.com/user-attachments/assets/5b1dd783-a5e8-4357-825d-b8effc81b540)

## 2. Base 클래스 설명

### 🔗 ModelBase
`ModelBase`는 모든 **Model** 클래스의 부모입니다.</br>데이터 초기화 및 데이터 변경 감지 등을 처리하며 게임의 비즈니스 로직의 핵심을 구성합니다.

#### 핵심 메서드:
- `Initialize`: 해당 Model과 연결될 Presenter를 초기화 하는 메서드
- `InitializeModelMethods`: 초기화 시 Presenter를 통해 호출 할 메서드를 AddMethod를 통해 등록 시키는 메서드
- `InitializeNestedProperties`: Model 내 참조 타입의 프로퍼티 변경 감지를 위해 이벤트를 등록하는 메서드
- `InitializeProperties`: Model 내 프로퍼티를 초기화 하기 위한 메서드
- `InitializeInvokeMethod`: 모든 초기화 완료 시 최초 실행되는 메서드
- `AddMethod`: 메서드 중 Presenter를 통해 외부에서 호울 할 메소드를 등록하기 위한 메서드
- `RemoveMethod`: 등록된 메서드를 제거하는 메서드 
- `GetProperty`: 프로퍼티 명을 통해 등록된 프로퍼티의 값을 반환하는 메서드
- `SetProperty`: 프로퍼티 명에 해당하는 프로퍼티의 값 변경 및 프로퍼티 변경 이벤트를 발생 시키는 메서드
- `OnNestedPropertyChanged`: Model 내 참조 타입의 프로퍼티 값 변경 시 호출되는 메서드
- `OnPropertyChanged`: 프로퍼티 변경 시 호출되는 메서드

```csharp
public abstract class ModelBase : MonoBehaviour, INotifyPropertyChanged
{
    ...
    public void Initialize(PresenterBase) { }
    public virtual void InitializeModelMethods() { }
    protected virtual void InitializeNestedProperties() { }
    protected virtual void InitializeProperties() { }
    protected virtual void InitializeInvokeMethod() { }
    protected void AddMethod(Enum, Delegate) { }
    protected void RemoveMethod(Enum) { }
    public T GetProperty<T>([CallerMemberName]string = null) { }
    protected void SetProperty<T>(T, [CallerMemberName] string = null) { }
    protected virtual void OnNestedPropertyChanged(object, PropertyChangedEventArgs) { }
    private void OnPropertyChanged(string) { }
}
```
### 🔗 ViewBase
`ViewBase`는 모든 **View** 클래스의 부모클래스 입니다

UI 컴포넌트(버튼, 텍스트 등)를 바인딩 및 초기화하고 사용자 상호작용을 담당합니다.


#### 주요 역할:
- UI 요소 초기화 및 바인딩
- Model의 변경 사항을 UI에 반영

#### 핵심 메서드:
- `Initialize`: 해당 View와 연결 될 Presenter를 초기화 하는 메서드
- `InitializeViewMethod`: View 내 기본 메서드 중 Presenter를 통해 호출되는 메서드를 등록하는 메서드
- `InitializeBindComponent` : UI요소 및 오브젝트들을 바인딩을 하기 위한 메서드
- `InitializeEventListeners`: 버튼, 슬라이더 등 UI 이벤트 리스너 추가를 위한 메서드
- `SetupView`: 초기화 완료 후 UI를 최초 업데이트 하는 메서드
- `UpdateView`: Model 내 변경된 프로퍼티에 맞게 UI를 업데이트 하는 메서드
- `ShowView`: View를 활성화하는 메서드
- `HideView`: View를 비활성화하는 메서드
- `InvokeMethod`: Presenter의 등록되어있는 메서드를 호출하는 메서드
- `GetProperty`: Presenter를 통해 해당하는 프로퍼티 값을 가져오는 메서드
- `Bind`: UI요소를 바인딩하는 메서드
- `GetBind`: 바인딩된 UI 요소를 반환하는 메서드

``` csharp
public abstract class ViewBase : MonoBehaviour
{
    ...
    
    public void Initialize(PresenterBase) { }
    protected virtual void InitializeBindComponent() { }
    protected virtual void InitializeEventListeners() { }
    protected virtual void SetupView() { }
    protected virtual void UpdateView(string) { }
    protected virtual void ShowView() { }
    protected virtual void HideView() { }
    protected void InvokeMethod(Enum, params object[]) { }
    protected TResult InvokeMethod<TResult>(Enum, params object[]) { }
    protected virtual void Bind<T>(Type) where T : Object { }
    protected T GetBind<T>(int) where T : Object { }
}
```

### 🔗 PresenterBase
`PresenterBase`는 View와 Model 사이에서 모든 데이터를 중계하며, View의 상호작용을 받아 Model의 로직을 실행합니다.

#### 주요 역할:
- Model의 데이터를 View에 전달
- View의 UI 상호작용 시 Model의 비즈니스 로직 실행을 중개

#### 핵심 메서드:
- `Initialize`: Model 및 View 초기화
- `AddMethod`: Model/View의 메서드를 등록하는 메서드
- `RemoveMethod`: Model/View의 메서드를 제거하는 메서드
- `InvokeMethod`: Model/View의 메서드를 호출하는 메서드
- `GetModelProperty`: Model의 프로퍼티 값을 반환하는 메서드
- `ShowView/HideView`: View의 활성화/비활성화 하는 메서드

```csharp
public abstract class PresenterBase : MonoBehaviour, IPresenter
{
    ...
    public virtual void Initialize() { /* Model, View 초기화 로직 실행*/ }
    public virtual void ShowView() => ExecuteSafe(_view, v => v.ShowView());
    public virtual void HideView() => ExecuteSafe(_view, v => v.HideView());
    public void AddMethod(Enum, Delegate) { }
    public void RemoveMethod(Enum) { }
    public void InvokeMethod(Enum, params object[]) { }
    public TResult InvokeMethod<TResult>(Enum, params object[]) { }
    public T GetModelProperty<T>(string) { }
}
```
---

## 3. MVPCreator를 활용한 간편한 MVP 클래스 생성
MVPCreator는 Unity 프로젝트에서 MVP 설계를 시작할 때 필수적인 Model, View, Presenter 클래스를 자동으로 생성하는 간편한 도구입니다.

### 사용법
1. Unity 프로젝트의 원하는 폴더를 마우스 우클릭.
2. **"Assets/Create/MVP Scripts"** 메뉴를 클릭.
3. 폴더 이름을 기반으로 아래와 같은 파일이 자동 생성됩니다:
   - `<폴더명>Model.cs`
   - `<폴더명>View.cs`
   - `<폴더명>Presenter.cs`


#### MVPCreator 특징:
- 파일 템플릿 활용으로 코드의 일관성 유지.
- 폴더 이름 활용으로 빠르고 자동화된 구조 생성.

---


## ClickerModel 예제: MVP의 실전 활용

### 플레이 영상

![MVP_Sample_Video](https://github.com/taehuuun/MVP-Pattern-Unity/assets/43982651/c136acc5-3f32-4ba3-aa45-15a08374b77f)


### [1️⃣ ClickerModel](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/Assets/MVP-Pattern-Unity/Scripts/Samples/Clicker/ClickerModel.cs)
ClickerModel은 클릭 기반 게임의 비즈니스 로직을 담당하는 Model 클래스입니다.

#### 주요 역할:
- 클릭 시 골드 증가 (`ClickAddGold` 메서드)
- 초당 골드 자동 증가

#### 예제:
```csharp
public class ClickerModel : ModelBase
{
    public enum MethodType
    {
        ClickAddGold,
        SecAddGold,
        UpgradeGoldPerClick,
        UpgradeGoldPerSec,
        StartGetGoldPerSec
    }
   
    ...
        
    public PlayerData Data { get; private set; } = new();
    public int CurrentGoldPerClick
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }
    public int CurrentGoldPerSec
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }
    public int NextGoldPerClickCost
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }
    public int NextGoldPerSecCost
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }
    
    ...

    public override void InitializeModelMethods()
    {
        base.InitializeModelMethods();
        
        AddMethod(MethodType.ClickAddGold, (Action)ClickAddGold);
        AddMethod(MethodType.SecAddGold, (Action)SecAddGold);
        AddMethod(MethodType.UpgradeGoldPerClick, (Action)UpgradeGoldPerClick);
        AddMethod(MethodType.UpgradeGoldPerSec, (Action)UpgradeGoldPerSec);
    }
    
    protected override void InitializeNestedProperties()
    {
        base.InitializeNestedProperties();
        
        Data.PropertyChanged += OnNestedPropertyChanged;
    }
    
    protected override void OnNestedPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnNestedPropertyChanged(sender, e);

        switch (e.PropertyName)
        {
            case nameof(Data.Gold):
                SetProperty(Data.Gold, nameof(Data.Gold));
                break;
            case nameof(Data.GoldPerClickLevel):
                SetProperty(Data.GoldPerClickLevel, nameof(Data.GoldPerClickLevel));
                break;
            case nameof(Data.GoldPerSecLevel):
                SetProperty(Data.GoldPerSecLevel, nameof(Data.GoldPerSecLevel));
                break;
        }
    }
    
    protected override void InitializeProperties()
    {
        ...
        Data.Gold = PlayerPrefs.GetInt(_GOLD_KEY,0);
        Data.GoldPerClickLevel = PlayerPrefs.GetInt(_GOLD_PER_CLICK_LEVEL_KEY, 0);
        Data.GoldPerSecLevel = PlayerPrefs.GetInt(_GOLD_PER_SEC_LEVEL_KEY, 0);
        
        CurrentGoldPerClick = _goldPerClickUpgrade.values[Data.GoldPerClickLevel];
        CurrentGoldPerSec = _goldPerSecUpgrade.values[Data.GoldPerSecLevel];
        NextGoldPerClickCost = _goldPerClickUpgrade.costs[Data.GoldPerClickLevel+1];
        NextGoldPerSecCost = _goldPerSecUpgrade.costs[Data.GoldPerSecLevel+1];
    }

    protected override void InitializeInvokeMethod()
    {
        base.InitializeInvokeMethod();
        
        StartGetGoldPerSec();
    }
    
    private void StartGetGoldPerSec() => StartCoroutine(GetGoldPerSec()); 
    private void ClickAddGold() { /* 클릭 당 골드 증가량 만큼 골드를 증가 시키는 메서드  */ }
    private void SecAddGold() { /* 초 당 골드를 증가 시키는 메서드 */ }
    private void UpgradeGoldPerClick() { /* 클릭 당 골드 증가량 업그레이드 메서드 */ }
    private void UpgradeGoldPerSec() { /* 초 당 골드 증가량 업그레이드 메서드 */ }
    private void OnApplicationQuit() { /* 게임 종료 시 진행 내용을 PlayerPrefs에 저장하는 메서드 */ }
  
    private IEnumerator SizeUp(int) { /* 보유 골드에 따라 원 크기를 조절하는 코루틴 */} 
    private IEnumerator GetGoldPerSec() { /* 초마다 일정 골드를 증가시키는 코루틴 */ }
}
```
---
### [️2️⃣ ClickerView](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/Assets/MVP-Pattern-Unity/Scripts/Samples/Clicker/ClickerView.cs)
ClickerView는 UI 상호작용 및 추적중인 프로퍼티가 바뀔 때 UI를 업데이트합니다.

#### 주요 역할:
- UI 버튼 및 텍스트를 관리.
- Model의 데이터가 변경될 경우 해당 변경 사항을 표시.

#### 예제:
```csharp
public class ClickerView : ViewBase
{
    private enum Texts
    {
        GoldPerClickText,
        CurrentGoldText,
        GoldPerSecText,
        ClickUpgradeLevelText,
        ClickUpgradeCostText,
        SecUpgradeLevelText,
        SecUpgradeCostText
    }

    private enum Buttons
    {
        TouchPanelBtn,
        ClickUpgradeBtn,
        SecUpgradeBtn
    }
    
    protected override void InitializeBindComponent()
    {
        base.InitializeBindComponent();
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }

    protected override void InitializeEventListeners()
    {
        base.InitializeEventListeners();
        GetBind<Button>((int)Buttons.TouchPanelBtn).onClick.AddListener(() => InvokeMethod(ClickerModel.MethodType.ClickAddGold));
        GetBind<Button>((int)Buttons.ClickUpgradeBtn).onClick.AddListener(() => InvokeMethod(ClickerModel.MethodType.UpgradeGoldPerClick));
        GetBind<Button>((int)Buttons.SecUpgradeBtn).onClick.AddListener(() => InvokeMethod(ClickerModel.MethodType.UpgradeGoldPerSec));
    }

    protected override void UpdateView(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ClickerModel.Data.Gold):
                GetBind<TMP_Text>((int)Texts.CurrentGoldText).text = $"{GetProperty<int>(propertyName)} G";
                break;
            case nameof(ClickerModel.Data.GoldPerClickLevel):
                GetBind<TMP_Text>((int)Texts.ClickUpgradeLevelText).text = $"Next Lv: {GetProperty<int>(nameof(ClickerModel.Data.GoldPerClickLevel))}";
                break;
            case nameof(ClickerModel.CurrentGoldPerClick):
                GetBind<TMP_Text>((int)Texts.GoldPerClickText).text = $"+{GetProperty<int>(nameof(ClickerModel.CurrentGoldPerClick))} / Click";
                break;
            case nameof(ClickerModel.NextGoldPerClickCost):
                GetBind<TMP_Text>((int)Texts.ClickUpgradeCostText).text = $"Next Cost: {GetProperty<int>(nameof(ClickerModel.NextGoldPerClickCost))}";
                break;
            case nameof(ClickerModel.CurrentGoldPerSec):
                GetBind<TMP_Text>((int)Texts.GoldPerSecText).text = $"+{GetProperty<int>(nameof(ClickerModel.CurrentGoldPerSec))} / Sec";
                break;
            case nameof(ClickerModel.Data.GoldPerSecLevel):
                GetBind<TMP_Text>((int)Texts.SecUpgradeLevelText).text = $"Next Lv: {GetProperty<int>(nameof(ClickerModel.Data.GoldPerSecLevel))}";
                break;
            case nameof(ClickerModel.NextGoldPerSecCost):
                GetBind<TMP_Text>((int)Texts.SecUpgradeCostText).text = $"Next Cost: {GetProperty<int>(nameof(ClickerModel.NextGoldPerSecCost))}";
                break;
        }
    }
}
```

---


### [3️⃣ ClickerPresenter](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/Assets/MVP-Pattern-Unity/Scripts/Samples/Clicker/ClickerPresenter.cs)
ClickerPresenter는 View의 이벤트를 받아 Model의 로직을 실행합니다.

#### 주요 역할:
- View와 Model 간 상호작용 중재

#### 예제:
```csharp
public class ClickerPresenter : PresenterBase
{
}

```
---


## 라이선스
이 프로젝트는 MIT 라이선스를 따릅니다. 자세한 내용은 [LICENSE](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/LICENSE) 파일을 참조하세요.
## 연락처 정보
- TISTORY 블로그: <https://devvdevv.tistory.com/>
- 이메일: <lsw463@naver.com>
