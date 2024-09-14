# llm-local-csharp

## Software Installation Requirements

1. At least [.NET 8](https://dotnet.microsoft.com/en-us/download) ➜ should be able to run ```dotnet``` CLI command
2. [Semantic Kernel NuGet package](https://www.nuget.org/packages/Microsoft.SemanticKernel) ➜ you don't need to download directly, but in case the one in the project gets outdated you can go get a new one using ```dotnet add package Microsoft.SemanticKernel --version latest```
3. [git](https://git-scm.com/downloads) ➜ can be used directly, but we will use through GitHub CLI
4. [GitHub CLI](https://cli.github.com/) ➜ so you can run CLI commands like ```gh repo clone devpartners/llm-local-csharp```
5. [ollama](https://www.ollama.com/download) ➜ tool for downloading and running SLM (small language models) on your laptop or in a VM

## Running ollama

```ollama serve``` ➜ run the server (unneccessary if you directly use ```ollama run <model>```)

```ollama list``` ➜ which models do I have downloaded for use locally?

```ollama run gemma2``` ➜ run the server with this model (and download the model if necessary)

## Running this project

### Terminal/Console/PowerShell window A

```bash
ollama run gemma2
```

There may be a long startup period if the gemma2 model needs to be downloaded. Once the above starts running, leave it alone and go to a second window B.

### Terminal/Console/PowerShell window B

```bash
gh repo clone devpartners/llm-local-csharp
cd llm-local-csharp
dotnet run
```

## Logs

```ollama``` logs found here (on MacOS at least): /Users/billdev/.ollama/logs
