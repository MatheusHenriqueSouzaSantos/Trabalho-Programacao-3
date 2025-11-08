namespace umfgcloud.programcaoiii.vendas.api.DTO
{
    public class ClienteDto
    {
        public string Nome { get; set; }=string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; }= string.Empty;

        public ClienteDto()
        {

        }

        public ClienteDto(string nome, string cpf, string endereco, string telefone)
        {
            Nome = nome;
            Cpf = cpf;
            Endereco = endereco;
            Telefone = telefone;
        }
    }
}
