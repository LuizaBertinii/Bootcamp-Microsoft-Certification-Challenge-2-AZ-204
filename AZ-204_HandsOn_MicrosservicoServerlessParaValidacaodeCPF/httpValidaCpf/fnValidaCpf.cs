using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace httpValidaCpf
{
    public static class fnValidaCpf
    {
        [FunctionName("fnValidaCpf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Iniciando a validação do CPF.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if(data == null){
                return new BadRequestObjectResult("Por favor, insira um CPF.");
            }

            string cpf = data?.cpf;

            if(!ValidarCPF(cpf)){
                return new BadRequestObjectResult("CPF inválido.");
            }

            string responseMessage = "CPF válido.";

            return new OkObjectResult(responseMessage);
        }

        public static bool ValidarCPF(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = Regex.Replace(cpf, "[^0-9]", "");

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (cpf[i] - '0') * (10 - i);
            }
            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            // Verifica o primeiro dígito verificador
            if (primeiroDigito != (cpf[9] - '0'))
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (cpf[i] - '0') * (11 - i);
            }
            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            // Verifica o segundo dígito verificador
            if (segundoDigito != (cpf[10] - '0'))
                return false;

            return true;
        }
    }
}