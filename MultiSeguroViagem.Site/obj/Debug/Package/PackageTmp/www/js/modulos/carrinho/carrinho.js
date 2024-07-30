class Carrinho {
  constructor(quantidadeViajantes, dataIda, dataVolta,  planos) {
    this.Dias = this.calculaDias(dataIda, dataVolta);
    this.QuantidadeViajantes = parseInt(quantidadeViajantes) || 0;
    this.DataIda = dataIda;
      this.DataVolta = dataVolta;

    this.ValorTotal = 0;
    this.Planos = planos;
    this.Viajantes = [];

    this.populaViajantes();
  }

  salva() {
    const carrinho = [];
    carrinho.push(this);

    setSession("carrinho", JSON.stringify(carrinho));
  }

  calculaDias (dataIda, dataVolta) {
    dataIda = dataIda.split("/");
    const dtIda = new Date(dataIda[2], dataIda[1] - 1, dataIda[0]);

    dataVolta = dataVolta.split("/");
    const dtVolta = new Date(dataVolta[2], dataVolta[1] - 1, dataVolta[0]);

    return Math.round((dtVolta - dtIda) / (1000 * 3600 * 24)) + 1;
  }

  populaViajantes() {
    for (let i = 1; i <= this.QuantidadeViajantes; i++) {
      const nome = $(`#Nome${i}`).val();
      const dataNascimento = $(`#DataNascimento${i}`).val();
      const cpf = $(`#Cpf${i}`).val();
      const sexo = $(`#Sexo${i}`).val();
      const valorUnitario = $(`#hdnValorUnitario${i}`).val();

      this.ValorTotal += parseFloat(valorUnitario) || 0;

      this.Viajantes.push(new Viajante(nome, cpf, dataNascimento, sexo, valorUnitario));
    }
  }
};
