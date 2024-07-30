//*************** INICIO MASCARAS - CARLOS NOGUEIRA 08/07/2015 ***************/

//MASCARAS
function mascara(o, f) 
{ 
    v_obj = o; 
    v_fun = f;
    if (typeof (v_obj.value) == "undefined") return "";
    setTimeout("execmascara()", 1);
};

function execmascara() 
{ 
    v_obj.value = v_fun(v_obj.value);
};

//PERTMITE APENAS NUMEROS
function soNumeros(v) 
{ 
    return v.replace(/\D/g, "");
};


//PERTMITE APENAS DECIMAIS
function mDecimal(v) {
    v = v.replace(/\D/g, "");
    v = new String(Number(v));
    var len = v.length;
    if (1 == len)
        v = v.replace(/(\d)/, "0.0$1");
    else if (2 == len)
        v = v.replace(/(\d)/, "0.$1");
    else if (len > 2) {
        v = v.replace(/(\d{2})$/, '.$1');
    }
    return v;
};

//MASCARA RG EX: 99.999.999-9
function mrg(v) 
{ 
    v = v.replace(/\D/g, ""); 
    v = v.substring(0, 11);
    v = v.replace(/(\d{2})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
    return v;
};

//MASCARA CPF EX: 999.999.999-99
function mcpf(v) 
{ 
    v = v.replace(/\D/g, ""); 
    v = v.substring(0, 11);
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
    return v;
};

//MASCARA CNPJ EX: 99.999.999/9999-99
function mcnpj(v) 
{ 
    v = v.replace(/\D/g, "");
    v = v.substring(0, 14); 
    v = v.replace(/^(\d{2})(\d)/, "$1.$2"); 
    v = v.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3"); 
    v = v.replace(/\.(\d{3})(\d)/, ".$1/$2"); 
    v = v.replace(/(\d{4})(\d)/, "$1-$2"); 
    return v;
};

function midentificador(v){   
    var identificador = soNumeros(v);
    if (identificador.length == 0) return "";

    if(identificador.length < 12){
        v = mcpf(identificador);
    } else { 
        v = mcnpj(identificador);
    }
    return v;
};

//MASCARA TELEFONE COM OPÇÃO EX: (99) 9999-9999
function mtelefoneFixo(v) {
    v = v.replace(/\D/g, "");
    v = v.replace(/^(\d{2})(\d)/g, "($1) $2");
    v = v.replace(/(\d)(\d{4})$/, "$1-$2");
    return v;
};


//MASCARA TELEFONE COM OPÇÃO PARA NONO DIGITO EX: (99) ?9999-9999
function mtelefone(v) {
    v = v.replace(/\D/g, "");
    v = v.replace(/^(\d{2})(\d)/g, "($1) $2");
    v = v.replace(/(\d)(\d{4})$/, "$1-$2");
    return v;
};
 
//MASCARA CEP EX: 99999-999
function mcep(v)
{
    v = v.replace(/\D/g, "");
    v = v.substring(0, 8);	
    v=v.replace(/^(\d{5})(\d)/,"$1-$2");
  return v;
};

//MASCARA DATA EX: DD/MM/AAAA
function mdata(v)
{ 
    v = v.replace(/\D/g, ""); 
    v = v.substring(0, 8);
    v = v.replace(/(\d{2})(\d)/, "$1/$2");
    v = v.replace(/(\d{2})(\d)/, "$1/$2");
    return v;
};

//MASCARA MES/ANO EX: MM/AAAA
function mMesAno(v) {
    v = v.replace(/\D/g, "");
    v = v.substring(0, 6);
    v = v.replace(/(\d{2})(\d)/, "$1/$2");
    return v;
};

//MASCARA HORA EX: 99:99
function mhora(v)
{ 
    v = v.replace(/\D/g, ""); 
    v = v.substring(0, 4);
    v = v.replace(/(\d{2})(\d)/, "$1:$2"); 
    return v;
};

//MASCARA NCM EX: 9999.99.99
function mncm(v) 
{ 
    v = v.replace(/\D/g, ""); 
    v = v.substring(0, 8);
    v = v.replace(/(\d{4})(\d)/, "$1.$2");
    v = v.replace(/(\d{2})(\d{1,2})$/, "$1.$2");
    return v;
};


//MASCARA CARTÂO DE CREDITO
function mcc(v) {
    v = v.replace(/\D/g, "");
    v = v.replace(/^(\d{4})(\d)/g, "$1 $2");
    v = v.replace(/^(\d{4})\s(\d{4})(\d)/g, "$1 $2 $3");
    v = v.replace(/^(\d{4})\s(\d{4})\s(\d{4})(\d)/g, "$1 $2 $3 $4");
    return v;
}


/*************** FIM MASCARAS **************/