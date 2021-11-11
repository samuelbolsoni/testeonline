﻿using System.Collections.Generic;

namespace TestBackEndApi.Infrastructure.Services.Contract
{
    public class CepResponse
    {
        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Localidade { get; set; }

        public List<MensagemResponse> Mensagens { get; set; }

        public string XmlGerado { get; set; }
    }

    public class MensagemResponse
    {
        public string Mensagem { get; set; }
    }
}
