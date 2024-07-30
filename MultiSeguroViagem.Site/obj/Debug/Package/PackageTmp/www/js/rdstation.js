obtemCookieUsuario = () => {
    const cookies = document.cookie;

    if (cookies === "") return undefined;

    const cookieRd = cookies.split("rdtrk=")[1];

    if (cookieRd === undefined) return undefined;

    return JSON.parse(decodeURIComponent(cookieRd.split(";").shift())).id;
};

chamadaConversao = (datajson) => {
        $.ajax({
            method: "POST",
            url: "https://www.rdstation.com.br/api/1.3/conversions",
            data: datajson,
            async: false,
            dataType: "json"
        });
    };

conversaoRDStation = (identificador, pagamento = false) => {

    const destinos = {
        "1": "América do Norte",
        "2": "África",
        "3": "América Central",
        "4": "América do Sul",
        "5": "Ásia",
        "6": "Brasil",
        "7": "Europa",
        "8": "Oceania",
        "9": "Oriente Médio",
    };
    const destino = destinos[$("#Destino").val()];

    const formasPagamento = {
        "1": "Itau Shopline",
        "3": "Rede"
    };
    const formaPagamento = formasPagamento[$("#OperadoraPagamento").val()];

    let email = $("#Email").val();
    if (!email) {
        email = getSession("email");
    }

    let dataIda;
    if ($("#DataIda").val()) {
        dataIda = $("#DataIda").val();
    }

    let dataVolta;
    if ($("#DataVolta").val()) {
        dataVolta = $("#DataVolta").val();
    }

    let emailReferencia;
    if (pagamento && email !== getSession("email")) {
        emailReferencia = getSession("email");
    }

    let nome;
    let complemento;
    let cpf;
    let telefone;
    let cep;
    let endereco;
    let bairro;
    let numero;
    let cidade;
    let estado;
    let contatoEmergencia;
    let telefoneEmergencia;

    if(pagamento) {
        nome = $("#NomeCompleto").val();
        cpf = $("#Documento").val();
        telefone = $("#TelefonePessoal").val();
        cep = $("#Cep").val();
        endereco = $("#Endereco").val();
        bairro = $("#Bairro").val();        
        if($("#Complemento").val()){
            complemento = $("#Complemento").val();
        }        
        numero = $("#Numero").val();
        cidade = $("#Cidade").val();
        estado = $("#Estado").val();
        contatoEmergencia = $("#NomeContato").val();
        telefoneEmergencia = $("#TelefoneContato").val();
    }

    const urlOrigem = (getLocalStorage("utmSource")) ? getLocalStorage("utmSource") : "Indefinido";
    const identificadorBanner = $('#Identificador').val() || "";

    const datajson = {
        "token_rdstation": "fc4864c74381a695bb88b6a77f7d3c49",
        "identificador": identificador,
        "email": email,
        "client_id": obtemCookieUsuario(),
        "website": window.location.origin + window.location.pathname,
        "tags": [ 
            urlOrigem,
            identificadorBanner
        ],
        "traffic_source": urlOrigem,
        "c_utmz": urlOrigem,
        "nome": nome || undefined,
        "CpfCpnj": cpf || undefined,
        "telefone": telefone || undefined,
        "Cep": cep || undefined,
        "Endereco": endereco || undefined,
        "EnderecoBairro": bairro || undefined,
        "EnderecoComplemento": complemento || undefined,
        "EnderecoNumero": numero || undefined,
        "cidade": cidade || undefined,
        "estado": estado || undefined,
        "NomeEmergencia": contatoEmergencia || undefined,
        "TelefoneEmergencia": telefoneEmergencia || undefined,
        "DataIda": dataIda || undefined,
        "DataVolta": dataVolta || undefined,
        "Destino": destino || undefined,
        "EmailReferencia": emailReferencia || undefined,
        "FormaPagamento": formaPagamento || undefined
    };

    if (datajson.email) {
        chamadaConversao(datajson);
    }

    if((identificador === "Voucher" && getSession("emailPagamento")) || (identificador === "Voucher Avulso" && getSession("emailPagamento"))){
        datajson.email = getSession("emailPagamento");
        chamadaConversao(datajson);
    }    
};