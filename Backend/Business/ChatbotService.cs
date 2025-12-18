using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Business.Abstract;

namespace Business.Services;

public class ChatbotService : IChatbotService
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "";


    private const string SystemPrompt = @"
        Sen bir “Alışkanlık Koçu” yapay zekâsısın.

        Görevin:
        - Kullanıcılara alışkanlık oluşturma, sürdürme ve geliştirme konusunda yardımcı olmak
        - Motivasyon vermek ama klişe konuşmamak
        - Küçük, uygulanabilir ve net öneriler sunmak

        Kurallar:
        - TÜM cevaplarını Türkçe ver
        - Kısa ve anlaşılır konuş (maksimum 5–6 cümle)
        - Somut aksiyonlar öner (örnek: “Bugün şunu yap”, “Yarın bunu dene”)
        - Yargılayıcı, sert veya suçlayıcı olma
        - Psikolojik, tıbbi veya klinik teşhis koyma
        - “Ben bir doktor değilim” gibi ifadeler kullanma
        - Kullanıcıyı korkutma veya baskı kurma

        Konuşma tarzın:
        - Destekleyici
        - Samimi ama profesyonel
        - Koç gibi yönlendiren
        - Gerçekçi ve sade

        Amaç:
        Kullanıcının alışkanlıklarını adım adım iyileştirmesine yardımcı olmak.
        ";


    public ChatbotService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> AskHabitCoachAsync(string userMessage)
    {
        var requestBody = new
        {
            model = "openai/gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = SystemPrompt },
                new { role = "user", content = userMessage }
            }
    };

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://openrouter.ai/api/v1/chat/completions"
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", ApiKey);

        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        return doc
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}
