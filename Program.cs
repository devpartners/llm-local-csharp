using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
// using Microsoft.SemanticKernel.ChatCompletion;

// var modelId = "phi3";
// var modelId = "llama3.1";
var modelId = "gemma2";
// var modelId = "notfound3.14159265"; // diagnose with "ollama list" and find more at https://ollama.com/library
string? apiKey = null;

var endpoint = new Uri("http://localhost:11434");
var maxTokens = 1000;
maxTokens = 750;

var builder = Kernel.CreateBuilder();

#pragma warning disable SKEXP0010
builder.AddOpenAIChatCompletion(modelId, endpoint, apiKey);
#pragma warning restore SKEXP0010

var kernel = builder.Build();

var prompt = @"{{$input}}

Concisely summarize this text for an intelligent but non-technical, non-expert audience.";

var summarize = kernel.CreateFunctionFromPrompt(prompt, executionSettings: new OpenAIPromptExecutionSettings { MaxTokens = maxTokens });

var sentiment_analysis_prompt = """
{{$input}}

Determine Sentiment as one of: Positive, Negative, Neutral. Return no explaination, just the one-word summary of the sentiment - one of the words 'Positive', 'Neutral', or 'Negative'.
""";

var analyze_sentiment = kernel.CreateFunctionFromPrompt(sentiment_analysis_prompt, executionSettings: new OpenAIPromptExecutionSettings { MaxTokens = maxTokens });

var recall_facts_prompt = """
{{$input}}
""";

var recall_facts = kernel.CreateFunctionFromPrompt(recall_facts_prompt, executionSettings: new OpenAIPromptExecutionSettings { MaxTokens = maxTokens });


// https://en.wikipedia.org/wiki/Nikola_Tesla
var text = """
Nikola Tesla was a Serbian-American engineer, futurist, and inventor. He is known for his contributions to the design of the modern alternating current (AC) electricity supply system.

Born and raised in the Austrian Empire, Tesla first studied engineering and physics in the 1870s without receiving a degree. He then gained practical experience in the early 1880s working in telephony and at Continental Edison in the new electric power industry. In 1884 he immigrated to the United States, where he became a naturalized citizen. He worked for a short time at the Edison Machine Works in New York City before he struck out on his own. With the help of partners to finance and market his ideas, Tesla set up laboratories and companies in New York to develop a range of electrical and mechanical devices. His AC induction motor and related polyphase AC patents, licensed by Westinghouse Electric in 1888, earned him a considerable amount of money and became the cornerstone of the polyphase system which that company eventually marketed.

Attempting to develop inventions he could patent and market, Tesla conducted a range of experiments with mechanical oscillators/generators, electrical discharge tubes, and early X-ray imaging. He also built a wirelessly controlled boat, one of the first ever exhibited. Tesla became well known as an inventor and demonstrated his achievements to celebrities and wealthy patrons at his lab and was noted for his showmanship at public lectures. Throughout the 1890s, Tesla pursued his ideas for wireless lighting and worldwide wireless electric power distribution in his high-voltage, high-frequency power experiments in New York and Colorado Springs. In 1893, he made pronouncements on the possibility of wireless communication with his devices. Tesla tried to put these ideas to practical use in his unfinished Wardenclyffe Tower project, an intercontinental wireless communication and power transmitter, but ran out of funding before he could complete it.

After Wardenclyffe, Tesla experimented with a series of inventions in the 1910s and 1920s with varying degrees of success. Having spent most of his money, Tesla lived in a series of New York hotels, leaving behind unpaid bills. He died in New York City in January 1943. Tesla's work fell into relative obscurity following his death until 1960 when the General Conference on Weights and Measures named the International System of Units (SI) measurement of magnetic flux density the tesla in his honor. There has been a resurgence in popular interest in Tesla since the 1990s.
""";

System.Diagnostics.Stopwatch sw = new();
TimeSpan elapsed;

Console.WriteLine($"\n------------ Sentiment Analysis (using model {modelId} with up to {maxTokens} tokens) ------------");
sw.Restart();
var fr = await kernel.InvokeAsync(analyze_sentiment, new() { ["input"] = text });
var sentiment = fr.ToString().Trim();
elapsed = sw.Elapsed;
Console.WriteLine(sentiment);
Console.WriteLine($"Elapsed: {elapsed.TotalSeconds} seconds");

Console.WriteLine($"\n------------ Summarized (using model {modelId} with up to {maxTokens} tokens) ------------");
sw.Restart();
var summary = ((await kernel.InvokeAsync(summarize, new() { ["input"] = text })).ToString().Trim());
elapsed = sw.Elapsed;;
Console.WriteLine(summary);
Console.WriteLine($"Elapsed: {elapsed.TotalSeconds} seconds");

var contextFreeQuery = "What is the value of pi?";
contextFreeQuery = "Who is Tesla?";

Console.WriteLine($"\n------------ No context, just from memory = '{contextFreeQuery}' (using model {modelId} with up to {maxTokens} tokens) ------------");
sw.Restart();
var response = ((await kernel.InvokeAsync(recall_facts, new() { ["input"] = contextFreeQuery })).ToString().Trim());
elapsed = sw.Elapsed;;
Console.WriteLine(response);
Console.WriteLine($"Elapsed: {elapsed.TotalSeconds} seconds");
