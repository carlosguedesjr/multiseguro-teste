obtemCookieUsuario = function obtemCookieUsuario() {
    var cookies = document.cookie;

    if (cookies === "") return undefined;

    var cookieRd = cookies.split("rdtrk=")[1];

    if (cookieRd === undefined) return undefined;

    return JSON.parse(decodeURIComponent(cookieRd.split(";").shift())).id;
};

chamadaConversao = function chamadaConversao(datajson) {
    $.ajax({
        method: "POST",
        url: "https://www.rdstation.com.br/api/1.3/conversions",
        data: datajson,
        async: false,
        dataType: "json"
    });
};

conversaoRDStation = function conversaoRDStation(identificador) {
    var pagamento = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : false;


    var destinos = {
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
    var destino = destinos[$("#Destino").val()];

    var formasPagamento = {
        "1": "Itau Shopline",
        "3": "Rede"
    };
    var formaPagamento = formasPagamento[$("#OperadoraPagamento").val()];

    var email = $("#Email").val();
    if (!email) {
        email = getSession("email");
    }

    var dataIda = void 0;
    if ($("#DataIda").val()) {
        dataIda = $("#DataIda").val();
    }

    var dataVolta = void 0;
    if ($("#DataVolta").val()) {
        dataVolta = $("#DataVolta").val();
    }

    var emailReferencia = void 0;
    if (pagamento && email !== getSession("email")) {
        emailReferencia = getSession("email");
    }

    var nome = void 0;
    var complemento = void 0;
    var cpf = void 0;
    var telefone = void 0;
    var cep = void 0;
    var endereco = void 0;
    var bairro = void 0;
    var numero = void 0;
    var cidade = void 0;
    var estado = void 0;
    var contatoEmergencia = void 0;
    var telefoneEmergencia = void 0;

    if (pagamento) {
        nome = $("#NomeCompleto").val();
        cpf = $("#Documento").val();
        telefone = $("#TelefonePessoal").val();
        cep = $("#Cep").val();
        endereco = $("#Endereco").val();
        bairro = $("#Bairro").val();
        if ($("#Complemento").val()) {
            complemento = $("#Complemento").val();
        }
        numero = $("#Numero").val();
        cidade = $("#Cidade").val();
        estado = $("#Estado").val();
        contatoEmergencia = $("#NomeContato").val();
        telefoneEmergencia = $("#TelefoneContato").val();
    }

    var urlOrigem = getLocalStorage("utmSource") ? getLocalStorage("utmSource") : "Indefinido";
    var identificadorBanner = $('#Identificador').val() || "";

    var datajson = {
        "token_rdstation": "fc4864c74381a695bb88b6a77f7d3c49",
        "identificador": identificador,
        "email": email,
        "client_id": obtemCookieUsuario(),
        "website": window.location.origin + window.location.pathname,
        "tags": [urlOrigem, identificadorBanner],
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

    if (identificador === "Voucher" && getSession("emailPagamento") || identificador === "Voucher Avulso" && getSession("emailPagamento")) {
        datajson.email = getSession("emailPagamento");
        chamadaConversao(datajson);
    }
};
var _extends=Object.assign||function(e){for(var t=1;t<arguments.length;t++){var n=arguments[t];for(var o in n)Object.prototype.hasOwnProperty.call(n,o)&&(e[o]=n[o])}return e},_typeof="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(e){return typeof e}:function(e){return e&&"function"==typeof Symbol&&e.constructor===Symbol&&e!==Symbol.prototype?"symbol":typeof e};!function(e,t){"object"===("undefined"==typeof exports?"undefined":_typeof(exports))&&"undefined"!=typeof module?module.exports=t():"function"==typeof define&&define.amd?define(t):e.LazyLoad=t()}(this,function(){"use strict";var e=function(){return{elements_selector:"img",container:window,threshold:300,throttle:150,data_src:"src",data_srcset:"srcset",class_loading:"loading",class_loaded:"loaded",class_error:"error",class_initial:"initial",skip_invisible:!0,callback_load:null,callback_error:null,callback_set:null,callback_processed:null,callback_enter:null}},t=!("onscroll"in window)||/glebot/.test(navigator.userAgent),n=function(e,t){e&&e(t)},o=function(e){return e.getBoundingClientRect().top+window.pageYOffset-e.ownerDocument.documentElement.clientTop},i=function(e,t,n){return(t===window?window.innerHeight+window.pageYOffset:o(t)+t.offsetHeight)<=o(e)-n},s=function(e){return e.getBoundingClientRect().left+window.pageXOffset-e.ownerDocument.documentElement.clientLeft},r=function(e,t,n){var o=window.innerWidth;return(t===window?o+window.pageXOffset:s(t)+o)<=s(e)-n},l=function(e,t,n){return(t===window?window.pageYOffset:o(t))>=o(e)+n+e.offsetHeight},a=function(e,t,n){return(t===window?window.pageXOffset:s(t))>=s(e)+n+e.offsetWidth},c=function(e,t,n){return!(i(e,t,n)||l(e,t,n)||r(e,t,n)||a(e,t,n))},u=function(e,t){var n,o=new e(t);try{n=new CustomEvent("LazyLoad::Initialized",{detail:{instance:o}})}catch(e){(n=document.createEvent("CustomEvent")).initCustomEvent("LazyLoad::Initialized",!1,!1,{instance:o})}window.dispatchEvent(n)},d=function(e,t){return e.getAttribute("data-"+t)},h=function(e,t,n){return e.setAttribute("data-"+t,n)},f=function(e,t){var n=e.parentNode;if(!n||"PICTURE"===n.tagName)for(var o=0;o<n.children.length;o++){var i=n.children[o];if("SOURCE"===i.tagName){var s=d(i,t);s&&i.setAttribute("srcset",s)}}},_=function(e,t,n){var o=e.tagName,i=d(e,n);if("IMG"===o){f(e,t);var s=d(e,t);return s&&e.setAttribute("srcset",s),void(i&&e.setAttribute("src",i))}"IFRAME"!==o?i&&(e.style.backgroundImage='url("'+i+'")'):i&&e.setAttribute("src",i)},p="undefined"!=typeof window,m=p&&"classList"in document.createElement("p"),g=function(e,t){m?e.classList.add(t):e.className+=(e.className?" ":"")+t},v=function(e,t){m?e.classList.remove(t):e.className=e.className.replace(new RegExp("(^|\\s+)"+t+"(\\s+|$)")," ").replace(/^\s+/,"").replace(/\s+$/,"")},w=function(t){this._settings=_extends({},e(),t),this._queryOriginNode=this._settings.container===window?document:this._settings.container,this._previousLoopTime=0,this._loopTimeout=null,this._boundHandleScroll=this.handleScroll.bind(this),this._isFirstLoop=!0,window.addEventListener("resize",this._boundHandleScroll),this.update()};w.prototype={_reveal:function(e){var t=this._settings,o=function o(){t&&(e.removeEventListener("load",i),e.removeEventListener("error",o),v(e,t.class_loading),g(e,t.class_error),n(t.callback_error,e))},i=function i(){t&&(v(e,t.class_loading),g(e,t.class_loaded),e.removeEventListener("load",i),e.removeEventListener("error",o),n(t.callback_load,e))};n(t.callback_enter,e),"IMG"!==e.tagName&&"IFRAME"!==e.tagName||(e.addEventListener("load",i),e.addEventListener("error",o),g(e,t.class_loading)),_(e,t.data_srcset,t.data_src),n(t.callback_set,e)},_loopThroughElements:function(){var e=this._settings,o=this._elements,i=o?o.length:0,s=void 0,r=[],l=this._isFirstLoop;for(s=0;s<i;s++){var a=o[s];e.skip_invisible&&null===a.offsetParent||(t||c(a,e.container,e.threshold))&&(l&&g(a,e.class_initial),this._reveal(a),r.push(s),h(a,"was-processed",!0))}for(;r.length;)o.splice(r.pop(),1),n(e.callback_processed,o.length);0===i&&this._stopScrollHandler(),l&&(this._isFirstLoop=!1)},_purgeElements:function(){var e=this._elements,t=e.length,n=void 0,o=[];for(n=0;n<t;n++){var i=e[n];d(i,"was-processed")&&o.push(n)}for(;o.length>0;)e.splice(o.pop(),1)},_startScrollHandler:function(){this._isHandlingScroll||(this._isHandlingScroll=!0,this._settings.container.addEventListener("scroll",this._boundHandleScroll))},_stopScrollHandler:function(){this._isHandlingScroll&&(this._isHandlingScroll=!1,this._settings.container.removeEventListener("scroll",this._boundHandleScroll))},handleScroll:function(){var e=this._settings.throttle;if(0!==e){var t=Date.now(),n=e-(t-this._previousLoopTime);n<=0||n>e?(this._loopTimeout&&(clearTimeout(this._loopTimeout),this._loopTimeout=null),this._previousLoopTime=t,this._loopThroughElements()):this._loopTimeout||(this._loopTimeout=setTimeout(function(){this._previousLoopTime=Date.now(),this._loopTimeout=null,this._loopThroughElements()}.bind(this),n))}else this._loopThroughElements()},update:function(){this._elements=Array.prototype.slice.call(this._queryOriginNode.querySelectorAll(this._settings.elements_selector)),this._purgeElements(),this._loopThroughElements(),this._startScrollHandler()},destroy:function(){window.removeEventListener("resize",this._boundHandleScroll),this._loopTimeout&&(clearTimeout(this._loopTimeout),this._loopTimeout=null),this._stopScrollHandler(),this._elements=null,this._queryOriginNode=null,this._settings=null}};var b=window.lazyLoadOptions;return p&&b&&function(e,t){var n=t.length;if(n)for(var o=0;o<n;o++)u(e,t[o]);else u(e,t)}(w,b),w});
obtemParametroUri = function obtemParametroUri(nomeParametro) {

    return decodeURIComponent((new RegExp("[?|&]" + nomeParametro + "=([^&;]+?)(&|#|;|$)").exec(location.search) || [null, ""])[1].replace(/\+/g, "%20")) || null;
};

obtemParametroUriMvc = function obtemParametroUriMvc(index) {

    return window.location.pathname.split("/")[index];
};

isMobile = function isMobile(userAgent) {

    return (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(userAgent)
    );
};

setaUtmSource = function setaUtmSource() {
    var utmSource = obtemParametroUri('utm_source');

    if (utmSource != null) setLocalStorage('utmSource', utmSource);
};

setaCupomAfiliado = function setaCupomAfiliado() {
    var cupom = obtemParametroUri('cpaff') || obtemParametroUri('Cpaff');

    if (cupom != null) {
        window.sessionStorage.setItem('cpaff', cupom);
    }
};

validaViajantesCarrinho = function validaViajantesCarrinho(url) {
    var carrinho = getParseSession("carrinho");
    var planosQueFaltamViajante = [];

    carrinho.map(function (item) {
        return item.QuantidadeViajantes == 0 ? planosQueFaltamViajante.push(item.Planos.Nome) : false;
    });

    if (planosQueFaltamViajante.length > 0) {
        swal("Oops!", "O(s) plano(s) <b>" + planosQueFaltamViajante.join(", ") + "</b> est\xE3o sem viajantes!<br>Edite ou remova do seu carrinho de compras.", "warning");

        return false;
    } else {

        if (url != undefined) window.location.href = url;

        return true;
    }
};
validaDataSemana = function validaDataSemana(dataIda) {

    var data = new Date();
    data.setHours(0, 0, 0, 0);

    var diasSemana = ["Domingo", "Segunda", "Terca", "Quarta", "Quinta", "Sexta", "Sabado"];

    var dataSplit = dataIda.split("/");

    var diaIda = new Date(dataSplit[2], dataSplit[1] - 1, dataSplit[0]);

    var diaSemanaHoje = diasSemana[data.getDay()];

    var quantidadeProxDia = 2;

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

formataDataMobile = function formataDataMobile(data) {
    var dia = data.getUTCDate();
    var mes = data.getUTCMonth() + 1;

    return (dia < 10 ? "0" + dia : dia) + "/" + (mes < 10 ? "0" + mes : mes) + "/" + data.getUTCFullYear();
};

//========================================================== STORAGE
getParseSession = function getParseSession(chave) {
    return JSON.parse(window.sessionStorage.getItem(chave));
};
getSession = function getSession(chave) {
    return window.sessionStorage.getItem(chave);
};
setSession = function setSession(chave, valor) {
    window.sessionStorage.setItem(chave, valor);
};
removeSession = function removeSession(chave) {
    window.sessionStorage.removeItem(chave);
};
getLocalStorage = function getLocalStorage(chave) {
    return window.localStorage.getItem(chave);
};
getParseLocalStorage = function getParseLocalStorage(chave) {
    return JSON.parse(window.localStorage.getItem(chave));
};
setLocalStorage = function setLocalStorage(chave, valor) {
    window.localStorage.setItem(chave, valor);
};
removeLocalStorage = function removeLocalStorage(chave) {
    window.localStorage.removeItem(chave);
};
//========================================================== FIM STORAGE


//========================================================== MASCARAS
$(document).ready(function () {

    if ($().mask == undefined) return;

    (mascaraCep = function mascaraCep() {
        return $('input[mask-cep="true"]').mask("00000-000");
    })();

    (mascaraTelefone = function mascaraTelefone() {

        var options = {
            onKeyPress: function onKeyPress(tel, ev, el, op) {
                var masks = ["(00) 00000-0000", "(00) 0000-00009"],
                    mask = tel.length > 14 ? masks[0] : masks[1];
                el.mask(mask, op);
            }
        };

        $('input[mask-telefone="true"]').mask("(00) 0000-00000", options);
    })();

    (mascaraDocumento = function mascaraDocumento() {
        var options = {
            onKeyPress: function onKeyPress(cpf, ev, el, op) {
                var masks = ["000.000.000-000", "00.000.000/0000-00"],
                    mask = cpf.length < 15 ? masks[0] : masks[1];
                el.mask(mask, op);
            }
        };

        $('input[mask-documento="true"]').mask("000.000.000-000", options);
    })();

    (mascaraCpf = function mascaraCpf() {
        return $('input[mask-cpf="true"]').mask("000.000.000-00", { reverse: true });
    })();

    (mascaraDataNascimento = function mascaraDataNascimento() {
        return $('input[mask-data-nascimento="true"]').mask("00/00/0000");
    })();

    (mascaraCartaoCredito = function mascaraCartaoCredito() {
        return $('input[mask-cartao-credito="true"]').mask("0000 0000 0000 0099", { reverse: true });
    })();

    (mascaraAnoValidadeCredito = function mascaraAnoValidadeCredito() {
        return $('input[mask-ano-validade="true"]').mask("00/0099");
    })();

    (mascaraCodigoSegurancaCredito = function mascaraCodigoSegurancaCredito() {
        return $('input[mask-codigo-seguranca="true"]').mask("000");
    })();
});
//========================================================== FIM MASCARAS

converteDataMobile = function converteDataMobile(data) {
    var lang = window.navigator.userLanguage || window.navigator.language;
    return ("0" + data.getDate()).slice(-2) + "/" + ("0" + (data.getMonth() + 1)).slice(-2) + "/" + data.getFullYear();
};