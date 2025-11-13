using Mscc.GenerativeAI; 
using System.Threading.Tasks;
using System;

namespace ApiUsers.Services
{
    public class GeminiChatService
    {
        private readonly ChatSession _chatSession;
        private const string ModelName = Model.Gemini25Flash;

        private const string ContextPromptText =
           "Você é um assistente de help desk amigável e conciso. Suas respostas devem ser focadas em resolver problemas de software, hardware, redes ou de Recursos Humanos (RH). Responda em Português e evite discussões fora do tópico de suporte técnico ou de RH. Nunca utilize formatação Markdown, como negrito, itálico ou listas.";

        public GeminiChatService(string apiKey)
        {
 
            var googleAI = new GoogleAI(apiKey: apiKey);

            var systemInstructionContent = new Content(
                Role.System,
                ContextPromptText 
            );

            var generationConfig = new GenerateContentConfig
            {
                SystemInstruction = systemInstructionContent
            };


            var generativeModel = googleAI.GenerativeModel(
                ModelName,
                generationConfig
            );


            _chatSession = generativeModel.StartChat();
        }

        public async Task<string> SendMessageAsync(string message)
        {
            try
            {
                if (_chatSession == null) return "ERRO FATAL: Sessão de chat não inicializada.";

                var response = await _chatSession.SendMessage(message);

                var rawText = response.Text;

                if (!string.IsNullOrEmpty(rawText))
                {
                    rawText = rawText.Replace("**", "");  
                    rawText = rawText.Replace("*", "");   
                    rawText = rawText.Replace("###", ""); 
                    rawText = rawText.Replace("```", ""); 
                    rawText = rawText.Replace("##", "");  
                    rawText = rawText.Replace("#", "");
                    rawText = rawText.Replace("---", "");
                    rawText = rawText.Replace("-", "");
                    rawText = rawText.Replace("--", "");

                    rawText = rawText.Trim();
                }

                return rawText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO NA API GEMINI: {ex.Message}");
                return $" ERRO da API: {ex.Message}";
            }
        }
    }
}