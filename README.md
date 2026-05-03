# نظام إجازات الموظفين
## Employee Leave Management System

---

## 🚀 خطوات التشغيل

### 1️⃣ Backend - .NET 8 Web API

```bash
cd EmployeeLeaveSystem/API

# تثبيت الـ packages
dotnet restore

# إنشاء Migration (لأول مرة)
dotnet ef migrations add InitialCreate

# تطبيق قاعدة البيانات
dotnet ef database update

# تشغيل السيرفر
dotnet run
```

> ✅ السيرفر هيشتغل على: http://localhost:5000
> ✅ Swagger UI: http://localhost:5000/swagger

---

### 2️⃣ Frontend - Angular 17

```bash
cd EmployeeLeaveSystem/Angular

# تثبيت الـ packages
npm install

# تشغيل Angular
ng serve
```

> ✅ Angular هيشتغل على: http://localhost:4200

---

## 🗄️ إعداد SQL Server

افتح `appsettings.json` وعدّل Connection String:

```json
"DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeLeaveDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

---

## 📋 الـ API Endpoints

| Method | Endpoint | الوصف |
|--------|----------|-------|
| GET | /api/employees | كل الموظفين |
| GET | /api/employees/{id} | موظف بالـ ID |
| POST | /api/employees | إضافة موظف |
| PUT | /api/employees/{id} | تعديل موظف |
| DELETE | /api/employees/{id} | حذف موظف |
| GET | /api/leaves/employee/{id} | إجازات الموظف |
| POST | /api/leaves | إضافة إجازة |
| PUT | /api/leaves/{id} | تعديل إجازة |
| DELETE | /api/leaves/{id} | حذف إجازة |

---

## ⚠️ قواعد الإجازات
1. مدة الإجازة لا تقل عن **30 يوم**
2. لا يمكن تسجيل **إجازتين في نفس الفترة** لنفس الموظف
3. إجمالي الإجازة لا يتجاوز **30 يوم سنوياً** لكل نوع

---

## 🛠️ المتطلبات
- .NET 8 SDK
- SQL Server (أو SQL Server Express)
- Node.js 18+
- Angular CLI: `npm install -g @angular/cli`
- Entity Framework Tools: `dotnet tool install --global dotnet-ef`
