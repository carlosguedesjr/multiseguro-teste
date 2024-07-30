(validaForm = (form) => {

    var validacao = true;
    var msg = '';   

    $(':input[data-message-empty]', form).not(':button, :submit, :reset, :hidden')  
        .each(function (obj) { 

            //trata input checkbox
            if ($(this)[0].type == 'checkbox') {

                if (!$(this)[0].checked) {
                    validacao = false;
                    $(`span[data-valmsg-for="${$(this).attr('id')}"]`).text($(this).attr('data-message-empty'));
                }
                else
                    $(`span[data-valmsg-for="${$(this).attr('id')}"]`).text('');

            } else {
                if (!$(this).val()) {

                    validacao = false;
                    $(`span[data-valmsg-for="${$(this).attr('id')}"]`).text($(this).attr('data-message-empty'));
                }
                else {

                    if ($(this).attr('data-type-validation')) {

                        var tipo = $(this).attr('data-type-validation');

                        if (tipo == 'cpf') {

                            if (!valida(this, validaCPF))
                                validacao = false;
                        }

                        if (tipo == 'telefone') {

                            if (!valida(this, validaTelefone))
                                validacao = false;
                        }

                        if (tipo == 'data') {

                            if (!valida(this, valida_Data))
                                validacao = false;
                        }

                        if (tipo == 'cpfCnpj') {

                            if (!valida(this, valida_CPFCNPJ))
                                validacao = false;
                        }

                        if (tipo == 'nomeCompleto') {

                            if (!valida(this, validaNomeCompleto))
                                validacao = false;
                        }

                        if (tipo == 'email') {
                           

                            if (!valida(this, validaEmail))
                                validacao = false;
                        }

                        if (tipo == 'dataMaximaPlano') {

                            if (!valida(this, validaDataMaximaPlano))
                                validacao = false;
                        }
                    }
                    else
                        $(`span[data-valmsg-for="${$(this).attr('id')}"]`).text('');
                }
            }
        });
    console.log(validacao);
    return validacao;
});

//validacoes
function valida(obj, func) {
    
    if (!func(obj)) {
        if (func == validaCPF)
            $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('CPF inválido');

        if (func == validaTelefone)
            $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('Telefone inválido');

        if (func == valida_CPFCNPJ)
            $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('CPF/CNPJ inválido');

        if (func == valida_Data)
            $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('Data inválida');

        if (func == validaNomeCompleto)
            $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('Digite o nome completo');

        if (func == validaEmail)
            $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('O campo Email não é um endereço de email válido');

        if (func == validaDataMaximaPlano)
            $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('É TETRAAAAAAAAAAAAAA');

        return false;
    }
    else
        $(`span[data-valmsg-for="${$(obj).attr('id')}"]`).text('');

    return true;
};

//Valida nome completo
function validaNomeCompleto(obj) {

    var RegExPattern = /([A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ]{2,}([ A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ'`.]{0,}?)[ ]{1,}([ A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ'`]{0,}?)[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ]{2,})/;

    if (!((obj.value.match(RegExPattern)) && (obj.value != ''))) {
        return false;
    }
    return true;
}

//Data
function valida_Data(obj) {
    var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;

    if (!((obj.value.match(RegExPattern)) && (obj.value != ''))) {
        return false;
    }
    return true;
}

//valida cpf
function validaCPF(obj) {

    var val = obj.value.replace(/\D+/g, '');

    if (val.length == 11) {
        return valida_CPF(val);
    } 
    else {
        return false;
    }

}

//valida Telefone
function validaTelefone(obj) {
    var val = obj.value;
    if (val.length >= 14) {
        return val;
    } 
    else {
        return false;
    }

}

// Validação de CPF e CNPJ
function valida_CPFCNPJ(obj) {

    var val = obj.value.replace(/\D+/g, '');

    if (val.length == 11) {
        return valida_CPF(val);
    } else if (val.length >= 14) {
        return valida_CNPJ(val);
    } else {
        return false;
    }

}

//Validação de CPF
function valida_CPF(obj) {

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

}

//Validação de CNPJ
function valida_CNPJ(obj) {

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

}

//Valida e-mail
function validaEmail(obj)
{
    var RegExPattern = /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
  if (!((obj.value.match(RegExPattern)) && (obj.value != ''))) {
        return false;
    }
    return true;
}



function validaDataMaximaPlano(obj) {
    var RegExPattern = /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
  if (!((obj.value.match(RegExPattern)) && (obj.value != ''))) {
        return false;
    }
    return true;
}