templateViajanteDesktop = (contador) => {
    var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
    if (!isSafari) {
        return (`        
        <div class="row viajante-${contador}">
            <div class="col-md-3 valid">
                <input id="hdnValorUnitario${contador}" name="hdnValorUnitario${contador}" type="hidden" value="">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="Nome${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">account_circle</i>
                    </span>
                    <input id="Nome${contador}" name="Nome${contador}" class="form-control" data-message-empty="Informe o nome" valida="campoVazio nomeCompleto" maxlength="70" placeholder="Nome completo" type="text" value="">
                </div>
            </div>

            <div class="col-md-3 valid">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="DataNascimento${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">date_range</i>
                    </span>
                    <input id="hdnIdadeMaximaPlano${contador}" type="hidden" value="">
                    <input class="form-control" id="DataNascimento${contador}" name="DataNascimento${contador}" type="text" value="" maxlength="10" data-message-empty="Informe a data de nascimento" valida="campoVazio data idadeMaximaPlano idadeMinimaPlano" mask-data-nascimento="true" placeholder="Data de Nasc.">
                </div>
            </div>

            <div class="col-md-3 valid">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="Cpf${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">insert_drive_file</i>
                    </span>
                    <input id="Cpf${contador}" name="Cpf${contador}" autocomplete="off" class="form-control" data-message-empty="Informe o CPF" valida="campoVazio cpf viajanteDuplicado" mask-cpf="true" placeholder="CPF" type="text" value="" maxlength="14">
                </div>
            </div>        

            <div class="col-md-2 valid">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="Sexo${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">wc</i>
                    </span>
                    <div class="form-group select-button">
                        <select id="Sexo${contador}" name="Sexo${contador}" class="selectpicker" data-message-empty="Informe o sexo" valida="campoVazio" data-style="form-control" title="Sexo" tabindex="-98">
                            <option class="bs-title-option" value="">Sexo</option>
                            <option disabled="disabled">Selecione seu sexo</option>
                            <option value="M">Masculino</option>
                            <option value="F">Feminino</option>
                        </select>
                    </div>
                </div>
            </div>

            <div class="col-md-1">
                <button title="Remover viajante" class="btn btn-simple btn-danger remover-viajante" value="${contador}">
                    <i class="material-icons hidden-xs hidden-sm">cancel</i> <i class="material-icons hidden-md hidden-lg">remove</i> <span class="hidden-md hidden-lg">Remover viajante</span>
                </button>
            </div>            
        </div>
    `);
    }
    else {
        return (`        
        <div class="row viajante-${contador}">
            <div class="col-md-3 valid">
                <input id="hdnValorUnitario${contador}" name="hdnValorUnitario${contador}" type="hidden" value="">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="Nome${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <input id="Nome${contador}" name="Nome${contador}" class="form-control" data-message-empty="Informe o nome" valida="campoVazio nomeCompleto" maxlength="70" placeholder="Nome completo" type="text" value="">
                </div>
            </div>

            <div class="col-md-3 valid">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="DataNascimento${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <input id="hdnIdadeMaximaPlano${contador}" type="hidden" value="">
                    <input class="form-control" id="DataNascimento${contador}" name="DataNascimento${contador}" type="text" value="" maxlength="10" data-message-empty="Informe a data de nascimento" valida="campoVazio data idadeMaximaPlano idadeMinimaPlano" mask-data-nascimento="true" placeholder="Data de Nasc.">
                </div>
            </div>

            <div class="col-md-3 valid">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="Cpf${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <input id="Cpf${contador}" name="Cpf${contador}" autocomplete="off" class="form-control" data-message-empty="Informe o CPF" valida="campoVazio cpf viajanteDuplicado" mask-cpf="true" placeholder="CPF" type="text" value="" maxlength="14">
                </div>
            </div>        

            <div class="col-md-2 valid">
                <div class="form-group ">
                    <label class="control-label">
                        <span data-valmsg-for="Sexo${contador}" class="control-label" data-valmsg-replace="true"></span>
                    </label>
                </div>
                <div class="input-group">
                    <div class="form-group select-button">
                        <select id="Sexo${contador}" name="Sexo${contador}" class="selectpicker" data-message-empty="Informe o sexo" valida="campoVazio" data-style="form-control" title="Sexo" tabindex="-98">
                            <option class="bs-title-option" value="">Sexo</option>
                            <option disabled="disabled">Selecione seu sexo</option>
                            <option value="M">Masculino</option>
                            <option value="F">Feminino</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="col-md-1">
                <button title="Remover viajante" class="btn btn-simple btn-danger remover-viajante" value="${contador}">
                    <i class="material-icons hidden-xs hidden-sm">cancel</i> <i class="material-icons hidden-md hidden-lg">remove</i> <span class="hidden-md hidden-lg">Remover viajante</span>
                </button>
            </div>            
        </div>
    `);

    }
};

templateAlerta = (mensagem, tipoIcone) => {

    const arrayIcones = {
        "person": '<i class="material-icons">person</i>',
        "error": '<i class="material-icons">error_outline</i>'
    };

    return (`
        <div class="row">
            <div class="alert alert-${tipoIcone === "error" ? "danger" : "info"}">
                <div class="container-fluid">
                    <div class="alert-icon">
                        ${arrayIcones[tipoIcone]}
                    </div>
                    ${mensagem}
                </div>
            </div>
        </div>
    `);
};


renderTemplateViajanteDesktop = (elemento, contador) => {

    const markup = templateViajanteDesktop(contador);
    $(elemento).append(markup);

    $(`#Sexo${contador}.selectpicker`).selectpicker("refresh");

    mascaraDataNascimento();
    mascaraCpf();

    removeViajanteDesktop(contador);
    adicionaViajanteDesktop(contador);

    $("#contador").val(contador);

    if (contador == 1) $('.remover-viajante[value="1"]').addClass("hidden");

    if (isMobile(navigator.userAgent)) {

        $(`.remover-viajante[value="${contador}"]`).removeClass("btn-simple");
        $(`.remover-viajante[value="${contador}"]`).addClass("btn-block");
    }
};

rendertemplateAlerta = (elemento, mensagem, tipoIcone) => {

    const markup = templateAlerta(mensagem, tipoIcone);

    $(elemento).empty().append(markup);
};



validaIdadeMaximaDoPlano = (idade, idadeMaximaPlano, melhorIdadeMaximaPlano) => {

    return idade > (melhorIdadeMaximaPlano || idadeMaximaPlano);
};


validaMelhorIdade = (idade, melhorIdadeMin, melhorIdadeMax) => {

    if (melhorIdadeMin == 0) {
        return false;
    }

    return (idade >= melhorIdadeMin) && (idade <= melhorIdadeMax);
};


validaReferrer = () => {
    if ((document.referrer).toLowerCase().indexOf("cotador/comparador") <= 0 && (document.referrer).toLowerCase().indexOf("cotador/cotacao") <= 0) {

        const cotacao = getParseSession("cotacao");
        let uri;

        cotacao ? uri = `/Cotador/Cotacao?Destino=${cotacao.Destino}&DataIda=${cotacao.DataIda}&DataVolta=${cotacao.DataVolta}` : uri = "/Home/Index";

        window.location.href = uri;
    }

};

atualizaValorTotal = function atualizaValorTotal(valor) {
    var operador = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "+";
    var contador = arguments[2];

    var total = parseFloat($(".preco-total span").text()) || 0;
    var unitario = parseFloat($('#hdnValorUnitario' + contador).val()) || 0;
    var acrescimo = (parseFloat(valor) || 0) * parseInt(operador + '1');

    total += acrescimo;
    unitario += acrescimo;
    var boleto = ((total/100)*5 - total) *-1;
    $(".preco-total span").text(total.toFixed(2));
    $(".boleto").text(boleto.toFixed(2).replace(".",","));
    $(".parcela").text(total.toFixed(2).replace(".",","));
    $('#hdnValorUnitario' + contador).val(unitario.toFixed(2));
};



adicionaViajanteDesktop = (contador) => {
    const cotacao = getParseSession("cotacao");
    const plano = planoSelecionado;
    let flagMelhorIdade = false;

    $(`#hdnIdadeMaximaPlano${contador}`).val(plano.MelhorIdadeMaxima || plano.IdadeMaxima);

    $(`#DataNascimento${contador}`).on("blur", function (evt) {

        $(".msg-alerta").empty();

        const dataNascimento = $(`#DataNascimento${contador}`).val();
        if (!dataNascimento) return false;

        const anoNascimento = $(`#DataNascimento${contador}`).val().split("/")[2];
        const anoIdaViagem = cotacao.DataIda.split("/")[2];
        const NascimentoCompleto = $(`#DataNascimento${contador}`).val();
        const DataIdaCompleta = cotacao.DataIda;

        const idade = Viajante.calculaIdade(NascimentoCompleto, DataIdaCompleta);

        if (validaIdadeMaximaDoPlano(idade, plano.IdadeMaxima, plano.MelhorIdadeMaxima) || idade < 0) {

            rendertemplateAlerta($(".msg-alerta"), "A idade do viajante não é suportada pelo plano. Ajuste a idade ou volte e escolha outro plano!", "error");
            flagMelhorIdade = false;
            return false;
        }

        const idoso = validaMelhorIdade(idade, plano.MelhorIdadeMinima, plano.MelhorIdadeMaxima);

        const acrescimoMelhorIdade = parseFloat(plano.PrecoMelhorIdade) - parseFloat(plano.PrecoComercial) || 0;

        if (flagMelhorIdade !== idoso) {

            flagMelhorIdade = idoso;

            let operador;

            idoso ? operador = "+" : operador = "-";

            atualizaValorTotal(acrescimoMelhorIdade || parseFloat(plano.PrecoComercial.replace(",", ".")), operador, contador);
        }

        if (idoso) {

            rendertemplateAlerta($(".msg-alerta"), `A idade do viajante é superior à ${plano.MelhorIdadeMinima} anos. Devido a isso, será acrescido o valor de R$ ${acrescimoMelhorIdade.toFixed(2)} conforme condições da operadora.`, "person");

        }
    });

    atualizaValorTotal(plano.PrecoComercial.replace(",", "."), "+", contador);
};

removeViajanteDesktop = (contador) => {

    $(`.remover-viajante[value="${contador}"]`).on("click", function (evt) {

        const unitario = $(`#hdnValorUnitario${contador}`).val() || 0;

        atualizaValorTotal(unitario, "-", contador);

        $(`.viajante-${evt.currentTarget.value}`).remove();
        $(".msg-alerta").empty();

        $("#contador").val(parseInt($("#contador").val()) - 1);

        if (isMobile(navigator.userAgent)) {
            $(`#lista-viajantes .viajante-${contador - 1}`).removeClass("hidden");
        }
    });
};
planoSelecionado = (urn) => {

    const idPlanoUrl = parseInt(obtemParametroUriMvc(3)) || 0;
    const cotacao = getParseSession("cotacao");

    let planoSelecionado;

    cotacao.Planos.map((plano) => {

        if (idPlanoUrl === plano.IdPlano) {
            plano.MelhorIdadeValorAcrescimo = plano.MelhorIdadeValorAcrescimo.replace(',', '.');
            plano.Preco = plano.Preco.replace(',', '.');
            plano.PrecoComercial = plano.PrecoComercial.replace(',', '.');
            plano.PrecoMelhorIdade = plano.PrecoMelhorIdade.replace(',', '.');

            planoSelecionado = plano;
        }
    });

    if (planoSelecionado != undefined)
        return planoSelecionado;

    var uri = `${urn + cotacao.Destino}&DataIda=${cotacao.DataIda}&DataVolta=${cotacao.DataVolta}`;

    window.location.href = uri;
};



setaDescricaoViajantes = () => {

    const cotacao = getParseSession("cotacao");

    const destinos = {
        "1": "América do Norte",
        "2": "África",
        "3": "América Central",
        "4": "América do Sul",
        "5": "Ásia",
        "6": "Brasil",
        "7": "Europa",
        "8": "Oceania",
        "9": "Oriente Médio"
    };

    $("#destino").text(destinos[cotacao.Destino]);
    $("#dataIda").text(cotacao.DataIda);
    $("#dataVolta").text(cotacao.DataVolta);
    $("#Plano").text(planoSelecionado.Operadora.Nome + " - " + planoSelecionado.Nome);
};