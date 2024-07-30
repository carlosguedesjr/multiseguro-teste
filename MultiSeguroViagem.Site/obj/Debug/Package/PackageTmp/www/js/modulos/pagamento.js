populaCidades = (dropEstado, dropCidade, urn, cidadeSelecionada) => {

    $(dropCidade).empty();
    $(dropCidade).removeAttr("disabled");

    let codigoEstado = $(dropEstado).val().substr(0, 2);
    let uri = `//${urn}/www/json/cidades/${codigoEstado}.json`;

    $.getJSON(uri, (data) => {

        let listaCidades = data[codigoEstado];

        $.each(listaCidades, (key, val) => dropCidade.append($("<option />").val(val).text(val)));

        if (cidadeSelecionada != "") {

            $(dropCidade).val(cidadeSelecionada);

        }

        $(".selectpicker").selectpicker("refresh");
        $(dropCidade).trigger('blur');
    });
};


consultaCep = (dropEstado, dropCidade, cep) => {


    let uri = `https://api.cep.cenarioconsulta.com.br/logradouro/${cep}`;

    $.ajax({
        url: uri,
    })
        .done((logradouro) => {
            
            if (logradouro.code == 200) {
            let endereco = $("#Endereco");
            let bairro = $("#Bairro");

            $(endereco).val("");
            $(bairro).val("");
            $(dropCidade).empty();
            $(endereco).val(logradouro.body.logradouro);
            $(bairro).val(logradouro.body.bairro);        
            $(dropEstado).val(logradouro.body.estado.cod_ibge + logradouro.body.estado.uf);

            populaCidades(dropEstado, dropCidade, window.location.host, logradouro.body.cidade.cidade);

            $("#Endereco, #Bairro").trigger('blur');
                $(dropEstado).trigger('blur');

            }
        });

    

        


};


calculaValorTotal = () => {

    let valorTotal = 0;
    let valorParcial = parseFloat(($('#spValor').text()) ? $('#spValor').text() : '0');
    let valorDescontoBoleto = parseFloat(($('#spDescontoBoleto').text()) ? $('#spDescontoBoleto').text() : '0');
    let valorDescontoCupom = parseFloat(($('#spDescontoCupom').text()) ? $('#spDescontoCupom').text() : '0');

    valorTotal = valorParcial - (valorDescontoBoleto + valorDescontoCupom);

    $('#spTotal').text(valorTotal.toFixed(2));

    $('#hdValorDesconto').val(valorDescontoBoleto);
};


resumoPedido = () => {

    if (getSession('carrinho')) {

        objCarrinho = getParseSession('carrinho');

        $('#hdnCarrinho').val(getSession('carrinho'));

        objCarrinho.map((obj) => {

            valorPedido = valorPedido + obj.ValorTotal;
            
            var div = isMobile(navigator.userAgent) ? document.querySelector('#ResumoPedidoMobile') : document.querySelector('#ResumoPedido');
            $(div).empty();


            $(div).append($('#ResumoPedidoConteudo').clone(false).html());                

            $(div).find("#imgOperadora").prop("alt", obj.Planos.Operadora.Nome + " - Seguro Viagem").prop("title", obj.Planos.Operadora.Nome + " - Seguro Viagem").prop("src", "../www/img/operadoras/" + obj.Planos.Operadora.ImagemLogo);
            $(div).find("#nomePlano").text(obj.Planos.Nome);
            $(div).find("#destino").text(obj.Planos.Destino.Nome);
            $(div).find("#dataIda").text(obj.DataIda);
            $(div).find("#dataVolta").text(obj.DataVolta);

            var assistenciaMedica = obj.Planos.Coberturas[0].MoedaCobertura ? obj.Planos.Coberturas[0].MoedaCobertura + " " + parseFloat(obj.Planos.Coberturas[0].ValorCobertura).toFixed(2) : "-";
            var perdaBagagem = obj.Planos.Coberturas[1] ? obj.Planos.Coberturas[1].MoedaCobertura + " " + parseFloat(obj.Planos.Coberturas[1].ValorCobertura).toFixed(2) : "-";
            $(div).find("#assistenciaMedica").text(assistenciaMedica);
            $(div).find("#perdaBagagem").text(perdaBagagem);

            $(div).find("#btnViajantes").val(obj.Planos.IdPlano).text(obj.QuantidadeViajantes + " " + (obj.QuantidadeViajantes == 1 ? 'viajante' : 'viajantes') + " - Visualizar");
			
          
        });

        $('#spValor').text(valorPedido.toFixed(2));

        calculaValorTotal();
    }
};

selecionaTemplatePagamento = (template) => {

    if (template != undefined) {

        let div = document.querySelector('#OperadoraPagamentoCampos');

        $(div).empty();
        $(div).append($(template).clone(false).html());

        $('.selectpicker').selectpicker();

        if (isMobile(navigator.userAgent)) {
            $('.selectpicker').selectpicker('mobile');
        }

        $('input[mask-cartao-credito="true"]').mask("0000 0000 0000 0099", { reverse: true });
        $('input[mask-ano-validade="true"]').mask("00/0099");
        $('input[mask-codigo-seguranca="true"]').mask("000");

        addEventCardCredit();

        $('#Parcelas').change(() => calculaParcelasPagamento());

       if(template == '#RedeCard'){      
        let div = document.querySelector('#divCartao');

        $(div).empty();
        $(div).append($('#RedeCardCartao').clone(false).html());

      } else{
          let div = document.querySelector('#divCartao');
          $(div).empty();
        }
    }
};


defineTemplatePagamento = (tipoOperadora) => {

    if (tipoOperadora != "") {

        let operadoraPagamento = (tipoOperadora == '3') ? '#RedeCard' : '#Itau';

        if (operadoraPagamento == '#Itau') {

            let descontoBoleto = valorPedido * 0.05;
            let descontoCupom = parseFloat($('#hdnDescontoCupom').val()) || 0;

            $('#hdnDescontoTotal').val((descontoBoleto + descontoCupom).toFixed(2));

            $('#hdnDescontoBoleto').val(descontoBoleto.toFixed(2));
            $('#spDescontoBoleto').text(descontoBoleto.toFixed(2));
            $('#descontoBoleto').removeClass('hidden');
            $('#cartaoCredito').addClass('hidden');

        } else {

            let descontoCupom = parseFloat($('#hdnDescontoCupom').val()) || 0;

            $('#hdnDescontoTotal').val((descontoCupom + 0).toFixed(2));

            $('#descontoBoleto').addClass('hidden');
            $('#hdnDescontoBoleto').val('0');
            $('#spDescontoBoleto').text('');
        }

        selecionaTemplatePagamento(operadoraPagamento);
        calculaValorTotal();
    }
};


calculaParcelasPagamento = () => {

    if ($('#OperadoraPagamento').val() == '3') {

        let tipo = $('#Parcelas').val();
        let valorCompra = parseFloat($('#spTotal').text());

        if (tipo != '') {

            switch (tipo) {
                case "00":
                    $('#cartaoCredito').text('Pagamento no cartão de crédito à vista');
                    $('#cartaoCredito').removeClass('hidden');
                    break;

                default:
                    var qtdParcela = parseInt(tipo);
                    var valorParcela = valorCompra / qtdParcela;
                    $('#cartaoCredito').html(`Pagamento no cartão de crédito em ${qtdParcela}x de <a style='color: #3d5afe;'>R$ ${valorParcela.toFixed(2)}</a>`);
                    $('#cartaoCredito').removeClass('hidden');
                    break;
            }
        }
    }
};


removeCupom = () => {

    var valorTotalComDesconto = 0;
    var porcentBoleto = (parseFloat($('#spDescontoBoleto').text())) ? 0.05 : 0;
    var valorDescontoBoleto = (porcentBoleto != 0) ? valorPedido * porcentBoleto : 0;

    valorTotalComDesconto = valorPedido - (valorPedido * porcentBoleto);

    $('#hdnDescontoBoleto').val((valorPedido * porcentBoleto).toFixed(2));
    $('#spDescontoBoleto').text((valorPedido * porcentBoleto).toFixed(2));

    $('#hdnDescontoCupom').val('0.00');

    $('#hdnDescontoTotal').val(valorDescontoBoleto.toFixed(2));
    $('#spDescontoCupom').text('');
    $('#descontoCupom').addClass('hidden');
    $('#spTotal').text(valorTotalComDesconto.toFixed(2));
    $('#hdnCupom').val('');
};


verificaPlanoEstudante = () => {
    const carrinho = JSON.parse(window.sessionStorage.getItem('carrinho'));
    const { Estudante: planoEstudante } = carrinho[0].Planos;
    
    $('[name=Estudante]').val(planoEstudante);
    
    if (!planoEstudante) {
        $('[name=Estudante]').removeAttr("valida");
        $('.estudanteJs').toggleClass('hidden');
    }
}