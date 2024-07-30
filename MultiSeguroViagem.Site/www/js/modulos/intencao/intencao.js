class Intencao {

    constructor(destino, dataIda, dataVolta, email, origem, referrer) {
        this.destino = destino;
        this.dataIda = dataIda;
        this.dataVolta = dataVolta;
        this.email = email;
        this.origem = origem;
        this.referrer = referrer;
        this.ip = this.obtemIp();
    }

    obtemIp() {
        let ip;

        $.ajax({
            url: "https://api.ipify.org",
            async: false
        }).done(function (data) {
            ip = data;
        });

        return ip;
    }
}