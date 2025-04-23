using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using TTS = IBM.Watson.TextToSpeech.v1;

namespace TextToSpeechAPI.Services
{
    public class TextToSpeechService // Classe Serviço: encapsula uma lógica de negócio específica, nesse caso, a lógica do TTS
    {
        private readonly TTS.TextToSpeechService _textToSpeechService; // Atributo do serviço do IBM Watson TTS
        private readonly string _apiKey; // Atributo referente a chave da API
        private readonly string _url; // Atributo referente a url da API

        public TextToSpeechService(IConfiguration config)
        {
            _apiKey = config["IBMTextToSpeechCredentials:ApiKey"];
            _url = config["IBMTextToSpeechCredentials:Url"];

            IamAuthenticator authenticator = new IamAuthenticator(apikey: _apiKey); // Autenticação das credenciais da API
            _textToSpeechService = new TTS.TextToSpeechService(authenticator); // Instanciação do serviço do IBM Watson TTS
            _textToSpeechService.SetServiceUrl(_url);
        }

        public byte[] ConvertTextToSpeech(string text, string voice)
        {
            DetailedResponse<MemoryStream> result = SynthesizeTextToSpeech(text, voice); //

            if (result != null && result.Result != null) // Verifica se a resposta não é nula e se contém um stream de áudio válido
            {
                using (MemoryStream memoryStream = new MemoryStream()) // Cria um novo MemoryStream para armazenar os dados do áudio
                {
                    result.Result.WriteTo(memoryStream); // Copia o conteúdo do stream retornado para o novo MemoryStream

                    return memoryStream.ToArray(); // Converte o conteúdo do MemoryStream para um array de bytes e retorna
                }
            }

            return null;
        }

        private DetailedResponse<MemoryStream> SynthesizeTextToSpeech(string text, string voice) // Função que chama o método Synthesize, do serviço do IBM Watson
        {
            return _textToSpeechService.Synthesize(
                text: text,
                accept: "audio/wav",
                voice: voice
            );
        }
    }
}