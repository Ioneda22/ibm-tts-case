using Microsoft.AspNetCore.Mvc;
using TextToSpeechAPI.Services;
using TextToSpeechAPI.Models;

namespace TextToSpeechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Define a rota base como "api/texttospeech"
    public class TextToSpeechController : ControllerBase
    {
        private readonly TextToSpeechService _ttsService;

        // Injeta o serviço de TTS no controller
        public TextToSpeechController(TextToSpeechService ttsService)
        {
            _ttsService = ttsService;
        }

        [HttpPost("synthesize-pt-BR")] // Rota POST que recebe texto e retorna um áudio em português do Brasil
        public IActionResult ConvertText([FromBody] TextRequest request)
        {
            try
            {
                // Converte o texto em áudio usando a voz brasileira Isabela
                var audioBytes = _ttsService.ConvertTextToSpeech(request.Text, "pt-BR_IsabelaV3Voice");

                // Retorna o áudio como arquivo .wav
                return File(audioBytes, "audio/wav", "speech.wav");
            }
            catch (ArgumentException ex)
            {
                // Retorna erro 400 se o texto for inválido
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Retorna erro 500 se ocorrer falha geral na conversão
                return StatusCode(500, $"Erro ao converter texto em fala: {ex.Message}");
            }
        }
    }
}
