# DNN Core Shared Dependencies by Version
System.Runtime.CompilerServices.Unsafe
This document summarizes the shared .NET dependencies for various DNN versions, both immediately after unzipping and after installation.  
For each dependency, the following columns are used:

| DLL Name | Assembly Version | Target | File Version | Notes |
|----------|------------------|--------|--------------|-------|

If a dependency has an `assemblyBinding` entry, the full XML is shown in a separate code block directly below the table row.

---

## DNN 9.6.1

### Unzipped

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |

---

### Installed

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |

---

## DNN 9.11.2

### Unzipped

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |

---

### Installed

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |
| System.Buffers                                   | 4.0.3.0          | .Net Framework v4.0   | 4.6.28619.01  | see below |

```xml
<!-- System.Buffers -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.3.0" />
</dependentAssembly>
```

---

## DNN 9.13.8

### Unzipped

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |
| System.Numerics.Vectors                          | 4.1.4.0          | .Net Framework v4.0   | 4.6.26515.06  | -     |
| System.Threading.Tasks.Extensions                | 4.2.0.1          | .Net Framework v4.0   | 4.6.28619.01  | -     |

---

### Installed

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |
| System.Buffers                                   | 4.0.3.0          | .Net Framework v4.0   | 4.6.28619.01  | see below |
| System.Memory                                    | 4.0.1.2          | .Net Framework v4.0   | 4.6.31308.01  | see below |
| System.Numerics.Vectors                          | 4.1.4.0          | .Net Framework v4.0   | 4.6.26515.06  | -     |
| System.Threading.Tasks.Extensions                | 4.2.0.1          | .Net Framework v4.0   | 4.6.28619.01  | see below |

```xml
<!-- System.Buffers -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.3.0" />
</dependentAssembly>

<!-- System.Memory -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Memory" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.1.2" />
</dependentAssembly>

<!-- System.Threading.Tasks.Extensions -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Threading.Tasks.Extensions" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.2.0.1" />
</dependentAssembly>
```

---

## DNN 9.13.9

### Unzipped

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |
| System.Numerics.Vectors                          | 4.1.4.0          | .Net Framework v4.0   | 4.6.26515.06  | -     |
| System.Threading.Tasks.Extensions                | 4.2.0.1          | .Net Framework v4.0   | 4.6.28619.01  | -     |

---

### Installed

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 2.1.1.0        | .NETStandard v2.0     | 2.1.1.18157   | -     |
| Microsoft.Extensions.DependencyInjection         | 2.1.1.0          | .Net Framework v4.6.1 | 2.1.1.18157   | -     |
| System.Buffers                                   | 4.0.3.0          | .Net Framework v4.0   | 4.6.28619.01  | see below |
| System.Memory                                    | 4.0.1.2          | .Net Framework v4.0   | 4.6.31308.01  | see below |
| System.Numerics.Vectors                          | 4.1.4.0          | .Net Framework v4.0   | 4.6.26515.06  | see below |
| System.Threading.Tasks.Extensions                | 4.2.0.1          | .Net Framework v4.0   | 4.6.28619.01  | see below |

```xml
<!-- System.Buffers -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.3.0" />
</dependentAssembly>

<!-- System.Memory -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Memory" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.1.2" />
</dependentAssembly>

<!-- System.Threading.Tasks.Extensions -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Threading.Tasks.Extensions" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.2.0.1" />
</dependentAssembly>
```

---

## DNN 10.0.0

### Unzipped

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Bcl.AsyncInterfaces                    | 8.0.0.0          | .Net Framework v4.6.2 | 8.0.23.53103  | -     |
| Microsoft.Extensions.DependencyInjection.Abstractions | 8.0.0.0        | .Net Framework v4.6.2 | 8.0.23.53103  | -     |
| Microsoft.Extensions.DependencyInjection         | 8.0.0.0          | .Net Framework v4.6.2 | 8.0.23.53103  | -     |
| System.Threading.Tasks.Extensions                | 4.2.0.1          | .Net Framework v4.0   | 4.6.28619.01  | -     |

---

### Installed

| DLL Name                                         | Assembly Version | Target                | File Version   | Notes |
|--------------------------------------------------|------------------|-----------------------|---------------|-------|
| Microsoft.Bcl.AsyncInterfaces                    | 8.0.0.0          | .Net Framework v4.6.2 | 8.0.23.53103  | see below |
| Microsoft.Extensions.DependencyInjection.Abstractions | 8.0.0.0        | .Net Framework v4.6.2 | 8.0.23.53103  | see below |
| Microsoft.Extensions.DependencyInjection         | 8.0.0.0          | .Net Framework v4.6.2 | 8.0.23.53103  | see below |
| System.Buffers                                   | 4.0.4.0          | .Net Framework v4.6.2 | 4.600.24.56208| see below |
| System.Memory                                    | 4.0.2.0          | .Net Framework v4.6.2 | 4.600.24.56208| see below |
| System.Numerics.Vectors                          | 4.1.5.0          | .Net Framework v4.6.2 | 4.600.24.56208| see below |
| System.Runtime.CompilerServices.Unsafe           | 6.0.1.0          | .Net Framework v4.0   | 6.100.24.56208| see below |
| System.Text.Encodings.Web                        | 9.0.0.2          | .Net Framework v4.6.2 | 9.0.225.6610  | see below |
| System.Text.Json                                 | 8.0.0.5          | .Net Framework v4.6.2 | 8.0.1024.46610| see below |
| System.Threading.Tasks.Extensions                | 4.2.0.1          | .Net Framework v4.0   | 4.6.28619.01  | see below |
| System.ValueTuple                                | 4.0.3.0          | .Net Framework v4.0   | 4.6.26515.06  | see below |

```xml
<!-- Microsoft.Bcl.AsyncInterfaces -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="Microsoft.Bcl.AsyncInterfaces" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="8.0.0.0" />
</dependentAssembly>

<!-- Microsoft.Extensions.DependencyInjection.Abstractions -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="Microsoft.Extensions.DependencyInjection.Abstractions" publicKeyToken="adb9793829ddae60" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="8.0.0.0" />
</dependentAssembly>

<!-- Microsoft.Extensions.DependencyInjection -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="Microsoft.Extensions.DependencyInjection" publicKeyToken="adb9793829ddae60" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="8.0.0.0" />
</dependentAssembly>

<!-- System.Buffers -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.4.0" />
</dependentAssembly>

<!-- System.Memory -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Memory" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.2.0" />
</dependentAssembly>

<!-- System.Numerics.Vectors -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Numerics.Vectors" publicKeyToken="b03f5f7f11d50a3a" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.1.5.0" />
</dependentAssembly>

<!-- SSystem.Runtime.CompilerServices.Unsafe -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Runtime.CompilerServices.Unsafe" publicKeyToken="b03f5f7f11d50a3a" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="6.0.1.0" />
</dependentAssembly>

<!-- System.Text.Encodings.Web -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Text.Encodings.Web" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="9.0.0.2" />
</dependentAssembly>

<!-- System.Text.Json -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Text.Json" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="8.0.0.5" />
</dependentAssembly>

<!-- System.Threading.Tasks.Extensions -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.Threading.Tasks.Extensions" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.2.0.1" />
</dependentAssembly>

<!-- System.ValueTuple -->
<dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity name="System.ValueTuple" publicKeyToken="cc7b13ffcd2ddd51" />
  <bindingRedirect oldVersion="0.0.0.0-32767.32767.32767.32767" newVersion="4.0.3.0" />
</dependentAssembly>
```

---

**Legend:**  
- `-` in Notes means no special `assemblyBinding` in web.config.  
- If "see below", the full `assemblyBinding` XML is shown after the table.
