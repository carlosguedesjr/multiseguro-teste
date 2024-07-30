(trackingAfiliace = () => {
   
    let $iframe = $('iframe[frame-track="true"]');

    if ($iframe.length) {

        let afiliado = obtemParametroUri('aff');
        let oferta = obtemParametroUri('off');

        if (afiliado != null && oferta != null) {
            var uri = `https://afiliace.go2cloud.org/aff_c?offer_id=${oferta}&aff_id=${afiliado}`;
            $iframe.attr('src', uri);
        }
    };

})();