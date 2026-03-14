# Auth API

Base URL: `/api/auth`

---

## POST `/api/auth/register`

Yangi foydalanuvchi (Student) ro'yxatdan o'tkazadi.

### Request

```http
POST /api/auth/register
Content-Type: application/json
```

```json
{
  "firstName": "Dilshod",
  "lastName": "Karimov",
  "phoneNumber": "+998901234567",
  "password": "Secret123",
  "dateOfBirth": "2000-01-15"
}
```

| Field         | Type     | Required | Description              |
|---------------|----------|----------|--------------------------|
| `firstName`   | string   | ✅       |                          |
| `lastName`    | string   | ✅       |                          |
| `phoneNumber` | string   | ✅       | Unikal bo'lishi kerak    |
| `password`    | string   | ✅       |                          |
| `dateOfBirth` | datetime | ❌       |                          |

> Role avtomatik `Student` tayinlanadi.

### Response `200 OK`

```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2026-03-11T12:00:00Z",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "fullName": "Dilshod Karimov",
    "roleName": "Student"
  }
}
```

---

## POST `/api/auth/login`

Mavjud foydalanuvchi tizimga kiradi.

### Request

```http
POST /api/auth/login
Content-Type: application/json
```

```json
{
  "phoneNumber": "+998901234567",
  "password": "Secret123"
}
```

| Field         | Type   | Required |
|---------------|--------|----------|
| `phoneNumber` | string | ✅       |
| `password`    | string | ✅       |

### Response `200 OK`

```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2026-03-11T12:00:00Z",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "fullName": "Dilshod Karimov",
    "roleName": "Student"
  }
}
```

---

## Xato holatlari

### `401 Unauthorized` — noto'g'ri login yoki parol

```json
{
  "errorCode": "error_invalid_credentials"
}
```

### `404 Not Found` — Student roli topilmadi (register da)

```json
{
  "errorCode": "error_student_role_not_found"
}
```

### `500 Internal Server Error`

```json
{
  "errorCode": "error_internal_server_error"
}
```

---

## JWT Token

Login/register dan qaytgan `accessToken` — keyingi so'rovlarda `Authorization` headeriga qo'shiladi:

```http
Authorization: Bearer <accessToken>
```

Token ichida quyidagi claim'lar bor:

| Claim  | Qiymat                      |
|--------|-----------------------------|
| `sub`  | `userId`                    |
| `name` | `fullName`                  |
| `role` | `roleName` (Student, Admin) |
