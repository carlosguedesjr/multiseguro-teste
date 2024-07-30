function getQueryParam(param, defaultValue = undefined)
 {
    location.search.substr(1)
            .split("&")
            .some(function(item) { // returns first occurence and stops
                return item.split("=")[0] == decodeURIComponent(param) && (defaultValue = decodeURIComponent(item.split("=")[1]), true)
            })
            return defaultValue;
 }

function removeArrayItemNTimes(arr,toRemove){

    for(var i = 0; i < arr.length; i++){
        if(arr[i]==toRemove) {
            arr.splice(i,1);
            i--; // Prevent skipping an item
        }
    }
    return arr;
}

function mascaraValor(valor) {
    valor = valor.toString().replace(/\D/g,"");
    valor = valor.toString().replace(/(\d)(\d{8})$/,"$1.$2");
    valor = valor.toString().replace(/(\d)(\d{5})$/,"$1.$2");
    valor = valor.toString().replace(/(\d)(\d{2})$/,"$1,$2");
    return valor;                    
}

function FormataStringDataAmericana(data) {
  var dia  = data.split("/")[0];
  var mes  = data.split("/")[1];
  var ano  = data.split("/")[2];

  return ano + '-' + ("0"+mes).slice(-2) + '-' + ("0"+dia).slice(-2);
  // Utilizo o .slice(-2) para garantir o formato com 2 digitos.
}

function ConsultaReal(Limite)
{

    var Data = new Object();
    var Valores = new Object();
    var ValoresTexto = $('.operadora-vantagens-preco-total').text().replace(/R/g,"").replace(/\$/g, ' ').replace(/\,/g, '.').split(" ");
    var Destino = $('#Destino').val();

    if(Destino == 1){
        Destino = 'north-america';
    }
    if(Destino == 2){
        Destino = 'africa';
    }
    if(Destino == 4){
        Destino = 'south-america';
    }
    if(Destino == 3){
        Destino = 'central-america';
    }
    if(Destino == 8){
        Destino = 'oceania';
    }
    if(Destino == 9){
        Destino = 'middle-east';
    }
    if(Destino == 5){
        Destino = 'asia';
    }
    if(Destino == 6){
        Destino = 'brasil';
    }
    if(Destino == 7){
        Destino = 'europe';
    }

    var NumeroEmail = Math.floor(Math.random() * 99999);

    ValoresTexto = removeArrayItemNTimes(ValoresTexto, "");
    Data.extra = true;
    Data.lead_tag = "Site";
    Data.viagem = {
                    destino: Destino,
                    partida:$('#DataIda').val(),
                    retorno:$('#DataVolta').val()
                   };
    Data.cliente = {
                    nome: getQueryParam('Nome'),
                    telefone: getQueryParam('Telefone'),
                    email:getQueryParam('Email')
                   };

    var DataJSON = JSON.stringify(Data);
    var ConfereValor = 0;
    var bitCriado = 0;
    var settings = {
                  "async": true,
                  "crossDomain": true,
                  "url": "https://www.seguroviagem.srv.br/seguro-viagem/selecionar.json",
                  "method": "POST",
                  "headers": {
                    "Content-Type": "application/json",
                    "authorization": "Token token=b2fb092fe16ec8ca75f4d0e2bbe90f6170db3ff42c95fda8683a4ba2100c87ae",
                    "Accept": "*/*",
                    "Cache-Control": "no-cache",
                    "cache-control": "no-cache"
                  },
                  "processData": false,
                  "data": DataJSON
                }

                $.ajax(settings).done(function (response) {
                  
                    var urlRedirect = response.extra.url;
                  
                    var byPrice = response.collection.slice(0);

                    response.collection.sort(function(a,b) {
                        return a.preco_em_reais - b.preco_em_reais;
                    });

                    $.each( response.collection, function( key, value ) {
                     
                       bitCriado = 0;
                       ConfereValor = parseFloat(value.preco_em_reais);
                        //$.each( ValoresTexto, function( key2, value ) {
                            //if(ConfereValor.toFixed(2) < parseFloat(value))
                           // {
                                if(bitCriado == 0){
                                    $('#realseguros').append(`<div id="PlanoRealSeguros`+key+`"></div>`);
                                    bitCriado = 1;    
                                }
                           // }
                        //});
                         
                         if(key<Limite)
                         {
                            MontaHTMLReal(key, value, value.propriedades,urlRedirect);
                         }
                         else
                         {
                            return 0;
                         }
                         
                    });
                });
}

 
function MontaHTMLReal(cont, atributos, propriedades, urlRedirect)
{
    var confereAssistencia = 0;
    var confereBagagem = 0;
    urlRedirect = 'https:'+urlRedirect;
    var despesaMedicaValor = 'Não Possui'; 
    var perdaBagagemValor = 'Não Possui'; 
    $.each( propriedades, function( key, value ) {
        if(value.grupo == 'assistencia-medica'){
            if(confereAssistencia == 0){
                despesaMedicaValor = value.valor_h+",00";
                confereAssistencia++;
            }
        }
        if(value.grupo == 'bagagem'){
            if(confereBagagem == 0){
                perdaBagagemValor = value.valor_h+",00";
                confereBagagem++;
            }   
        }
    });

    var html = `<div class="cotacao-interno cotacao-interno-real-seguros`+cont+`">
        <div class="card">
            <div class="card-content cotacao-card">
                <span   data-toggle="tooltip" data-placement="top" glose="Este resultado é um dos nossos parceiros com um excelente atendimento e muitas compras com bom custo" class="ribbon4 tags" style="text-transform: uppercase;background-color: #ffb500;color: white;">PATROCINADO</span>
                <div class="col-md-2" style="text-align: center;">
                    <img title="" alt="" id="ImagemReal`+cont+`"  src="/www/img/operadoraRealSeguro.png" class="operadora">
                </div>

                <div class="col-md-2 operadora-titulo-box" style="margin-top: 3%;">
                    <h4 class="title operadora-titulo"  id="NomePlanoReal`+cont+`">`+atributos.nome_plano+`</h4>              
                </div>
                <div class="col-md-2 col-sm-6 col-xs-6 operadora-vantagens">
                    <i class="material-icons operadora-vantagens-icone">favorite</i>
                    <h6 class="title operadora-vantagens-title" id="AssistenciaMedicaReal`+cont+`">ASSISTÊNCIA MÉDICA</h6>
                    <h4 class="title operadora-vantagens-preco" id="AssistenciaMedicaValorReal`+cont+`">`+despesaMedicaValor+`</h4>
                </div>
                <div class="col-md-2 col-sm-6 col-xs-6 operadora-vantagens">
                    <i class="material-icons operadora-vantagens-icone">work</i>
                    <h6 class="title operadora-vantagens-title" id="PerdaBagagemReal`+cont+`">PERDA DE BAGAGEM</h6>
                    <h4 class="title operadora-vantagens-preco" id="PerdaBagagemRealValorReal`+cont+`">`+perdaBagagemValor+`</h4>
                </div>
                <div class="col-md-2 col-sm-12 col-xs-12 operadora-vantagens-preco">
                    <div class="operadora-vantagens-preco">
                        <h6 class="title operadora-vantagens-title">A PARTIR DE</h6>
                        <p class="title operadora-vantagens-preco-total" id="PrecoPlanoReal`+cont+`">R$ `+mascaraValor(atributos.preco_em_reais.toFixed(2))+`</p>
                     </div>
                </div>
                    <div class="col-md-2 col-sm-12 col-xs-12 operadora-vantagens-preco">
                            <a target="_blank" href="`+urlRedirect+`" class="btn btn-primary btn-comprar-real">
                                <i class="material-icons">shopping_cart</i> Comprar
                            </a>
                        <p class="title operadora-idade-comprar" id="IdadeMaximaReal`+cont+`">IDADE: `+atributos.idade_minima+` A `+atributos.idade_maxima+` ANOS</p>
                    </div>
                </div>
           </div>
      </div>`;
      $('#PlanoRealSeguros'+cont).append(html);
      
    return html;
}



