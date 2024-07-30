filtraPlanos = (uri, tipoFiltro, filtro) => {

    const cotacoes = getParseSession('cotacao');
    var cont = 0;
    $.each( cotacoes.Planos, function( key, value ) {
            if(filtro != value.Operadora.Nome){
                delete value.RealSeguros;
            }else{
                if(cont > 0){ 
                    delete value.RealSeguros;
                }
            }
            cont++;
        });
   

    $.ajax({
        url: uri,
        data: {
            model: cotacoes,
            tipoFiltro: tipoFiltro,
            filtro: filtro
        },
        method: "post",
        success: data => $('#listaCotacoes').html(data),
        error: data => {
            console.log(data);
        }
    });

};

executaFiltros = (uri) => {
    
    $('.filtroJS').change(obj => { 
        $('#footerfixed').addClass('hidden');
        filtraPlanos(uri, obj.target.options[0].value, obj.target.value);
        
    });
};


downloadCoberturas = (plano, uriExterna, action) => {

    let idUpload;
    let dataCadastro;

    plano.Arquivos.map((obj) => {

        if (obj.TipoArquivo == 'LaminaCoberturas') {
            idUpload = obj.IdFileUploads;
            dataCadastro = obj.DataCadastro;
        }

    });

    if (idUpload != undefined) {

        var url = action + uriExterna + idUpload + '/' + dataCadastro;

        return window.location.origin + '/' + url;
    }
};


adicionaCarrinho = (dias, cotacao) => {

    var carrinho = [];

    if (getSession('carrinho')) {

        carrinho = getParseSession('carrinho');

        carrinho.map((obj, i) => {

            if (obj.Planos.IdPlano == cotacao.Planos[0].IdPlano && obj.Dias == dias) {
                carrinho.splice(i, 1);
                return false;
            }
        });

    }

    carrinho.push({
        Dias: dias,
        QuantidadeViajantes: 0,
        DataIda: cotacao.DataIda,
        DataVolta: cotacao.DataVolta,

        ValorTotal: 0,
        Planos: cotacao.Planos[0],
        Viajantes: null
    });

    setSession('carrinho', JSON.stringify(carrinho));

    $('.carrinhoCount').text(carrinho.length);
};

converteData = (data) => {
  var dt = data.split("/");
    return new Date(dt[2], dt[1] - 1, dt[0]);
};

calculaDias = (dataIda, dataVolta) => {  

   return Math.round((converteData(dataVolta) - converteData(dataIda)) / (1000*3600*24)) + 1;     
};

registraIntencaoCotacao = (uri, dataIda, dataVolta, destino) => {
    var dados = {
        DataIda: dataIda,
        DataVolta: dataVolta,
        Destino: destino,
        Email: window.localStorage.getItem('email')
    };

    if (getLocalStorage('IdVisitante'))
        dados.IdVisitante = getLocalStorage('IdVisitante');

    $.ajax({
        url: uri,
        method: 'post',
        dataType: "json",
        data: dados,
        success: function (data) {
            setLocalStorage('IdVisitante', data.IdVisitante);
        }
    });
};

