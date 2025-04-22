# DNN Imageflow v1.12.0

## Referenced Dependencies (with Transitive Dependency Tree)  
_Targets: .NET Standard 2.0 / .NET Framework 4.7.2_

- **Imageflow.NativeRuntime.win-arm64** (2.1.0-rc11)
- **Imageflow.NativeRuntime.win-x86** (2.1.0-rc11)
- **Imageflow.NativeRuntime.win-x86_64** (2.1.0-rc11)
- **Imageflow.Net** (0.14.0-rc01)
  - Microsoft.IO.RecyclableMemoryStream (>= 3.0.1, < 4.0.0)
  - System.Buffers (>= 4.6.0)
  - System.Memory (>= 4.5.5)
    - System.Buffers (>= 4.6.0)
    - System.Numerics.Vectors (>= 4.5.0)
    - System.Runtime.CompilerServices.Unsafe (>= 6.0.0)
  - System.Text.Json (>= 6.0.11)
      - Microsoft.Bcl.AsyncInterfaces (>= 6.0.0)
      - System.Buffers (>= 4.6.0)
      - System.Memory (>= 4.5.5)
      - System.Numerics.Vectors (>= 4.5.0)
      - System.Runtime.CompilerServices.Unsafe (>= 6.0.0)
      - System.Text.Encodings.Web (>= 6.0.1)
      - System.Threading.Tasks.Extensions (>= 4.5.4)
- **Imazen.HybridCache** (0.8.3)
  - Imazen.Common (>= 0.8.3)
    - Microsoft.Extensions.Hosting.Abstractions (>= 2.2.0)
        - Microsoft.Extensions.Configuration.Abstractions (>= 2.2.0)
          - Microsoft.Extensions.Primitives (>= 2.2.0)
        - Microsoft.Extensions.DependencyInjection.Abstractions (>= 2.2.0)
        - Microsoft.Extensions.FileProviders.Abstractions (>= 2.2.0)
        - Microsoft.Extensions.Logging.Abstractions (>= 2.2.0)

---

## Dependency Assembly Details

| DLL Filename                                    | Assembly Version | Target Framework     | File Version   | Notes           |
|-------------------------------------------------|------------------|----------------------|----------------|-----------------|
| **ToSic.Imageflow.Dnn.dll**                     | 1.12.0.0         | .NET Framework 4.7.2 | 01.12.00       | `bin`           |
| **Imageflow.Net.dll**                           | 0.14.0.0         | .NET Standard 2.0    | 0.14.0.0       | `bin`           |
| **Imazen.Common.dll**                           | 0.8.3.0          | .NET Framework 4.7.2 | 0.8.3.0        | `bin`           |
| **Imazen.HybridCache.dll**                      | 0.8.3.0          | .NET Framework 4.7.2 | 0.8.3.0        | `bin`           |
| Microsoft.Bcl.AsyncInterfaces.dll               | 8.0.0.0          | .NET Framework 4.6.2 | 8.0.23.53103   | `bin` DNN10 has 8.0.0.0 with bindings |
| Microsoft.Extensions.Configuration.Abstractions.dll | 2.2.0.0      | .NET Standard 2.0    | 2.2.0.18315    | `bin/imageflow` |
| Microsoft.Extensions.DependencyInjection.Abstractions.dll | 2.2.0.0 | .NET Standard 2.0   | 2.2.0.18315    | `bin/imageflow` DNN<10 has 2.1.1.0 without bindings; DNN10 has 8.0.0.0 with bindings |
| Microsoft.Extensions.FileProviders.Abstractions.dll | 2.2.0.0      | .NET Standard 2.0    | 2.2.0.18315    | `bin/imageflow` |
| Microsoft.Extensions.Hosting.Abstractions.dll   | 2.2.0.0          | .NET Standard 2.0    | 2.2.0.18316    | `bin/imageflow` |
| Microsoft.Extensions.Logging.Abstractions.dll   | 2.2.0.0          | .NET Standard 2.0    | 2.2.0.18315    | `bin/imageflow` |
| Microsoft.Extensions.Primitives.dll             | 2.2.0.0          | .NET Standard 2.0    | 2.2.0.18315    | `bin/imageflow` |
| Microsoft.IO.RecyclableMemoryStream.dll         | 3.0.1.0          | .NET Standard 2.0    | 3.0.1.0        | `bin/imageflow` |
| System.Buffers.dll                              | 4.0.4.0          | .NET Framework 4.6.2 | 4.600.24.56208 | `bin` DNN<10 has 4.0.3.0 with bindings; DNN10 has 4.0.4.0 with bindings |
| System.Memory.dll                               | 4.0.1.2          | .NET Framework 4.6.2 | 4.6.31308.01   | `bin` DNN<10 has 4.0.1.2 with bindings; DNN10 has 4.0.2.0 with bindings |
| System.Numerics.Vectors.dll                     | 4.1.4.0          | .NET Framework 4.6.2 | 4.6.26515.06   | `bin` DNN<10 has 4.1.4.0 without bindings; DNN10 has 4.1.5.0 with bindings |
| System.Runtime.CompilerServices.Unsafe.dll      | 6.0.0.0          | .NET Framework 4.0   | 6.0.21.52210   | `bin` DNN10 has 6.0.1.0 with bindings |
| System.Text.Encodings.Web.dll                   | 6.0.0.1          | .NET Framework 4.6.1 | 6.0.3624.51421 | `bin` DNN10 has 9.0.0.2 with bindings |
| System.Text.Json.dll                            | 6.0.0.11         | .NET Framework 4.6.1 | 6.0.3624.51421 | `bin` DNN10 has 8.0.0.5 with bindings |
| System.Threading.Tasks.Extensions.dll           | 4.2.0.1          | .NET Framework 4.0   | 4.6.28619.01   | `bin` DNN has 4.2.0.1 with bindings |
| System.ValueTuple.dll                           | 4.0.3.0          | .NET Framework 4.0   | 4.6.26515.06   | `bin` DNN10 has 4.0.3.0 with bindings |

### Notes:
- `Microsoft.Bcl.AsyncInterfaces.dll` updated to `6.0.0.0` to `8.0.0.0` avoid potential issue with assembly bindings as part of DNN 10.0.0.0 upgrade
---
