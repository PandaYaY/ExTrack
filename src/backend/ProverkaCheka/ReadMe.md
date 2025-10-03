# ProverkaCheka

Клиент для работы с API сервиса проверки кассовых чеков [proverkacheka.com](https://proverkacheka.com).

## Описание

ProverkaCheka - это .NET библиотека для интеграции с сервисом проверки кассовых чеков. Сервис позволяет проверять легальность кассовых чеков и получать подробную информацию о покупках.

## Возможности

- ✅ Проверка кассовых чеков по реквизитам
- ✅ Проверка чеков по QR-коду (строка)
- ✅ Получение детальной информации о покупке
- ✅ Поддержка всех типов операций (приход, возврат прихода, расход, возврат расхода)
- ✅ Асинхронные операции
- ✅ Настраиваемые таймауты
- ✅ Полная типизация данных

## Установка

### NuGet пакеты

```xml
<PackageReference Include="Refit" Version="8.0.0" />
<PackageReference Include="Refit.HttpClientFactory" Version="8.0.0" />
<PackageReference Include="Refit.Newtonsoft.Json" Version="8.0.0" />
```

## Быстрый старт

### 1. Настройка конфигурации

Добавьте в `appsettings.json`:

```json
{
  "ProverkaCheka": {
    "BaseUrl": "https://proverkacheka.com",
    "Timeout": "00:00:30",
    "AccessToken": "YOUR_ACCESS_TOKEN"
  }
}
```

**Получение токена доступа:**
1. Перейдите на [https://proverkacheka.com/cabinet/profile](https://proverkacheka.com/cabinet/profile)
2. Войдите в личный кабинет
3. Скопируйте токен доступа из раздела API

### Регистрация сервиса

```csharp
using ProverkaCheka.Client;

// В Program.cs или Startup.cs
builder.Services.AddProverkaChekaClient(builder.Configuration);
```

## Конфигурация

### Настройки клиента

```json
{
  "ProverkaChecka": {
    "BaseUrl": "https://proverkacheka.com",  // Базовый URL API
    "Timeout": "00:00:30",                   // Таймаут запросов
    "AccessToken": "YOUR_TOKEN"              // Токен доступа
  }
}
```

## Поддержка

- Документация API: [https://proverkacheka.com](https://proverkacheka.com)
- Получение токена: [https://proverkacheka.com/cabinet/profile](https://proverkacheka.com/cabinet/profile)

## Структура проекта

```
ProverkaCheka/
├── ProverkaCheka.Client/          # HTTP клиент
├── ProverkaCheka.Dto/             # Модели данных
└── ProverkaCheka.Tests/           # Тесты
```
