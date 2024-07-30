class IntencaoCompra extends Intencao {

    constructor(destino, dataIda, dataVolta, email, origem, referrer, viajantes, plano) {
        super(destino, dataIda, dataVolta, email, origem, referrer);

        this.viajantes = [];

        const self = this;
        viajantes.map(viajante => self.viajantes.push(new Viajante(viajante.Nome, viajante.Cpf, viajante.DataNascimento, viajante.Sexo, viajante.ValorUnitario, plano)));
    }
}