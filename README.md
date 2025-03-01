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

![MVP](https://github.com/user-attachments/assets/6a8db886-c797-4bef-8b3f-02bf8ed372f9)

## 2. Base 클래스 설명

### 🔗 ModelBase
`ModelBase`는 모든 **Model** 클래스의 부모로써 데이터를 관리하고, Presenter와의 상호작용을 지원하는 역할을 수행합니다. 데이터 초기화 및 데이터 변경 감지 등을 처리하며 게임의 비즈니스 로직의 핵심을 구성합니다.

#### 핵심 메서드:
- `InitializeMethods`: Model 내 메서드 중 Presenter를 통해 호출 할 메서드를 AddMethod를 통해 등록 시키는 메서드
- `InitializeProperties`: Model 내 프로퍼티를 초기화 하기 위한 메서드
- `InitializeNestedProperties`: Model 내 참조 타입의 프로퍼티 변경 감지를 위해 이벤트를 등록하는 메서드
- `AddMethod`: Model 내 메서드들을 등록하기 위한 메서드
- `InvokeMethod`: 등록된 메서드를 Presenter를 통해 호출하기 위한 메서드
```csharp
public abstract class ModelBase : MonoBehaviour, INotifyPropertyChanged
{
    ...
    public void Initialize()
    {
        InitializeMethods();
        InitializeNestedProperties();
        InitializeProperties();
    }
    
    protected virtual void InitializeMethods() { }
    protected virtual void InitializeNestedProperties() { }
    protected abstract void InitializeProperties();
}
```
### 🔗 ViewBase
`ViewBase`는 모든 **View** 클래스의 부모클래스로, UI 컴포넌트(버튼, 텍스트 등)를 초기화하고 Presenter와 상호작용합니다. **ViewBase**는 사용자 인터페이스를 담당합니다.


#### 주요 역할:
- UI 요소 초기화 및 바인딩
- Model의 변경 사항을 UI에 반영

#### 핵심 메서드:
- `InitializeBind()` : UI요소 및 오브젝트들을 바인딩을 하기 위한 메서드
- `InitializeEvents(PresenterBase<TModel>)`: 버튼, 슬라이더의 이벤트 리스너 추가를 위한 메서드
- `UpdateView(string)`: Model 내 변경된 프로퍼티에 맞게 UI를 업데이트 하는 메서드

``` csharp
public abstract class ViewBase<TModel> : MonoBehaviour where TModel : ModelBase
{
    ...
    
    public virtual void Initialize(PresenterBase<TModel> presenter)
    {
        p_presenter = presenter;
        InitializeBind();
        InitializeEvents(presenter);
    }
    
    public abstract void UpdateView(string propertyName);
    protected virtual void InitializeBind() { }
    protected virtual void InitializeEvents(PresenterBase<TModel> presenter) { }
}
```

### 🔗 PresenterBase
`PresenterBase`는 View와 Model 사이에서 모든 데이터를 중계하며, View의 상호작용을 받아 Model의 로직을 실행합니다.

#### 주요 역할:
- Model의 데이터를 View에 전달
- View의 UI 상호작용 시 Model의 비즈니스 로직 실행을 중개

#### 핵심 메서드:
- `Initialize`: Model 및 View 초기화
- `InvokeMethod`: Model의 메서드를 호출
- `ShowView/HideView`: View의 활성화/비활성화

```csharp
public abstract class PresenterBase<TModel> : MonoBehaviour, IPresenter where TModel : ModelBase
{
    ...
    protected virtual void Initialize()
    {
        /* Model, View 초기화 로직 실행*/
    }
    
    public virtual void ShowView() => ExecuteSafe(_view, v => v.ShowView());
    public virtual void HideView() => ExecuteSafe(_view, v => v.HideView());
    public void InvokeMethod(Enum methodType) { /* 반환값 X, 매개변수X */ }
    public void InvokeMethod<TParam>(Enum methodType, TParam param) { /* 반환값 X, 매개변수 1개 */ }
    
    ... 호출 타입에 맞는 InvokeMethod ...
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
    
    ...

    protected override void InitializeMethods()
    {
        base.InitializeMethods();
        
        AddMethod(MethodType.ClickAddGold, ClickAddGold);
        AddMethod(MethodType.SecAddGold, SecAddGold);
        AddMethod(MethodType.UpgradeGoldPerClick, UpgradeGoldPerClick);
        AddMethod(MethodType.UpgradeGoldPerSec, UpgradeGoldPerSec);
        AddMethod(MethodType.StartGetGoldPerSec, StartGetGoldPerSec);
    }
    protected override void InitializeNestedProperties()
    {
        base.InitializeNestedProperties();
        
        Data.PropertyChanged += OnNestedPropertyChanged;
    }
    protected override void InitializeProperties()
    {
        ...
        Data.Gold = PlayerPrefs.GetInt(_GOLD_KEY,0);
        Data.GoldPerClickLevel = PlayerPrefs.GetInt(_GOLD_PER_CLICK_LEVEL_KEY, 0);
        Data.GoldPerSecLevel = PlayerPrefs.GetInt(_GOLD_PER_SEC_LEVEL_KEY, 0);
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
public class ClickerView : ViewBase<ClickerModel>
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
    
    protected override void InitializeBind()
    {
        base.InitializeBind();
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }

    protected override void InitializeEvents(PresenterBase<ClickerModel> presenter)
    {
        base.InitializeEvents(presenter);
        GetBind<Button>((int)Buttons.TouchPanelBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.MethodType.ClickAddGold));
        GetBind<Button>((int)Buttons.ClickUpgradeBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.MethodType.UpgradeGoldPerClick));
        GetBind<Button>((int)Buttons.SecUpgradeBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.MethodType.UpgradeGoldPerSec));
    }

    public override void UpdateView(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ClickerModel.Data.Gold):
                GetBind<TMP_Text>((int)Texts.CurrentGoldText).text = $"{GetModel().Data.Gold} G";
                break;
            case nameof(ClickerModel.Data.GoldPerClickLevel):
                UpdateGoldPerClickUpdateUI();
                break;
            case nameof(ClickerModel.Data.GoldPerSecLevel):
                UpdateGoldPerSecUpdateUI();
                break;
        }
    }

    private void UpdateGoldPerClickUpdateUI() { /* 클릭 당 골드 UI 업데이트 로직 */ }
    private void UpdateGoldPerSecUpdateUI() { /* 초 당 골드 UI 업데이트 로직 */ }
}
```

---


### [3️⃣ ClickerPresenter](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/Assets/MVP-Pattern-Unity/Scripts/Samples/Clicker/ClickerPresenter.cs)
ClickerPresenter는 View의 이벤트를 받아 Model의 로직을 실행합니다.

#### 주요 역할:
- View와 Model 간 상호작용 중재

#### 예제:
```csharp
public class ClickerPresenter : PresenterBase<ClickerModel>
{
    protected override void Initialize()
    {
        base.Initialize();

        InvokeMethod(MethodType.StartGetGoldPerSec);
    }
}
```
---


## 라이선스
이 프로젝트는 MIT 라이선스를 따릅니다. 자세한 내용은 [LICENSE](https://github.com/taehuuun/MVP-Pattern-Unity/blob/main/LICENSE) 파일을 참조하세요.
## 연락처 정보
- TISTORY 블로그: <https://devvdevv.tistory.com/>
- 이메일: <lsw463@naver.com>
