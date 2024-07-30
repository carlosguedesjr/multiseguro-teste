function renderizaTemplateCartaoCredito() {
    const div = $("#divCartaoCredito");
    $(div).empty();
    $(div).append($("#TemplateRedeCard").clone(false).html());

    $(".selectpicker").selectpicker();

    if (isMobile(navigator.userAgent)) {
        $(".selectpicker").selectpicker("mobile");
    }

    $('input[mask-cartao-credito="true"]').mask("0000 0000 0000 0099", { reverse: true });
    $('input[mask-ano-validade="true"]').mask("00/0099");
    $('input[mask-codigo-seguranca="true"]').mask("000");
};

function calculaParcelasCartaoCredito() {
    const tipo = $("#Parcelas").val() || "00";

    if (tipo === "00") {
        $("#pValorCartaoCredito").text("Pagamento no cartão de crédito à vista");
        $("#pValorCartaoCredito").removeClass("hidden");

    } else {
        const valorCompra = parseFloat($("#spValorTotal").text());
        const qtdParcela = parseInt(tipo);
        const valorParcela = valorCompra / qtdParcela;

        $("#pValorCartaoCredito")
            .html(`Pagamento no cartão de crédito em ${qtdParcela}x de <a style='color: #3d5afe;'>R$ ${
                valorParcela.toFixed(2)}</a>`);
        $("#pValorCartaoCredito").removeClass("hidden");
    }
};

function atualizaResumoPedido(tipoOperadora) {
    if (tipoOperadora != "") {
        const operadoraPagamento = { "1": "#Itau", "3": "#RedeCard" };

        const valorTotalPedido = parseFloat($("#spValorParcial").text()) || 0;

        switch (operadoraPagamento[tipoOperadora]) {
            case "#Itau":
            {
                let descontoBoleto = valorTotalPedido * 0.05;

                $("#hdnValorDescontoBoleto").val(descontoBoleto.toFixed(2));
                $("#spValorDescontoBoleto").text(descontoBoleto.toFixed(2));
                $("#divCartaoCredito").empty();
                $("#pDescontoBoleto").removeClass("hidden");
                $("#pValorCartaoCredito").addClass("hidden");
                break;
            }
            case "#RedeCard":
            {
                $("#pDescontoBoleto").addClass("hidden");
                $("#hdnValorDescontoBoleto").val("0");
                $("#spValorDescontoBoleto").text("");

                renderizaTemplateCartaoCredito();

                $("#Parcelas").change(function() {
                    calculaParcelasCartaoCredito();
                });
            }
        }
        calculaValorTotal();
    }
}

function calculaValorTotal() {
    const valorParcial = parseFloat(($("#spValorParcial").text()) ? $("#spValorParcial").text() : "0");
    const valorDescontoBoleto = parseFloat(($("#spValorDescontoBoleto").text()) ? $("#spValorDescontoBoleto").text() : "0");
    const valorDescontoCupom = parseFloat(($("#spValorDescontoCupom").text()) ? $("#spValorDescontoCupom").text() : "0");

    const valorTotal = valorParcial - (valorDescontoBoleto + valorDescontoCupom);
    $("#spValorTotal").text(valorTotal.toFixed(2));
};