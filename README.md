# 📐 Ronixa.Scheduler.Core - Job Scheduler Project

The **Ronixa.Scheduler.Core** project is a **.NET 8 library** that implements a powerful and flexible job scheduling system built on top of the `IHostedService` dependency structure.  
It is designed to make background job scheduling in **ASP.NET Core applications** clean, extensible, and developer-friendly.  

By integrating this library into your application, you can:
- Run automated recurring jobs with flexible scheduling.
- Start, stop, pause, and resume jobs at runtime programmatically.
- Capture execution details such as last success, last error, duration, and execution state.
- Manage jobs via API endpoints or programmatically through a **Job Manager Service**.

![dotnet-version](https://img.shields.io/badge/dotnet%20version-net8.0-blue)

---

## ⭐ Introduction

In modern applications, recurring tasks such as **sending emails**, **generating reports**, **data synchronization**, or **cleanup tasks** are very common.  
Instead of relying on external schedulers or over-complicating with Windows services and cron jobs, **Ronixa.Scheduler.Core** provides a **lightweight and native .NET solution**.  

This library allows developers to implement recurring jobs with:
- Minimal configuration
- Centralized monitoring
- Full control over job lifecycle at runtime

---

## 🔎 Core Features

- ⏰ **Recurring Jobs** – Schedule jobs to run at fixed intervals using `TimeSpan`.
- ▶️ **Job Lifecycle Management** – Start, stop, pause, and resume jobs dynamically.
- 📝 **Execution Logging** – Track job execution state (`Started`, `Done`, `Error`), duration, and timestamps.
- ⚡ **Error Handling** – Automatically logs job exceptions with contextual information.
- 🛠 **Dependency Injection Friendly** – Fully integrated with the **ASP.NET Core DI container**.
- 📊 **Status Monitoring** – Access job metadata (`LastExecution`, `NextExecution`, `LastSuccess`, `LastException`) at runtime.
- 🚀 **Plug-and-Play Integration** – Just add your jobs and register them via built-in extension methods.

---

## ✅ Technical Features

- Built on **.NET 8** and `IHostedService` for first-class integration.
- Provides **`IRonixaJobManagerService`** to manage jobs globally.
- Declarative job metadata using the `RonixaJobSchedulerAttribute`.
- Strongly-typed job configuration via `IRonixaJobScheduleConfiguration<T>`.
- Lightweight implementation (no external dependencies, only uses BCL & Microsoft.Extensions.Hosting).
- Thread-safe lifecycle management with lock-based state control.

---

## 🧑‍💻 Development and Deployment Features

- **Cross-Platform** – Works on Windows, Linux, and macOS.
- **Cloud Ready** – Ideal for containerized environments like **Docker** and **Kubernetes**.
- **Extensible** – Easily add custom logging, monitoring, and execution policies.
- **Production Ready** – Designed with error handling, safe job restarts, and resiliency.

---

## 💾 Getting Started

### Prerequisites
Make sure you have the following tools installed on your machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/)

### Cloning the Repository
Open your terminal or command prompt and run the following command:

```bash
git clone https://github.com/mh-zolfaghari/Ronixa.Scheduler.Core.git
```

This will create a local copy of the **Ronixa.Scheduler.Core** repository on your machine.

### Restoring Dependencies
Navigate to the project directory:

```bash
cd Ronixa.Scheduler.Core
```

Run the following command to restore project dependencies:

```bash
dotnet restore
```

---

## 🚀 Usage Example

### 1. Define a Job
```csharp
[RonixaJobScheduler(
    id: "EDF401E5-4FB7-4B98-B6B8-68528F617432",
    title: "Sample Job",
    description: "This is Sample Job."
)]
public class SampleJob : RonixaJob
{
    private readonly ILogger<SampleJob> _logger;

    public SampleJob(IRonixaJobScheduleConfiguration<SampleJob> config, ILogger<SampleJob> logger)
        : base(config.Duration, config.ForceExecuteInStartJob)
    {
        _logger = logger;
        this.Job_Executed += SampleJob_Job_Executed;
    }

    private void SampleJob_Job_Executed(object? sender, IRonixaJobExecuted e)
    {
        if (e.Status.State == RonixaJobExecutionState.Done)
            _logger.LogInformation($"{e.Information.Title} completed in {e.Status.Duration?.TotalSeconds ?? 0}s");

        if (e.Status.State == RonixaJobExecutionState.Error)
            _logger.LogError(e.Status.Exception, $"{e.Information.Title} failed after {e.Status.Duration?.TotalSeconds ?? 0}s");
    }

    public override IRonixaJobInformation SetJobInfo() => GetJobInfo(this);

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("SampleJob is running...");
        await Task.CompletedTask;
    }
}
```

### 2. Register Services
```csharp
builder.Services.AddRonixaSchedulingJob<SampleJob>(c =>
{
    c.Duration = TimeSpan.FromSeconds(10);
    c.ForceExecuteInStartJob = true;
});

builder.Services.AddRonixaJobManagerService();
```

### 3. Control Jobs via API
```csharp
[ApiController]
[Route("jobs")]
public class JobController : ControllerBase
{
    private readonly IRonixaJobManagerService _manager;

    public JobController(IRonixaJobManagerService manager) => _manager = manager;

    [HttpGet("start/{id}")]
    public IActionResult Start(Guid id) => Ok(_manager.StartJob(id));

    [HttpGet("pause/{id}")]
    public IActionResult Pause(Guid id) => Ok(_manager.PauseJob(id));

    [HttpGet("stop/{id}")]
    public IActionResult Stop(Guid id) => Ok(_manager.StopJob(id));

    [HttpGet("status/{id}")]
    public IActionResult Status(Guid id) => Ok(_manager.GetJobStatus(id));
}
```

---

## 🌈 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request.  
You can also simply open an issue with the tag `"enhancement"`.

1. Fork the Project  
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)  
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)  
4. Push to the Branch (`git push origin feature/AmazingFeature`)  
5. Open a Pull Request  

---

## 🌟 Support this project
If you believe this project has potential, feel free to **star this repo** ⭐ and share it with others.  
Your support helps this library grow and reach more developers!

---

## 🔐 License

Distributed under the MIT License. See `LICENSE.txt` for more information.

---

## 🩷 Follow Me!

You can connect with me on the social media and communication channels listed below:

[![LinkedIn][linkedin-shield]][linkedin-url]  [![Telegram][telegram-shield]][telegram-url]  [![WhatsApp][whatsapp-shield]][whatsapp-url]  [![Gmail][gmail-shield]][gmail-url]  ![GitHub followers](https://img.shields.io/github/followers/mh-zolfaghari)

---

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?logo=linkedin&color=555
[linkedin-url]: https://www.linkedin.com/in/ronixa/

[telegram-shield]: https://img.shields.io/badge/-Telegram-black.svg?logo=telegram&color=fff
[telegram-url]: https://t.me/DanialDotNet

[whatsapp-shield]: https://img.shields.io/badge/-WhatsApp-black.svg?logo=whatsapp&color=fff
[whatsapp-url]: https://wa.me/989389043224

[gmail-shield]: https://img.shields.io/badge/-Gmail-black.svg?logo=gmail&color=fff
[gmail-url]: mailto:personal.mhz@gmail.com