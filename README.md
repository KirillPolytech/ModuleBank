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

### 3. Применить миграции

```bash
dotnet ef database update
```

### 4. Запустить сервис

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

### 3. Применить миграции в контейнере (если нужно вручную)

```bash
docker exec -it bank-api-1 dotnet ef database update --connection "Host=db;Port=5432;Database=mydb;Username=postgres;Password=qwerty123"
```

---

## 📂 Структура проекта

```
BankAccount/         # Исходный код сервиса
docker-compose.yml   # Конфигурация сервисов
Dockerfile           # Сборка контейнера BankAccount API
README.md            # Документация проекта
```

---

## 🛠 Полезные команды

### Подключиться к PostgreSQL внутри контейнера

```bash
docker exec -it modulebank_task_1-db-1 psql -U postgres -d mydb
```

### Просмотреть логи сервиса

```bash
docker logs bank-api-1
```
