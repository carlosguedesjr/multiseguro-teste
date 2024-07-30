const cardCredits = [{
    nome: "Mastercard",
    colore: "#0061A8",
    src: "../www/img/cartoes/Mastercard-logo.png"
}, {
    nome: "Visa",
    colore: "#1c4671",
    src: "../www/img/cartoes/Visa-logo.png"
}, {
    nome: "Dinersclub",
    colore: "#888",
    src: "../www/img/cartoes/Diners-logo.png"
}, {
    nome: "Jcb",
    colore: "#108168",
    src: "../www/img/cartoes/JCB-logo.png"
}, {
    nome: "Hipercard",
    colore: "#b3131b",
    src: "../www/img/cartoes/Hipercard-logo.png"
}, {
    nome: "Generico",
    colore: "#696969",
    src: ""
}];

var month = 0;
var html = document.getElementsByTagName('html')[0];
var number = "";

var selected_cardCredit = -1;

$(document).click(function (e) {
    if (!$(e.target).is(".ccv") || !$(e.target).closest(".ccv").length) {
        $(".cardCredit").css("transform", "rotatey(0deg)");
        $(".seccode").css("color", "var(--text-color)");
    }
    if (!$(e.target).is(".expire") || !$(e.target).closest(".expire").length) {
        $(".date_value").css("color", "var(--text-color)");
    }
    if (!$(e.target).is(".number") || !$(e.target).closest(".number").length) {
        $(".cardCredit_number").css("color", "var(--text-color)");
    }
    if (!$(e.target).is(".inputname") || !$(e.target).closest(".inputname").length) {
        $(".fullname").css("color", "var(--text-color)");
    }
});


function addEventCardCredit() {

    $(".number").on('keyup', function () {

        number = mascaraCartaoCredito().val();

        $(".cardCredit_number").text(number);

        if (parseInt(number.substring(0, 2)) > 50 && parseInt(number.substring(0, 2)) < 56) {
            selected_cardCredit = 0;
        } else if (parseInt(number.substring(0, 1)) == 4) {
            selected_cardCredit = 1;
        } else if (parseInt(number.substring(0, 2)) == 36 || parseInt(number.substring(0, 2)) == 39 || parseInt(number.substring(0, 3)) == 301 || parseInt(number.substring(0, 3)) == 305) {
            selected_cardCredit = 2;
        } else if (parseInt(number.substring(0, 2)) == 35) {
            selected_cardCredit = 3;
        } else if (parseInt(number.substring(0, 2)) == 60) {
            selected_cardCredit = 4;
        } else if (parseInt(number.substring(0, 2)) == 38) {
            selected_cardCredit = 5;
        } else {
            selected_cardCredit = -1;
        }

        if (selected_cardCredit != -1) {
            html.setAttribute("style", "--cardCredit-color: " + cardCredits[selected_cardCredit].colore);

            if (cardCredits[selected_cardCredit].src != '')
                $(".bankid").attr("src", cardCredits[selected_cardCredit].src).show();

            if ($(".hdnBandeira")[0])
                $(".hdnBandeira").val(cardCredits[selected_cardCredit].nome);

        } else {
            html.setAttribute("style", "--cardCredit-color: #cecece");
            $(".bankid").attr("src", "").hide();

            if ($(".hdnBandeira")[0])
                $(".hdnBandeira").val('');
        }

        if ($(".cardCredit_number").text().length === 0) {
            $(".cardCredit_number").html("&#x25CF;&#x25CF;&#x25CF;&#x25CF; &#x25CF;&#x25CF;&#x25CF;&#x25CF; &#x25CF;&#x25CF;&#x25CF;&#x25CF; &#x25CF;&#x25CF;&#x25CF;&#x25CF;");
        }

    }).focus(function () {
        $(".cardCredit_number").css("color", "white");

    }).on("keydown input", function () {
        number = mascaraCartaoCredito().val();
        $(".cardCredit_number").text(number);
    });

    $(".inputname").keyup(function () {
        $(this).val($(this).val().toUpperCase());
        $(".fullname").text($(this).val());
        if ($(".inputname").val().length === 0) {
            $(".fullname").text("NOME COMPLETO");
        }
        return event.charCode;
    }).focus(function () {
        $(".fullname").css("color", "white");
    });

    $(".ccv").focus(function () {
        $(".cardCredit").css("transform", "rotatey(180deg)");
        $(".seccode").css("color", "white");
    }).keyup(function () {
        $(".seccode").text($(this).val());
        if ($(this).val().length === 0) {
            $(".seccode").html("&#x25CF;&#x25CF;&#x25CF;");
        }
    }).focusout(function () {
        $(".cardCredit").css("transform", "rotatey(0deg)");
        $(".seccode").css("color", "var(--text-color)");
    });

    $(".expire").keydown(function () {

    }).keyup(function (event) {
        $(".date_value").text($(this).val());

        if ($(this).val().length === 0) {
            $(".date_value").text("MM / AAAAA");
        }
    }).focus(function () {
        $(".date_value").css("color", "white");
    });
}
