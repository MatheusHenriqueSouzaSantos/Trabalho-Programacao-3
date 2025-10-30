namespace umfgcloud.programcaoiii.vendas.api.DTO
{
    public class ClienteDto
    {
        //[Required(ErrorMessage ="O nome é obrigatório")]
        public string Nome { get; set; }=string.Empty;
        //[Required(ErrorMessage = "O Cpf é obrigatório")]
        //[MaxLength(11)]
        //validar formato ou tirar . e -
        public string Cpf { get; set; } = string.Empty;
        //[Required(ErrorMessage = "O Endereço é obrigatório")]
        public string Endereco { get; set; } = string.Empty;
        //[Required(ErrorMessage = "O telefone é obrigatório")]
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
