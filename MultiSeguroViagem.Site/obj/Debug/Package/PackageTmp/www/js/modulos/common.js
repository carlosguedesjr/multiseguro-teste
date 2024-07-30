obtemParametroUri = (nomeParametro) => {

    return decodeURIComponent((new RegExp(`[?|&]${nomeParametro}=([^&;]+?)(&|#|;|$)`).exec(location.search)
           || [null, ""])[1].replace(/\+/g, "%20"))
           || null;
};

obtemParametroUriMvc = (index) => {

    return window.location.pathname.split("/")[index];
};

isMobile = (userAgent) => {

    return /Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(userAgent);
};

setaUtmSource = () => {
  const utmSource = obtemParametroUri('utm_source');

  if (utmSource != null)
      setLocalStorage('utmSource', utmSource);
};

setaCupomAfiliado = () => {
    const cupom = obtemParametroUri('cpaff') || obtemParametroUri('Cpaff');
    
    if (cupom != null){
        window.sessionStorage.setItem('cpaff', cupom);
    }
};

validaViajantesCarrinho = (url) => {
    let  carrinho = getParseSession("carrinho");
    let planosQueFaltamViajante = [];

    carrinho.map((item) => (item.QuantidadeViajantes == 0) ? planosQueFaltamViajante.push(item.Planos.Nome) : false);

    if (planosQueFaltamViajante.length > 0)
    {
        swal("Oops!",
            `O(s) plano(s) <b>${planosQueFaltamViajante.join(", ")}</b> estão sem viajantes!<br>Edite ou remova do seu carrinho de compras.`,
            "warning");

        return false;

    } else {

        if (url != undefined)
            window.location.href = url;

        return true;
    }
};
validaDataSemana = (dataIda) => {

    let data = new Date();
    data.setHours(0, 0, 0, 0);

    let diasSemana = ["Domingo", "Segunda", "Terca", "Quarta", "Quinta", "Sexta", "Sabado"];

    let dataSplit = dataIda.split("/");

    let diaIda = new Date(dataSplit[2], dataSplit[1] - 1, dataSplit[0]);

    let diaSemanaHoje = diasSemana[data.getDay()];
    
    let quantidadeProxDia = 2;

    switch (diaSemanaHoje) {

        case "Sexta":
            quantidadeProxDia = 4;
            break;

        case "Sabado":
            quantidadeProxDia = 3;
            break;
    }
    
    data.setDate(data.getDate() + quantidadeProxDia);

    if (diaIda < data) {

        swal("Oops", "A sua viagem está muito próxima! Devido ao prazo padrão de análise de perfil das seguradoras, sua emissão online não estará disponível. Entre em contato direto com nossa equipe para emissão em tempo real.", "warning");
        return false;
    }

    return true;
};


formataDataMobile = (data) => {
    let dia = data.getUTCDate();
    let mes = data.getUTCMonth() + 1;

    return (dia < 10 ? `0${dia}` : dia) + "/" + (mes < 10 ? `0${mes}` : mes) + "/" + data.getUTCFullYear();
};

//========================================================== STORAGE
getParseSession = (chave) => {
    return JSON.parse(window.sessionStorage.getItem(chave));
};
getSession = (chave) => {
    return window.sessionStorage.getItem(chave);
};
setSession = (chave, valor) => {
    window.sessionStorage.setItem(chave, valor);
};
removeSession = (chave) => {
    window.sessionStorage.removeItem(chave);
};
getLocalStorage = (chave) => {
    return window.localStorage.getItem(chave);
};
getParseLocalStorage = (chave) => {
    return JSON.parse(window.localStorage.getItem(chave));
};
setLocalStorage = (chave, valor) => {
    window.localStorage.setItem(chave, valor);
};
removeLocalStorage = (chave) => {
    window.localStorage.removeItem(chave);
};
//========================================================== FIM STORAGE


//========================================================== MASCARAS
$(document).ready(function() {

    if ($().mask == undefined) return;

    (mascaraCep = () => $('input[mask-cep="true"]').mask("00000-000") )();

    (mascaraTelefone = () => {

        const options = {
            onKeyPress: (tel, ev, el, op) => {
                const masks = ["(00) 00000-0000", "(00) 0000-00009"],
                      mask = tel.length > 14 ? masks[0] : masks[1];
                el.mask(mask, op);
            }
        };

        $('input[mask-telefone="true"]').mask("(00) 0000-00000", options);

    })();

    (mascaraDocumento = () => {
        const options = {
            onKeyPress: (cpf, ev, el, op) => {
                const masks = ["000.000.000-000", "00.000.000/0000-00"],
                      mask = cpf.length < 15 ? masks[0] : masks[1];
                el.mask(mask, op);
            }
        };

        $('input[mask-documento="true"]').mask("000.000.000-000", options);
    })();

    (mascaraCpf = () => $('input[mask-cpf="true"]').mask("000.000.000-00", {reverse: true}) )();

    (mascaraDataNascimento = () => $('input[mask-data-nascimento="true"]').mask("00/00/0000") )();

    (mascaraCartaoCredito = () => $('input[mask-cartao-credito="true"]').mask("0000 0000 0000 0099", {reverse: true}) )();

    (mascaraAnoValidadeCredito = () => $('input[mask-ano-validade="true"]').mask("00/0099") )();
    
    (mascaraCodigoSegurancaCredito = () => $('input[mask-codigo-seguranca="true"]').mask("000") )();
});
//========================================================== FIM MASCARAS

converteDataMobile = (data) => {
    var lang = window.navigator.userLanguage || window.navigator.language;
    return ("0" + data.getDate()).slice(-2) + "/" + ("0" + (data.getMonth() + 1)).slice(-2) + "/" + data.getFullYear();
};