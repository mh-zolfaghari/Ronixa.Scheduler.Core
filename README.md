
# 📐 Ronixa.Scheduler.Core - Job Scheduler Project

🇬🇧 English | 🇮🇷 فارسی

![dotnet-version](https://img.shields.io/badge/dotnet%20version-net8.0-blue)

---

# 📑 Table of Contents | فهرست مطالب
- [⭐ Introduction | مقدمه](#-introduction--مقدمه)
- [🔎 Core Features | ویژگی‌های اصلی](#-core-features--ویژگیهای-اصلی)
- [✅ Technical Features | ویژگی‌های فنی](#-technical-features--ویژگیهای-فنی)
- [🧑‍💻 Development & Deployment | توسعه و استقرار](#-development--deployment--توسعه-و-استقرار)
- [💾 Getting Started | شروع به کار](#-getting-started--شروع-به-کار)
- [🚀 Usage Example | نمونه استفاده](#-usage-example--نمونه-استفاده)
- [📊 Job Manager API | متدهای مدیریت جاب](#-job-manager-api--متدهای-مدیریت-جاب)
- [🛠 Best Practices | بهترین شیوه‌ها](#-best-practices--بهترین-شیوهها)
- [❓ FAQ | سوالات متداول](#-faq--سوالات-متداول)
- [🌈 Contributing | مشارکت](#-contributing--مشارکت)
- [🌟 Support | پشتیبانی](#-support--پشتیبانی)
- [🔐 License | لایسنس](#-license--لایسنس)
- [🩷 Follow Me | ارتباط با من](#-follow-me--ارتباط-با-من)

---

## ⭐ Introduction | مقدمه

**EN**  
The **Ronixa.Scheduler.Core** project is a **.NET 8 library** for background job scheduling using `IHostedService`.  
It enables developers to run recurring jobs, monitor execution, and manage job lifecycles easily in **ASP.NET Core applications**.

**FA**  
پروژه **Ronixa.Scheduler.Core** یک کتابخانه‌ی **.NET 8** برای زمان‌بندی وظایف پس‌زمینه با استفاده از `IHostedService` است.  
این کتابخانه به توسعه‌دهندگان کمک می‌کند تا به راحتی جاب‌های تکراری را اجرا کنند، وضعیت آن‌ها را پایش نمایند و چرخه عمرشان را مدیریت کنند.

---

## 🔎 Core Features | ویژگی‌های اصلی

**EN**
- ⏰ Recurring Jobs with `TimeSpan`
- ▶️ Lifecycle management (Start/Stop/Pause/Resume)
- 📝 Execution Logging & Metadata
- ⚡ Exception Handling
- 🛠 DI Friendly
- 📊 Status Monitoring
- 🚀 Plug-and-Play

**FA**
- ⏰ جاب‌های تکراری با استفاده از `TimeSpan`
- ▶️ مدیریت چرخه عمر (شروع/توقف/مکث/ادامه)
- 📝 ثبت لاگ و متادیتا
- ⚡ مدیریت خطاها
- 🛠 سازگار با DI
- 📊 مانیتورینگ وضعیت
- 🚀 نصب و استفاده سریع

---

## ✅ Technical Features | ویژگی‌های فنی

**EN**
- Built on **.NET 8** & `IHostedService`
- `IRonixaJobManagerService` for global control
- Declarative job metadata with attributes
- Strongly-typed configuration
- No external dependencies

**FA**
- ساخته‌شده بر بستر **.NET 8** و `IHostedService`
- استفاده از `IRonixaJobManagerService` برای کنترل سراسری
- متادیتای جاب‌ها با Attributeها
- کانفیگ strongly-typed
- بدون وابستگی خارجی

---

## 🧑‍💻 Development & Deployment | توسعه و استقرار

**EN**
- Cross-platform (Windows/Linux/macOS)
- Cloud & Docker/Kubernetes Ready
- Extensible for monitoring/logging
- Production-ready

**FA**
- کراس‌پلتفرم (ویندوز/لینوکس/مک)
- آماده برای Cloud و Docker/K8s
- قابلیت توسعه برای مانیتورینگ/لاگ‌گیری
- آماده برای محیط Production

---

## 💾 Getting Started | شروع به کار

### Prerequisites | پیش‌نیازها
**EN**
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Git](https://git-scm.com/)

**FA**
- نصب [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- نصب [Git](https://git-scm.com/)

### Clone & Restore | کلون و بازیابی

```bash
git clone https://github.com/mh-zolfaghari/Ronixa.Scheduler.Core.git
cd Ronixa.Scheduler.Core
dotnet restore
```

---

## 🚀 Usage Example | نمونه استفاده

### 1. Define Job | تعریف جاب
```csharp
[RonixaJobScheduler(id: JobIds.SampleJobKey, title: "Sample Job", description: "This is Sample Job.")]
public class SampleJob : RonixaJob
{
    private readonly ILogger<SampleJob> _logger;

    public SampleJob(IRonixaJobScheduleConfiguration<SampleJob> config, ILogger<SampleJob> logger)
        : base(config.Duration, config.ForceExecuteInStartJob)
    {
        _logger = logger;
        Job_Executed += SampleJob_Job_Executed;
    }

    private void SampleJob_Job_Executed(object? sender, IRonixaJobExecuted e)
    {
        if (e.Status.State == RonixaJobExecutionState.Done)
            _logger.LogInformation($"{e.Information.Title} completed.");

        if (e.Status.State == RonixaJobExecutionState.Error)
            _logger.LogError(e.Status.Exception, $"{e.Information.Title} failed.");
    }

    public override IRonixaJobInformation SetJobInfo() => GetJobInfo(this);

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("SampleJob is running...");
        await Task.CompletedTask;
    }
}
```

### 2. Register Services | ثبت سرویس‌ها
```csharp
builder.Services.AddRonixaSchedulingJob<SampleJob>(c =>
{
    c.Duration = TimeSpan.FromSeconds(10);
    c.ForceExecuteInStartJob = true;
});

builder.Services.AddRonixaJobManagerService();
```

### 3. Control via API | کنترل از طریق API

**EN**  
Below is a sample `JobController` that demonstrates how to control jobs using REST API. This example is aligned with the **Sample Project** included in the repository.

**FA**  
کد زیر یک `JobController` نمونه است که نشان می‌دهد چگونه می‌توانید از طریق REST API جاب‌ها را کنترل کنید. این مثال مطابق با **پروژه Sample** موجود در ریپازیتوری نوشته شده است.

```csharp
[ApiController]
[Route("jobs")]
public class JobController(IRonixaJobManagerService ronixaJobManager) : ControllerBase
{
    [HttpGet("/play")]
    public IActionResult Play() => GetSampleJobStatus(() => ronixaJobManager.StartJob(JobIds.SampleJobId));

    [HttpGet("/pause")]
    public IActionResult Pause() => GetSampleJobStatus(() => ronixaJobManager.PauseJob(JobIds.SampleJobId));

    [HttpGet("/stop")]
    public IActionResult Stop() => GetSampleJobStatus(() => ronixaJobManager.StopJob(JobIds.SampleJobId));

    [HttpGet("/status")]
    public IActionResult Status() => GetSampleJobStatus();

    [HttpGet("/list")]
    public IActionResult List() => Ok(ronixaJobManager.GetAll());

    private OkObjectResult GetSampleJobStatus(Action? jobAction = null)
    {
        jobAction?.Invoke();
        return Ok(ronixaJobManager.GetJobStatus(JobIds.SampleJobId));
    }
}
```

---

## 📊 Job Manager API | متدهای مدیریت جاب

| Method | Description (EN) | توضیح (FA) |
|--------|------------------|-------------|
| `StartJob(Guid id)` | Start job execution | شروع جاب |
| `PauseJob(Guid id)` | Pause job execution | توقف موقت جاب |
| `ResumeJob(Guid id)` | Resume paused job | ادامه جاب متوقف‌شده |
| `StopJob(Guid id)` | Stop job completely | توقف کامل جاب |
| `GetJobStatus(Guid id)` | Get job status & metadata | دریافت وضعیت جاب |
| `GetAll()` | List all registered jobs | لیست همه جاب‌های ثبت‌شده |

---

## 🛠 Best Practices | بهترین شیوه‌ها

**EN**
- Use **CancellationToken** properly in jobs.  
- Keep jobs **idempotent** (safe to run multiple times).  
- Implement **structured logging**.  
- Prefer **short intervals** only for lightweight jobs.  
- Always handle **exceptions inside ExecuteAsync**.

**FA**
- از **CancellationToken** به‌درستی استفاده کنید.  
- جاب‌ها را **idempotent** طراحی کنید.  
- لاگ‌گیری ساخت‌یافته داشته باشید.  
- فاصله‌های کوتاه را فقط برای جاب‌های سبک استفاده کنید.  
- همیشه **مدیریت خطاها** را داخل `ExecuteAsync` انجام دهید.

---

## ❓ FAQ | سوالات متداول

**Q: Can I run multiple jobs in parallel?**  
Yes, each job runs in its own hosted task.  

**س: آیا می‌توان چند جاب را موازی اجرا کرد؟**  
بله، هر جاب در یک Task مجزا اجرا می‌شود.

**Q: Can I dynamically add jobs at runtime?**  
Currently, jobs must be registered at startup, but manager APIs allow runtime control.  

**س: آیا می‌توان جاب‌ها را در زمان اجرا اضافه کرد؟**  
فعلاً جاب‌ها باید در startup ثبت شوند، ولی مدیریت آن‌ها در زمان اجرا امکان‌پذیر است.

---

## 🌈 Contributing | مشارکت

**EN**
Contributions are welcome! Fork, branch, commit, and PR 🚀

**FA**
مشارکت شما پذیرفته است! Fork کنید، Branch بسازید، Commit بزنید و PR بدهید 🚀

---

## 🌟 Support | پشتیبانی

**EN**
If you like this project, please ⭐ star the repo.  

**FA**
اگر این پروژه را مفید می‌دانید، لطفاً ⭐ ستاره بدهید.

---

## 🔐 License | لایسنس

Distributed under the MIT License.  
توزیع‌شده تحت لایسنس MIT.

---

## 🩷 Follow Me | ارتباط با من

[![LinkedIn][linkedin-shield]][linkedin-url]  [![Telegram][telegram-shield]][telegram-url]  [![WhatsApp][whatsapp-shield]][whatsapp-url]  [![Gmail][gmail-shield]][gmail-url]  ![GitHub followers](https://img.shields.io/github/followers/mh-zolfaghari)

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?logo=linkedin&color=555
[linkedin-url]: https://www.linkedin.com/in/ronixa/

[telegram-shield]: https://img.shields.io/badge/-Telegram-black.svg?logo=telegram&color=fff
[telegram-url]: https://t.me/DanialDotNet

[whatsapp-shield]: https://img.shields.io/badge/-WhatsApp-black.svg?logo=whatsapp&color=fff
[whatsapp-url]: https://wa.me/989389043224

[gmail-shield]: https://img.shields.io/badge/-Gmail-black.svg?logo=gmail&color=fff
[gmail-url]: mailto:personal.mhz@gmail.com
