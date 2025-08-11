````markdown
# BankAccount API
````

**BankAccount API** — это REST API сервис для управления банковскими счетами.  
Реализован на **ASP.NET 9**, с поддержкой **JWT-аутентификации через Keycloak**, хранением данных в **PostgreSQL** и документацией в **Swagger**.

## 📋 Возможности
- Создание, просмотр, обновление и удаление банковских счетов.
- Аутентификация пользователей через Keycloak.
- Хранение данных в PostgreSQL.
- Полная спецификация API в Swagger.
- Использование unit тестов.

---

## 🚀 Запуск локально

### 1. Установить зависимости
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- PostgreSQL
- Keycloak

### 2. Настроить строку подключения в `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=qwerty123"
}
````

### 3. Запуск сервиса
Зайти в папку с проектом \ModuleBank_Task_1
```bash
dotnet run --project BankAccount
```

Сервис будет доступен по адресу:
[http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## 🐳 Запуск в Docker

### 1. Собрать и запустить контейнеры

```bash
docker compose up --build
```

### 2. Сервисы будут доступны:

* **BankAccount API** → [http://localhost:5000/swagger](http://localhost:5000/swagger)
* **Keycloak** → [http://localhost:18080](http://localhost:18080) (логин: `admin`, пароль: `admin`)
* **PostgreSQL** → `localhost:5432` (user: `postgres`, password: `qwerty123`, db: `mydb`)

---

## 📂 Структура проекта

```
BankAccount/             # Исходный код сервиса
BankAccount/Dockerfile   # Сборка контейнера BankAccount API
docker-compose.yml       # Конфигурация сервисов
README.md                # Документация проекта
```

---

## 🧪 Unit тесты

В проекте реализованы unit тесты для проверки бизнес-логики сервиса.

### Запуск тестов в IDE

- Откройте решение в вашей IDE.
- Запустите все тесты через встроенный тестовый раннер:
  - В **Visual Studio**: меню *Test* → *Run All Tests* или панель тестов.
  - В **Rider**: окно *Unit Tests* → *Run All*.
  - В **VS Code**: используйте расширение *.NET Test Explorer* для запуска тестов.
  
Тесты автоматически соберутся и выполнятся. Результаты будут отображены в панели тестов.
