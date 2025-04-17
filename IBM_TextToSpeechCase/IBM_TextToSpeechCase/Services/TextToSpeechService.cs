using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using TTS = IBM.Watson.TextToSpeech.v1;

namespace TextToSpeechAPI.Services
{
    public class TextToSpeechService
    {
        private readonly TTS.TextToSpeechService _textToSpeechService;
        private readonly string _apiKey;
        private readonly string _url;

        public TextToSpeechService(IConfiguration config)
        {
            _apiKey = config["IBMTextToSpeechCredentials:ApiKey"];
            _url = config["IBMTextToSpeechCredentials:Url"];

            IamAuthenticator authenticator = new IamAuthenticator(apikey: _apiKey);
            _textToSpeechService = new TTS.TextToSpeechService(authenticator);
            _textToSpeechService.SetServiceUrl(_url);
        }

        public byte[] ConvertTextToSpeech(string text, string voice)
        {
            DetailedResponse<MemoryStream> result = SynthesizeTextToSpeech(text, voice);

            if (result != null && result.Result != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    result.Result.WriteTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            return null;
        }

        private DetailedResponse<MemoryStream> SynthesizeTextToSpeech(string text, string voice)
        {
            return _textToSpeechService.Synthesize(
                text: text,
                accept: "audio/wav",
                voice: voice
            );
        }
    }
}