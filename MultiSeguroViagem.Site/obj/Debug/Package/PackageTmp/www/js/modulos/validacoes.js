
// ######################################### VALIDACÕES
calculaDigitosCpf = (obj) => {
    s = obj;

    if (isNaN(s)) {
        return false;
    }

    var verify = false;
    var aux = 1;
    //verifica se os numeros são iguais
    for (var i = 0; i < s.length; i++) {
        if (aux < s.length) {
            if (s.charAt(i) != s.charAt(aux))
                verify = true;
        }
        aux++;
    }
    if (!verify) return false;

    var i;
    var c = s.substr(0, 9);
    var dv = s.substr(9, 2);
    var d1 = 0;

    for (i = 0; i < 9; i++) {

        d1 += c.charAt(i) * (10 - i);

    }

    if (d1 == 0) {

        return false;

    }

    d1 = 11 - (d1 % 11);

    if (d1 > 9) d1 = 0;

    if (dv.charAt(0) != d1) {

        return false;

    }

    d1 *= 2;

    for (i = 0; i < 9; i++) {

        d1 += c.charAt(i) * (11 - i);

    }

    d1 = 11 - (d1 % 11);

    if (d1 > 9) d1 = 0;

    if (dv.charAt(1) != d1) {

        return false;

    }

    return true;

};

calculoDigitosCnpj = (obj) => {
    
    s = obj;

    if (isNaN(s)) {

        return false;

    }

    var i;
    var c = s.substr(0, 12);
    var dv = s.substr(12, 2);
    var d1 = 0;


    for (i = 0; i < 12; i++) {

        d1 += c.charAt(11 - i) * (2 + (i % 8));

    }

    if (d1 == 0)

        return false;

    d1 = 11 - (d1 % 11);

    if (d1 > 9) d1 = 0;

    if (dv.charAt(0) != d1) {

        return false;

    }

    d1 *= 2;

    for (i = 0; i < 12; i++) {

        d1 += c.charAt(11 - i) * (2 + ((i + 1) % 8));

    }

    d1 = 11 - (d1 % 11);

    if (d1 > 9)

        d1 = 0;

    if (dv.charAt(1) != d1) {

        return false;

    }

    return true;

};

campoVazio = (obj) => {
    if ($(obj).val() === "" || $(obj).val() === null) {
        $(`span[data-valmsg-for="${$(obj).attr("name")}"]`).text($(obj).attr("data-message-empty"));
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("name")}"]`).text("");
    return true;
};

nomeCompleto = (obj) => {

    const regExPattern = /([A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ]{2,}([ A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ'`.]{0,}?)[ ]{1,}([ A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ'`]{0,}?)[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ]{2,})/;

    if (!(obj.value.match(regExPattern) && (obj.value != ""))) {

        $(`span[data-valmsg-for="${$(obj).attr("name")}"]`).text("Digite o nome completo");
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("name")}"]`).text("");
    return true;
};

data = (obj) => {

    const regExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;

    if (!(obj.value.match(regExPattern) && (obj.value != ""))) {

        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("Data inválida");
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
    return true;
};

cpf = (obj) => {

    const digitosCpf = obj.value.replace(/\D+/g, "");

    if (!calculaDigitosCpf(digitosCpf)) {

        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("CPF inválido");
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
    return true;
};

cnpj = (obj) => {

    const digitosCnpj = obj.value.replace(/\D+/g, "");

    if (!calculoDigitosCnpj(digitosCnpj)) {

        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("CNPJ inválido");
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
    return true;
};
cpfCnpj = (obj) => {

    const digitosCpfCnpj = obj.value.replace(/\D+/g, "");

    if (digitosCpfCnpj.length == 11) {

        return cpf(obj);
    } else if (digitosCpfCnpj.length >= 14) {

        return cnpj(obj);
    } else {

        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("Documento inválido");
        return false;
    }
};

email = (obj) => {

    const valido = $(obj).val()
                         .match(/(?:[a-z0-9!#$%&'*+\=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)/gi);

    if (!valido) {

        $(`span[data-valmsg-for="${$(obj).attr("name")}"]`).text("E-mail inválido");
        return false;
    }
    
    $(`span[data-valmsg-for="${$(obj).attr("name")}"]`).text("");
    return true;
};

idadeMaximaPlano = (obj) => {
    const contador = $(obj).attr("id").replace(/[^\d]/g, "");
    const idadeMaxima = parseInt($(`#hdnIdadeMaximaPlano${contador}`).val()) || 0;
    var data = new Date();
    data = data.toLocaleDateString();
    var data2 = new Date($(obj).val().split('/').reverse());
    data2 = data2.toLocaleDateString();

    var nascimento = data2.split("/");
    var dataNascimentoViajante = new Date(parseInt(nascimento[2], 10),
        parseInt(nascimento[1], 10) - 1,
        parseInt(nascimento[0], 10));

    var dataAtual = data.split("/");
    var data_atual = new Date(parseInt(dataAtual[2], 10),
        parseInt(dataAtual[1], 10) - 1,
        parseInt(dataAtual[0], 10));

    var diferenca = data_atual.getTime() - dataNascimentoViajante.getTime();
    var idade = new Date(diferenca);


    idade = Math.abs(idade.getUTCFullYear() - 1970);
   

    const idadeViajante = idade;


    let dataNascimento = new Date($(obj).val().split('/').reverse());
    dataNascimento.setHours(0, 0, 0, 0);

    if (idadeViajante > idadeMaxima || dataNascimento > new Date()) {
        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("Idade superior a idade máxima do plano");
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
    return true;
};

idadeMinimaPlano = (obj) => {

  // const contador = $(obj).attr("id").replace(/[^\d]/g, "");
  // const idadeMinima = parseInt($(`#hdnIdadeMinimaPlano${contador}`).val()) || 0;
  const idadeViajante = new Date().getFullYear() - (parseInt($(obj).val().split("/")[2]) || 0);

  if (idadeViajante < 0) {

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("Idade inferior a idade mínima do plano");
    return false;
  }

  $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
  return true;
};

viajanteDuplicado = (obj) => {

    const contador = $(obj).attr("id").replace(/\D/g, "");

    const nome = $(`#Nome${contador}`).val();

    const viajanteDuplicado = $("#lista-viajantes").find("div[class*='viajante']")
        .has("input[name*='Nome']:valueEquals(" + nome + ")")
        .has("input[name*='Cpf']:valueEquals(" + $(obj).val() + ")");

    if (viajanteDuplicado.length > 1) {
        
        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("Viajante duplicado");
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
    return true;
};

camposDiferentes = (obj) => {

    var objTarget = $(obj).attr('data-target-camposdiferentes');
    if ($(obj).val() === $(objTarget).val()) {

        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text($(obj).attr("data-message-camposdiferentes"));
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
    return true;
};

campoChecado = (obj) => {

    if (!$(obj).is(":checked")) {

        $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text($(obj).attr("data-message-checked"));
        return false;
    }

    $(`span[data-valmsg-for="${$(obj).attr("id")}"]`).text("");
    return true;
};


// ######################################### EXECUÇÃO
validaCampo = (input, formulario) => {
    
    let valid = true;
    let validacaoInput = true;
    const inputs = $(input).attr("valida").split(" ");

    inputs.map((tipoValidacao) => {

        if (!validacaoInput) return;

        const fn = window[tipoValidacao];

        if (typeof fn === "function") {
            
            if (!fn(input)) {

                valid = false;
                validacaoInput = false;
                return;
            }
        }
    });

    $(`${formulario ? formulario : ''} .valid:has(label.control-label span.control-label:not(:empty))`).has(input).removeClass("has-success").addClass("has-error");
    $(`${formulario ? formulario : ''} .valid:has(label.control-label span.control-label:empty)`).has(input).removeClass("has-error").addClass("has-success");

    return valid;
};

validaFormulario = (formulario) => {
    
    let validacaoFormulario = true;
    let seletor;

    seletor = `${formulario} [valida]`;

    $(seletor).each((index, input) => {

        if (!validaCampo(input, formulario)) {
            validacaoFormulario = false;
        }
    });

    return validacaoFormulario;
};

validacao = (formulario, opcoes) => {
    
    opcoes = opcoes || {};
    var botao = 'botao' in opcoes ? $(opcoes.botao) : $(formulario + ' button[type="submit"]');
    let objAcao = botao.attr('type') === 'submit' ? $(formulario) : botao;
    let tipoEvento = botao.attr('type') === 'submit' ? 'submit' : 'click';

    $(formulario).on('blur keyup change', ':input[valida]:not(:button), :input[data-toggle="dropdown"]', function (obj) {

        let target = obj.target;
        if ($(target).attr('data-toggle') == 'dropdown') {
            target = '#' + $(target).attr('data-id');
        }
        validaCampo(target, formulario);
    });

    objAcao.on(tipoEvento, function (e) {
        
        botao.button('loading');

        if (validaFormulario(formulario)) {

            if ('submit' in opcoes) {
                opcoes.submit(e);
            }
        }
        else {

            if ('invalido' in opcoes) {
                opcoes.invalido(e);
            }

            e.preventDefault();
            botao.button('reset');
        }
    });
};