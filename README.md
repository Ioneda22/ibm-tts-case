# 🗣️ IBM Text to Speech API (.NET 8)

Este projeto é uma API REST desenvolvida em .NET 8 que utiliza o serviço **Text to Speech** da **IBM Cloud** para converter textos em áudio. O serviço recebe uma string com o texto desejado e retorna um arquivo de áudio no formato `.wav`, sintetizado com uma voz natural em português brasileiro.



## 📁 Funcionamento do projeto

O projeto segue uma arquitetura simples e bem organizada, separando responsabilidades entre partes distintas. Isso facilita a manutenção, reutilização de código e testes. As duas principais partes são:

### 1. **Serviço** – `Services/TextToSpeechService.cs`
É parte do código que contém a lógica da função que transforma um texto em um áudio.

- Contém a classe `TextToSpeechService`:
  - Dentro do construtor dessa classe fazemos a autenticação das credenciais da API utilizando o SDK do IBM Cloud
  - Temos também o método `SynthesizeTextToSpeech`, que chama o método `Synthesize` do namespace  `IBM.Watson.TextToSpeech.v1`, que sintetiza um texto (string) em áudio (wav);

### 2. **Controlador** – `Controllers/TextToSpeechController.cs`
É a parte do código que contém o endpoint principal do servidor e controla as requisições dos usuários.

- Define o endpoint `POST /api/TextToSpeech/synthesize-pt-BR`:
  - Recebe uma requisição JSON contendo o texto a ser convertido.
  - Valida o texto para garantir que não está vazio ou inválido.
  - Chama o serviço `TextToSpeechService` para gerar o áudio e retorna o arquivo `.wav` como resposta.
  - Em caso de erro, retorna uma mensagem apropriada com o código HTTP correspondente.

O diagrama abaixo ilustra resumidamente o fluxo de dados entre os componentes da aplicação, desde a requisição do usuário até a resposta com o áudio:

![Fluxo da aplicação](./images/tts-scheme.png)
