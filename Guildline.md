## GIT CONVENTION

### COMMIT

- feat            新功能
- fix             修 bug
- docs            文件變更
- style           格式變更（無功能變動）
- refactor        重構程式碼（非新功能或修 bug）
- test            加入測試或測試變動
- chore           雜項，不影響程式的修改
- perf            性能優化
- ci              持續整合設定
- build           建構系統或外部依賴變更

### CHANGELOG
- Added           新增功能或項目（新增了什麼）
- Changed         現有功能的改動（行為變了）
- Deprecated      即將被移除的功能（仍可用，但不建議）
- Removed         移除了的功能（不再可用）
- Fixed           錯誤修正（Bug 修復）
- Security        安全性修補（如漏洞封堵）

## CODE CONVENTION

### NAMING

- Private Field             _camelCase        _health, _speed
- Unity Serialized Field    _camelCase        _jumpForce, _cooldownTime
- Public Field              PascalCase        MaxSpeed, IsDead
- Property                  PascalCase        Health, IsAlive
- Local Variable            camelCase         damage, newPosition

### ORDERING

#### File root level or within a namespace

- Extern Alias Directives
- Using Directives
- Namespaces
- Delegates
- Enums
- Interfaces
- Structs
- Classes
- Within a class, struct, or interface, elements should be positioned in the following order:

#### Within a class, struct, or interface, elements

- Fields
- Constructors
- Finalizers (Destructors)
- Delegates
- Events
- Enums
- Interfaces
- Properties
- Indexers
- Methods
- Structs
- Classes*
