using Microsoft.AspNetCore.Mvc;
using TextToSpeechAPI.Services;
using TextToSpeechAPI.Models;

namespace TextToSpeechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextToSpeechController : ControllerBase
    {
        private readonly TextToSpeechService _ttsService;

        public TextToSpeechController(TextToSpeechService ttsService)
        {
            _ttsService = ttsService;
        }

        [HttpPost("synthesize-pt-BR")]
        public IActionResult ConvertText([FromBody] TextRequest request)
        {
            if (!IsTextValid(request.Text))
                return BadRequest("Text cannot be empty.");

            try
            {
                var audioBytes = _ttsService.ConvertTextToSpeech(request.Text, "pt-BR_IsabelaV3Voice");

                if (audioBytes != null)
                    return File(audioBytes, "audio/wav", "speech.wav");

                return StatusCode(500, "Audio could not be generated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error converting text to speech: {ex.Message}");
            }
        }

        private bool IsTextValid(string text)
        {
            return (!string.IsNullOrWhiteSpace(text)
                && System.Text.RegularExpressions.Regex.IsMatch(text, @"[a-zA-Z0-9]"));
        }
    }
}
