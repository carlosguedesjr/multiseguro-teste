class Viajante {

    constructor(nome, cpf, dataNascimento, sexo, valorUnitario, plano = "") {
        this.Nome = nome,
            this.Cpf = cpf,
            this.DataNascimento = dataNascimento,
            this.Sexo = sexo,
            this.ValorUnitario = parseFloat(valorUnitario) || 0,
            this.Plano = plano;
    }

    static calculaIdade(nascimento, dataIda) {
        var atual_nascimento = nascimento.split("/");
        var dataNascimento = new Date(parseInt(atual_nascimento[2], 10),
            parseInt(atual_nascimento[1], 10) - 1,
            parseInt(atual_nascimento[0], 10));

        var atual_ida = dataIda.split("/");
        var dataIda = new Date(parseInt(atual_ida[2], 10),
            parseInt(atual_ida[1], 10) - 1,
            parseInt(atual_ida[0], 10));

        var diferenca = dataIda.getTime() - dataNascimento.getTime();
        var idade = new Date(diferenca);


        idade = Math.abs(idade.getUTCFullYear() - 1970);

        return idade;

    }
};