using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using TTS = IBM.Watson.TextToSpeech.v1;

namespace TextToSpeechAPI.Services
{
    // Serviço responsável por encapsular a lógica de uso da API IBM Watson Text to Speech
    public class TextToSpeechService
    {
        private readonly TTS.TextToSpeechService _textToSpeechService;
        private readonly string _apiKey;
        private readonly string _url;

        // Inicializa o serviço com as credenciais e configura a URL da API
        public TextToSpeechService(IConfiguration config)
        {
            _apiKey = config["IBMTextToSpeechCredentials:ApiKey"];
            _url = config["IBMTextToSpeechCredentials:Url"];

            var authenticator = new IamAuthenticator(apikey: _apiKey);
            _textToSpeechService = new TTS.TextToSpeechService(authenticator);
            _textToSpeechService.SetServiceUrl(_url);
        }

        // Converte texto para áudio, retornando um array de bytes contendo o .wav
        public byte[] ConvertTextToSpeech(string text, string voice)
        {
            if (!IsTextValid(text))
                throw new ArgumentException("Texto inválido para conversão.");

            var result = CallWatsonSynthesisAPI(text, voice);

            if (result?.Result != null)
            {
                using var memoryStream = new MemoryStream();
                result.Result.WriteTo(memoryStream);
                return memoryStream.ToArray();
            }

            throw new InvalidOperationException("A conversão falhou: resposta da API está vazia.");
        }

        // Encapsula a chamada direta à API do IBM Watson
        private DetailedResponse<MemoryStream> CallWatsonSynthesisAPI(string text, string voice)
        {
            return _textToSpeechService.Synthesize(
                text: text,
                accept: "audio/wav",
                voice: voice
            );
        }

        // Valida se o texto contém conteúdo útil para conversão
        private bool IsTextValid(string text)
        {
            return !string.IsNullOrWhiteSpace(text)
                && System.Text.RegularExpressions.Regex.IsMatch(text, @"[a-zA-Z0-9]");
        }
    }
}
