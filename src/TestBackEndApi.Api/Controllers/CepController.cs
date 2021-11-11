using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestBackEndApi.Domain.Queries.Cep.Get;
using TestBackEndApi.Infrastructure.Services.Contract;

namespace TestBackEndApi.Api.Controllers
{
    [Route("api/cep")]
    [ApiController]
    public class CepController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public CepController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCepQuery query)
        {

            var urlViaCep = _configuration.GetSection("UrlViaCep").Value;

            string url = urlViaCep + query.Cep + "/json/";
            string statusResponse = null;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                statusResponse = httpWebResponse.StatusDescription;
                httpWebResponse.Close();
            }
            catch (WebException we)
            {
                statusResponse = ((HttpWebResponse)we.Response).StatusCode.ToString();
                throw new Exception("Cep informado não está correto. A mensagem de erro é: " + statusResponse);
                
            }

            var json = new WebClient().DownloadString(urlViaCep + query.Cep+"/json/");

            var resultJson = JsonConvert.DeserializeObject<CepResponse>(json);

            var mensagem = new List<MensagemResponse>();

            mensagem.Add(new MensagemResponse()
            {
                Mensagem = statusResponse
            });

            resultJson.Mensagens = mensagem;

            var jsonToXml = JsonConvert.SerializeObject(resultJson);

            //Converte para xml
            XNode xml = JsonConvert.DeserializeXNode(jsonToXml, "Root");

            resultJson.XmlGerado = xml.ToString();

            return Ok(resultJson);
        }

    }
}
