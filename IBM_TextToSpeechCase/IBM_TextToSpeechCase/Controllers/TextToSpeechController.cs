using Microsoft.AspNetCore.Mvc;
using TextToSpeechAPI.Services;
using TextToSpeechAPI.Models;

namespace TextToSpeechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Controlador: Lida com  as requisições dos usuários
    public class TextToSpeechController : ControllerBase
    {
        private readonly TextToSpeechService _ttsService; // Atributo do Service

        public TextToSpeechController(TextToSpeechService ttsService)
        {
            _ttsService = ttsService;
        }

        [HttpPost("synthesize-pt-BR")] // Endpoint POST para lidar com o TTS
        public IActionResult ConvertText([FromBody] TextRequest request)
        {
            if (!IsTextValid(request.Text)) // Verifica se o texto recebido é válido
                return BadRequest("Text cannot be empty.");

            try
            {
                var audioBytes = _ttsService.ConvertTextToSpeech(request.Text, "pt-BR_IsabelaV3Voice"); // Variável que recebe o áudio da função ConvertTextToSpeech da classe do Serviço.
                                                                                                        // Como segundo parâmetro, colocamos a voz/língua do áudio que queremos e, nesse caso,
                                                                                                        // estamos usando uma voz brasileira

                if (audioBytes != null)
                    return File(audioBytes, "audio/wav", "speech.wav");

                return StatusCode(500, "Audio could not be generated.");
            }
            catch (Exception ex) // Tratamento de exceções na conversão
            {
                return StatusCode(500, $"Error converting text to speech: {ex.Message}");
            }
        }

        private bool IsTextValid(string text) // Função que verifica se um texto é válido
        {
            return (!string.IsNullOrWhiteSpace(text) // Verifica se o texto contém somente espaços em branco
                && System.Text.RegularExpressions.Regex.IsMatch(text, @"[a-zA-Z0-9]")); // Verifica se o texto contém algum caractere válido
        }
    }
}
